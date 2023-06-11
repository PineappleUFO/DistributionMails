using Core.Models;
using EF.Interfaces;
using Npgsql;
using PostgresRepository.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgresRepository.Repositories
{
    public class OutgoingMailRepository : IOutgoingRepository
    {
        IConnectionString connectionString;
        public OutgoingMailRepository(IConnectionString connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<List<OutgoingMail>> GetAllOutputMail()
        {
            //если по какой то причине строка подключения пустая
            if (connectionString == null)
                throw new Exception("Не задана строка подключения");

            var result = new List<OutgoingMail>();
            await using var connection = connectionString.TryGetConnetion();
            await connection.OpenAsync();

            try
            {
                await using var command = connection.CreateCommand();
                command.CommandText = @"select
        om.mail_id,  
        om.number,   
        om.date_export,
        om.date_answer,
        om.theme,
        p.sender_name as project_name,
        p.project_id,
        p.project_color,
        s.sender_id,
        s.sender_name,
    om.text
            from incoming_mail m
        left join users u on u.user_id = m.responsible
        inner join projects p on m.id_project=p.project_id
        inner join sender s on m.id_sender = s.sender_id
        right join outgoing_mail om on m.id_outgoing_mail = om.mail_id
        order by om.date_export desc";

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    
                    //Получаем отправителя
                    Sender sender = new Sender();
                    if (reader["sender_id"] != DBNull.Value)
                    {
                        sender.Id = Convert.ToInt32(reader["sender_id"]);
                        sender.Name = reader["sender_name"].ToString();
                    }

                    Project project = new Project();

                    if (reader["project_id"] != DBNull.Value)
                    {
                        //Получаем цвет проекта, если есть
                        string colorProject = default;
                        if (reader["project_color"] != DBNull.Value)
                        {
                            colorProject = $"#{reader["project_color"].ToString()}";
                        }

                        project = new Project(Convert.ToInt32(reader["project_id"]), reader["project_name"].ToString(), colorProject);
                    }

                    DateTime? date_answer = null;
                    if (reader["date_answer"] != DBNull.Value)
                    {
                        date_answer = Convert.ToDateTime(reader["date_answer"]);
                    }

                    var mail = new OutgoingMail();
                    mail.Id = Convert.ToInt32(reader["mail_id"]);
                    mail.Number = reader["number"].ToString();
                    mail.DateExport = Convert.ToDateTime(reader["date_export"]);
                    mail.Sender = sender;
                    mail.Project = project;
                    mail.Text = reader["text"].ToString();
                    mail.Theme = reader["theme"].ToString();

                    //генерируем предпологаемый путь до файлов  (в продакшнене тут ссылка на файловый сервер, в данном случае сделана динамическая ссылка на папку проекта и в ней папка с pdf симулирующая файловый сервер)
                    var CurentPath = new DirectoryInfo(Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
                        ?.Parent
                        ?.Parent
                        ?.Parent
                        ?.Parent
                        ?.Parent
                        ?.Parent
                        .FullName, ".FileServer", "Output", reader.GetInt32(0).ToString()));

                    //если путь существует то добавляем его к письму
                    if (CurentPath.Exists)
                    {
                        mail.PathFolder = CurentPath;
                    }


                    result.Add(mail);
                }
            }
            catch (NpgsqlException e)
            {
                //todo: логирование
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                await connection.CloseAsync();
            }

            //Если подключение не корректно 
            return result;
        }
    }
}
