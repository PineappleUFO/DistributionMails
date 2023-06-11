using Core.Models;
using EF.Interfaces;
using Npgsql;
using PostgresRepository.Interfaces;
using PostgresRepository.PostgresCommon;

namespace EF.Repositories;

public class MailRepository : IMailRepository
{
    IConnectionString connectionString;
    public MailRepository(IConnectionString connectionString)
    {
        this.connectionString = connectionString;
    }

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
        om.text,--17
        m.reply_required,--18
(select count(*) from distribution_tree d where
                                      d.id_mail = m.mail_id
                                    and d.id_user = m.responsible
                                    and d.is_responsible = true) as mailIsDone,
(SELECT CONCAT(LEFT(u.name, 1), '.', LEFT(u.surname, 1),'.')) AS inicials
            from incoming_mail m
        left join users u on u.user_id = m.responsible
        inner join projects p on m.id_project=p.project_id
        inner join sender s on m.id_sender = s.sender_id
        left join outgoing_mail om on m.id_outgoing_mail = om.mail_id
        inner join mail_type mt on m.id_type = mt.mail_type_id
        where mt.mail_type_id=3
        order by m.date_input desc";

        return await loadMailByQuery(query);
    }

    public async Task<List<Mail>> GetArchiveUser(User user)
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
        om.text,--17
   m.reply_required,--18
(select count(*) from distribution_tree d where
                                      d.id_mail = m.mail_id
                                    and d.id_user = m.responsible
                                    and d.is_responsible = true) as mailIsDone,
(SELECT CONCAT(LEFT(u.name, 1), '.', LEFT(u.surname, 1),'.')) AS inicials
            from incoming_mail m
        left join users u on u.user_id = m.responsible
        inner join projects p on m.id_project=p.project_id
        inner join sender s on m.id_sender = s.sender_id
        inner join users_archive_mails du on m.mail_id=du.id_mail
        left join outgoing_mail om on m.id_outgoing_mail = om.mail_id
        where du.id_user={user.Id}
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
        om.text,--17
   m.reply_required,--18
(select count(*) from distribution_tree d where
                                      d.id_mail = m.mail_id
                                    and d.id_user = m.responsible
                                    and d.is_responsible = true) as mailIsDone,
(SELECT CONCAT(LEFT(u.name, 1), '.', LEFT(u.surname, 1),'.')) AS inicials
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

    public async Task<List<string>> GetFastResolution()
    {
        //если по какой то причине строка подключения пустая
        if (connectionString == null)
            throw new Exception("Не задана строка подключения");

        var result = new List<string>();
        await using var connection = connectionString.TryGetConnetion();
        await connection.OpenAsync();

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = "select resolution_text from fast_resolution";

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(reader[0].ToString());
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

    public async Task<List<Mail>> GetFavoriteUser(User user)
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
        om.text,--17
        m.reply_required,--18
(select count(*) from distribution_tree d where
                                      d.id_mail = m.mail_id
                                    and d.id_user = m.responsible
                                    and d.is_responsible = true) as mailIsDone,
(SELECT CONCAT(LEFT(u.name, 1), '.', LEFT(u.surname, 1),'.')) AS inicials
            from incoming_mail m
        left join users u on u.user_id = m.responsible
        inner join projects p on m.id_project=p.project_id
        inner join sender s on m.id_sender = s.sender_id
        inner join users_favorite_mail du on m.mail_id=du.id_mail
        left join outgoing_mail om on m.id_outgoing_mail = om.mail_id
        where du.id_user={user.Id}
        order by m.date_input desc";

        return await loadMailByQuery(query);
    }

    public async Task<List<Mail>> GetMailsByType(MailType type) 
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
        om.theme,--16
        om.text,--17
        m.reply_required,--18
