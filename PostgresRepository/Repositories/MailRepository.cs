﻿using Core.Models;
using Npgsql;
using PostgresRepository.Interfaces;
using PostgresRepository.PostgresCommon;

namespace EF.Repositories;

public class MailRepository : IMailRepository
{
    public async Task<List<Mail>> GetAllMails()
    {
        string query = @"select 
        m.mail_id,  --0
        m.number,   --1
        m.date_input,--2
        m.date_answer,--3
        m.theme,--4
        u.user_id,--5
        u.family,--6
        p.project_id,--7
        p.sender_name,--8
        p.project_color,--9
        s.sender_id,--10
        s.sender_name,--11
        om.mail_id as owMail,--12
        om.number,--13
        om.date_export,--14
        om.date_answer as owDate_answer,--15
        om.theme,--16
        om.text--17
            from incoming_mail m
        left join users u on u.user_id = m.responsible
        inner join projects p on m.id_project=p.project_id
        inner join sender s on m.id_sender = s.sender_id
        left join outgoing_mail om on m.id_outgoing_mail = om.mail_id
        order by m.date_input desc";

        return await loadMailByQuery(query);
    }

    public async Task<List<Mail>> GetArchiveUser(User user)
    {
        string query = @"select
        m.mail_id,  --0
        m.number,   --1
        m.date_input,--2
        m.date_answer,--3
        m.theme,--4
        u.user_id,--5
        u.family,--6
        p.project_id,--7
        p.sender_name,--8
        p.project_color,--9
        s.sender_id,--10
        s.sender_name,--11
        om.mail_id as owMail,--12
        om.number,--13
        om.date_export,--14
        om.date_answer as owDate_answer,--15
        om.theme,--16
        om.text--17
            from incoming_mail m
        left join users u on u.user_id = m.responsible
        inner join projects p on m.id_project=p.project_id
        inner join sender s on m.id_sender = s.sender_id
        inner join users_archive_mails du on m.mail_id=du.id_mail
        left join outgoing_mail om on m.id_outgoing_mail = om.mail_id
        where du.id_user=157
        order by m.date_input desc";

        return await loadMailByQuery(query);
    }

    public async Task<List<Mail>> GetDistributedToUser(User user)
    {
        string query = $@"select
        m.mail_id,  --0
        m.number,   --1
        m.date_input,--2
        m.date_answer,--3
        m.theme,--4
        u.user_id,--5
        u.family,--6
        p.project_id,--7
        p.sender_name,--8
        p.project_color,--9
        s.sender_id,--10
        s.sender_name,--11
        om.mail_id as owMail,--12
        om.number,--13
        om.date_export,--14
        om.date_answer as owDate_answer,--15
        om.theme,--16
        om.text--17
            from incoming_mail m
        left join users u on u.user_id = m.responsible
        inner join projects p on m.id_project=p.project_id
        inner join sender s on m.id_sender = s.sender_id
        inner join distributed_to_user du on m.mail_id=du.id_mail
        left join outgoing_mail om on m.id_outgoing_mail = om.mail_id
        where du.id_user={user.Id}
        order by m.date_input desc";

        return await loadMailByQuery(query);
    }


    /// <summary>
    /// Функция получения писем
    /// </summary>
    private async Task<List<Mail>> loadMailByQuery(string query)
    {
        //если по какой то причине строка подключения пустая
        if (string.IsNullOrWhiteSpace(PostgresConnectionString.ConnectionString))
            throw new Exception("Не задана строка подключения");

        var result = new List<Mail>();
        await using var connection = new NpgsqlConnection(PostgresConnectionString.ConnectionString);
        await connection.OpenAsync();

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = query;

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                //получаем ответсвенного, если есть
                var responsible = new User();
                if (reader["user_id"] != DBNull.Value)
                {
                    responsible.Id = Convert.ToInt32(reader["user_id"]);
                    responsible.Family = reader.GetString(6);
                }



                //Получаем отправителя
                Sender sender = new Sender();
                if (reader["sender_id"] != DBNull.Value)
                {
                    sender.Id = Convert.ToInt32(reader["sender_id"]);
                    sender.Name = reader.GetString(11);
                }

                OutgoingMail outMail = new OutgoingMail();
                //Получаем ответ на письмо
                if (reader["owMail"] != DBNull.Value)
                {
                    outMail = new OutgoingMail(reader.GetInt32(12), reader.GetString(13), reader.GetDateTime(14),
                        reader.GetDateTime(15), reader.GetString(16), reader.GetString(17));
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

                    project = new Project(reader.GetInt32(7), reader.GetString(8), colorProject);
                }

                DateTime? date_answer = null;
                if (reader["date_answer"] != DBNull.Value)
                {
                    date_answer = Convert.ToDateTime(reader["date_answer"]);
                }
                var mail = new Mail(reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2),
                    date_answer, reader.GetString(4),
                    responsible, project, sender, outMail);

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