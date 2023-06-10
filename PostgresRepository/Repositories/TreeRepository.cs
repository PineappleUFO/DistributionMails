using Core.Models;
using EF.Interfaces;
using Npgsql;

namespace EF.Repositories
{
    public class TreeRepository : ITreeRepository
    {
        IConnectionString connectionString;
        public TreeRepository(IConnectionString connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddDistributionInMail(Mail mail, int treeId, User toUser, DateTime deadline, string resolution, bool isResponible, bool isReplying)
        {
            //если по какой то причине строка подключения пустая
            if (connectionString == null)
                throw new Exception("Не задана строка подключения");
            using var connection = connectionString.TryGetConnetion();
            connection.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;

                cmd.CommandText = "INSERT INTO distribution_tree (id_mail,up_id, id_user, deadline, resolution, is_responsible, is_replying, log) VALUES (@id_mail,@up_id, @id_user, @deadline, @resolution, @is_responsible, @is_replying, @log) ON CONFLICT DO NOTHING";
                cmd.Parameters.AddWithValue("id_mail", mail.Id);
                cmd.Parameters.AddWithValue("id_user", toUser.Id);
                cmd.Parameters.AddWithValue("up_id", treeId);
                cmd.Parameters.AddWithValue("deadline", deadline);
                cmd.Parameters.AddWithValue("resolution", resolution??"");
                cmd.Parameters.AddWithValue("is_responsible", isResponible);
                cmd.Parameters.AddWithValue("is_replying", isReplying);
                cmd.Parameters.AddWithValue("log", "Добавление распределения");

                cmd.ExecuteNonQuery();
            }
        }

        public void AddOneLevelDistributionInMail(Mail mail, User user, DateTime deadline, string resolution, bool isResponible, bool isReplying)
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

        public void ChangeDeadline(int treeId, DateTime deadline)
        {
            //если по какой то причине строка подключения пустая
            if (connectionString == null)
                throw new Exception("Не задана строка подключения");
            using var connection = connectionString.TryGetConnetion();
            connection.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = $"UPDATE distribution_tree SET deadline = @deadline WHERE id = @treeId;";
                cmd.Parameters.AddWithValue("@deadline", deadline);
                cmd.Parameters.AddWithValue("@treeId", treeId);
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteUserFromTree(int treeId)
        {
            //если по какой то причине строка подключения пустая
            if (connectionString == null)
                throw new Exception("Не задана строка подключения");
            using var connection = connectionString.TryGetConnetion();
            connection.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = $"DELETE from distribution_tree where id = {treeId};";
                cmd.ExecuteNonQuery();
            }
        }

        public async Task<List<TreeItem>> GetTreeByMailId(Mail mail)
        {


            //если по какой то причине строка подключения пустая
            if (connectionString == null)
                throw new Exception("Не задана строка подключения");

            var result = new List<TreeItem>();
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
       t.up_id,
       t.deadline,
       t.resolution,
       t.is_responsible,
       t.is_replying,
       t.date_add,
       t.log,
       s.status_id,
       s.status_name,
       s.status_color
from distribution_tree t
inner join users u on u.user_id = t.id_user
left join status s on t.id_status = s.status_id
where t.id_mail = {mail.Id}";

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var distr = new TreeItem();
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
                    if (reader["up_id"] != DBNull.Value)
                        upId = Convert.ToInt32(reader["up_id"]);

                    distr.UpId = upId;

                    bool isResponible = false;
                    if (reader["is_responsible"] != DBNull.Value)
                        isResponible = Convert.ToBoolean(reader["is_responsible"]);

                    bool isReplying = false;
                    if (reader["is_replying"] != DBNull.Value)
                        isReplying = Convert.ToBoolean(reader["is_replying"]);

                    distr.IsResponsible = isResponible;
                    distr.IsReplying = isReplying;

                    DateTime? deadline = null;
                    if (reader["deadline"] != DBNull.Value)
                        deadline = Convert.ToDateTime(reader["deadline"]);

                    distr.Deadline = deadline;

                    distr.Resolution = reader["resolution"].ToString();

                    distr.Log = reader["log"].ToString();

                    DateTime? date_add = null;
                    if (reader["date_add"] != DBNull.Value)
                        date_add = Convert.ToDateTime(reader["date_add"]);
                    distr.DateAdd = date_add;

                    var status = new Status();
                    if (reader["status_id"] != DBNull.Value)
                    {
                        status.Id = Convert.ToInt32(reader["status_id"]);
                        status.Name = reader["status_name"].ToString();
                        status.Color = $"#{reader["status_color"]}";
                    }
                    distr.Status = status;

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

        public void SetAccept(int treeId)
        {
            //если по какой то причине строка подключения пустая
            if (connectionString == null)
                throw new Exception("Не задана строка подключения");
            using var connection = connectionString.TryGetConnetion();
            connection.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = $"UPDATE distribution_tree SET id_status = 1 WHERE id = @treeId;";
                cmd.Parameters.AddWithValue("@treeId", treeId);
                cmd.ExecuteNonQuery();
            }
        }

        public void SetDone(int treeId)
        {
            //если по какой то причине строка подключения пустая
            if (connectionString == null)
                throw new Exception("Не задана строка подключения");
            using var connection = connectionString.TryGetConnetion();
            connection.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = $"UPDATE distribution_tree SET id_status = 2 WHERE id = @treeId;";
                cmd.Parameters.AddWithValue("@treeId", treeId);
                cmd.ExecuteNonQuery();
            }
        }

        public void SetReplyingInTree(int treeId)
        {
            //если по какой то причине строка подключения пустая
            if (connectionString == null)
                throw new Exception("Не задана строка подключения");
            using var connection = connectionString.TryGetConnetion();
            connection.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = $"UPDATE distribution_tree SET is_replying = NOT is_replying, is_responsible = false WHERE id = {treeId};";
                cmd.ExecuteNonQuery();
            }
        }

        public void SetResponibleInTree(int treeId)
        {
            //если по какой то причине строка подключения пустая
            if (connectionString == null)
                throw new Exception("Не задана строка подключения");
            using var connection = connectionString.TryGetConnetion();
            connection.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = $"UPDATE distribution_tree SET is_responsible = NOT is_responsible,is_replying = false WHERE id = {treeId};";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