(select count(*) from distribution_tree d where
                                      d.id_mail = m.mail_id
                                    and d.id_user = m.responsible
                                    and d.is_responsible = true) as mailIsDone,
        (SELECT CONCAT(LEFT(u.name, 1), '.', LEFT(u.surname, 1),'.')) AS inicials
            from incoming_mail m
        left join users u on u.user_id = m.responsible
        inner join projects p on m.id_project=p.project_id
        inner join sender s on m.id_sender = s.sender_id
        left join outgoing_mail om on m.id_outgoing_mail = om.mail_id
        where m.id_type = {type.Id}
        order by m.date_input desc";

        return await loadMailByQuery(query);
    }

    public int GetMailsInWork(int userId)
    {
        //если по какой то причине строка подключения пустая
        if (connectionString == null)
            throw new Exception("Не задана строка подключения");

        var result = 0;
        using var connection = connectionString.TryGetConnetion();
        connection.Open();



        try
        {
            using var command = connection.CreateCommand();
            command.CommandText = $@"SELECT COUNT(*) AS num_of_emails
FROM distribution_tree dt
JOIN incoming_mail im ON dt.id_mail = im.mail_id
WHERE dt.id_user = {userId}
AND im.id_outgoing_mail IS NULL";
        
            result = Convert.ToInt32(command.ExecuteScalar());

        }
        catch (Exception ex)
        {

        }
        return result;
    }

    public void TransferToArchive(Mail mail, User user)
    {
        //если по какой то причине строка подключения пустая
        if (connectionString == null)
            throw new Exception("Не задана строка подключения");
        using var connection = connectionString.TryGetConnetion();
        connection.Open();
        
        using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = connection;
            cmd.CommandText = $"SELECT count(*) from distributed_to_user where id_mail = {mail.Id} and id_user={user.Id}";
            if(Convert.ToInt32(cmd.ExecuteScalar())==0)
            {
                return;
            }
        }

        using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = connection;
            cmd.CommandText = $"DELETE from distributed_to_user where id_mail = {mail.Id} and id_user={user.Id}";
            cmd.ExecuteNonQuery();
        }

        using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = connection;
            cmd.CommandText = "INSERT INTO users_archive_mails (id_mail, id_user) VALUES (@id_mail, @id_user) ON CONFLICT DO NOTHING";
            cmd.Parameters.AddWithValue("id_mail", mail.Id);
            cmd.Parameters.AddWithValue("id_user", user.Id);
            cmd.ExecuteNonQuery();
        }
        using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = connection;
            cmd.CommandText = $"UPDATE distribution_tree SET id_status = 4 WHERE id_mail = @idMail and id_user = @idUser and id_status IS NULL";
            cmd.Parameters.AddWithValue("@idMail", mail.Id);
            cmd.Parameters.AddWithValue("@idUser", user.Id);
            cmd.ExecuteNonQuery();
        }
       
    }


    /// <summary>
    /// Функция получения писем
    /// </summary>
    private async Task<List<Mail>> loadMailByQuery(string query)
    {
        //если по какой то причине строка подключения пустая
        if (connectionString == null)
            throw new Exception("Не задана строка подключения");

        var result = new List<Mail>();
        await using var connection = connectionString.TryGetConnetion();
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
                    responsible.Inicials = reader["inicials"].ToString();
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

                //генерируем предпологаемый путь до файлов  (в продакшнене тут ссылка на файловый сервер, в данном случае сделана динамическая ссылка на папку проекта и в ней папка с pdf симулирующая файловый сервер)
                var CurentPath = new DirectoryInfo(Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
                    ?.Parent
                    ?.Parent
                    ?.Parent
                    ?.Parent
                    ?.Parent
                    ?.Parent
                    .FullName, ".FileServer","Input", reader.GetInt32(0).ToString()));

                //елсли путь существует то добавляем его к письму
                if(CurentPath.Exists)
                {
                    mail.PathFolder = CurentPath;
                }

                if (reader.GetInt32(19)>0)
                {
                    mail.IsMailDone =true;
                }else
                {
                    mail.IsMailDone = false;
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