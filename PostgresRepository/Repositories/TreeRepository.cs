using Core.Models;
using EF.Interfaces;
using Npgsql;
using PostgresRepository.PostgresCommon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Repositories
{
    public class TreeRepository : ITreeRepository
    {
        IConnectionString connectionString;
        public TreeRepository(IConnectionString connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddOneLevelDistributionInMail(Mail mail,User user,DateTime deadline,string resolution,bool isResponible, bool isReplying)
        {
            //если по какой то причине строка подключения пустая
            if (connectionString == null)
                throw new Exception("Не задана строка подключения");
            using var connection = connectionString.TryGetConnetion();
            connection.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;

                cmd.CommandText = "INSERT INTO distribution_tree (id_mail, id_user, deadline, resolution, is_responsible, is_replying, log) VALUES (@id_mail, @id_user, @deadline, @resolution, @is_responsible, @is_replying, @log) ON CONFLICT DO NOTHING";
                cmd.Parameters.AddWithValue("id_mail", mail.Id);
                cmd.Parameters.AddWithValue("id_user", user.Id);
                cmd.Parameters.AddWithValue("deadline", deadline);
                cmd.Parameters.AddWithValue("resolution", resolution);
                cmd.Parameters.AddWithValue("is_responsible", isResponible);
                cmd.Parameters.AddWithValue("is_replying", isReplying);
                cmd.Parameters.AddWithValue("log", "Добавление 1 уровня распределения");

                cmd.ExecuteNonQuery();
            }
        }

        public async Task<List<DistributionTreeElement>> GetTreeByMailId(Mail mail)
        {


            //если по какой то причине строка подключения пустая
            if (connectionString == null)
                throw new Exception("Не задана строка подключения");

            var result = new List<DistributionTreeElement>();
            await using var connection = connectionString.TryGetConnetion();
            await connection.OpenAsync();

            try
            {
                await using var command = connection.CreateCommand();
                command.CommandText =
                    $@"select t.id,
       t.id_mail,
       u.user_id,
       u.family,
       u.name,
       u.surname,
       (SELECT CONCAT(LEFT(u.name, 1), '.', LEFT(u.surname, 1),'.')) AS inicials,
       t.id_status,
       t.up_id,
       t.deadline,
       t.resolution,
       t.is_responsible,
       t.is_replying,
       t.date_add,
       t.log
from distribution_tree t
inner join users u on u.user_id = t.id_user
where t.id_mail = {mail.Id}";

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var distr = new DistributionTreeElement();
                    var user = new User();
                    user.Id = Convert.ToInt32(reader["user_id"]);
                    user.Name = Convert.ToString(reader["name"]);
                    user.Family = Convert.ToString(reader["Family"]);
                    user.Surname = Convert.ToString(reader["Surname"]);
                    user.Inicials = Convert.ToString(reader["inicials"]);

                    distr.Id = Convert.ToInt32(reader["id"]);
                    distr.User = user;
                    distr.MailId = Convert.ToInt32(reader["id_mail"]);

                    int upId = default;
                    if(reader["up_id"] != DBNull.Value)
                        upId = Convert.ToInt32(reader["up_id"]);

                    distr.UpId = upId;

                    bool isResponible = false;
                    if (reader["is_responsible"] != DBNull.Value)
                        isResponible = Convert.ToBoolean(reader["is_responsible"]);

                    bool isReplying = false;
                    if (reader["is_replying"] != DBNull.Value)
                        isResponible = Convert.ToBoolean(reader["is_replying"]);

                    distr.IsResponsible = isResponible;
                    distr.IsReplying = isReplying;

                    DateTime? deadline = null;
                    if (reader["deadline"] != DBNull.Value)
                        deadline = Convert.ToDateTime(reader["deadline"]);

                    distr.DeadLine = deadline;

                    distr.Resolution = reader["resolution"].ToString();

                    distr.Log = reader["log"].ToString();

                    DateTime? date_add = null;
                    if (reader["date_add"] != DBNull.Value)
                        date_add = Convert.ToDateTime(reader["date_add"]);
                    distr.DateAdd = date_add;

                    result.Add(distr);
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
