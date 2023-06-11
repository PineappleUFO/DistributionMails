using Core.Models;
using EF.Interfaces;
using Npgsql;
using PostgresRepository.Interfaces;
using System.Text;

namespace EF.Repositories;

public class UserRepository : IUserRepository
{
    IConnectionString connectionString;
    public UserRepository(IConnectionString connectionString)
    {
        this.connectionString = connectionString;
    }

    /// <summary>
    /// Попытка получить пользователя при его авторизации
    /// </summary>
    public async Task<User?> TryGetUserByLogin(string login, string password, CancellationToken cancellationToken = default)
    {

        //если по какой то причине строка подключения пустая
        if (string.IsNullOrWhiteSpace(connectionString.GenerateConenctionStringByLogin(login, password)))
            throw new Exception("Не задана строка подключения");

        await using var connection = connectionString.TryGetConnetion();
        await connection.OpenAsync(cancellationToken);

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText =
                $@"select u.user_id,
       u.family,
       u.name,
       u.surname,
       u.login,
       u.photo,
       u.phone,
       d.dep_id,
       d.dep_name,
       d.dep_about,
       p.position_id,
       p.position_name
from users u,deps d,positions p
where u.login = '{login}' and
      u.id_dep = d.dep_id and
      p.position_id = u.id_position";

            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if (await reader.ReadAsync(cancellationToken))
            {
                byte[]? photo = null;
                try
                {
                    photo = (byte[])reader["photo"];
                }
                catch (Exception)
                {

                }
                return new User(reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    photo,
                    reader.GetString(6),
                    new Dep(reader.GetInt32(7), reader.GetString(8), reader.GetString(9)),
                    new Position(reader.GetInt32(10), reader.GetString(11)));
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
        return null;
    }

    public async Task<List<User?>> GetAllUsers()
    {
        string querry = $@"select u.user_id,
       u.family,
       u.name,
       u.surname,
       u.login,
       u.photo,
       u.phone,
       d.dep_id,
       d.dep_name,
       d.dep_about,
       p.position_id,
       p.position_name,
 (SELECT CONCAT(LEFT(u.name, 1), '.', LEFT(u.surname, 1),'.')) AS inicials
from users u,deps d,positions p
where u.id_dep = d.dep_id and
      p.position_id = u.id_position
order by u.family";


        return await GetUsersAsync(querry);
    }

    public async Task<List<User?>> GetUserByCount(int userId)
    {
        string querry = $@"select u.user_id,
       u.family,
       u.name,
       u.surname,
       u.login,
       u.photo,
       u.phone,
       d.dep_id,
       d.dep_name,
       d.dep_about,
       p.position_id,
       p.position_name,
 (SELECT CONCAT(LEFT(u.name, 1), '.', LEFT(u.surname, 1),'.')) AS inicials
from users u,deps d,positions p,distribution_counter dc

where
    dc.distributed_user_id = u.user_id and
    u.id_dep = d.dep_id and
      p.position_id = u.id_position
and dc.id_user = {userId}
order by dc.count desc";
        return await GetUsersAsync(querry);
    }

    public async Task<List<User?>> GetUserFromDep(int depId)
    {
        string querry = $@"select u.user_id,
       u.family,
       u.name,
       u.surname,
       u.login,
       u.photo,
       u.phone,
       d.dep_id,
       d.dep_name,
       d.dep_about,
       p.position_id,
       p.position_name,
 (SELECT CONCAT(LEFT(u.name, 1), '.', LEFT(u.surname, 1),'.')) AS inicials
from users u,deps d,positions p
where
    u.id_dep = d.dep_id and
      p.position_id = u.id_position
and d.dep_id = {depId}
order by u.family";

        return await GetUsersAsync(querry);
    }

    public async Task<List<User?>> GetUsersFromReplacement(int userId)
    {
        string querry = $@"select u.user_id,
       u.family,
       u.name,
       u.surname,
       u.login,
       u.photo,
       u.phone,
       d.dep_id,
       d.dep_name,
       d.dep_about,
       p.position_id,
       p.position_name,
 (SELECT CONCAT(LEFT(u.name, 1), '.', LEFT(u.surname, 1),'.')) AS inicials
from users u,deps d,positions p,users_replacement ur
where
    u.user_id = ur.who_user_id and
    u.id_dep = d.dep_id and
    p.position_id = u.id_position
and ur.whom_user_id = 157
order by u.family";

        return await GetUsersAsync(querry);
    }

    private async Task<List<User?>> GetUsersAsync(string querry)
    {
        //если по какой то причине строка подключения пустая
        if (connectionString == null)
            throw new Exception("Не задана строка подключения");

        await using var connection = connectionString.TryGetConnetion();
        await connection.OpenAsync();

        List<User> result = new();
        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText = querry;

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                byte[]? photo = null;
                try
                {
                   
                    photo = (byte[])reader["photo"];
                }
                catch (Exception)
                {

                }
                var user = new User(reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    photo,
                    reader.GetString(6),
                    new Dep(reader.GetInt32(7), reader.GetString(8), reader.GetString(9)),
                    new Position(reader.GetInt32(10), reader.GetString(11)));

                user.Inicials = reader["inicials"].ToString();
                result.Add(user);
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

        return result;
    }

    public void InsertImg()
    {
        //если по какой то причине строка подключения пустая
        if (connectionString == null)
            throw new Exception("Не задана строка подключения");
        using var connection = connectionString.TryGetConnetion();
        connection.Open();
        using (var cmd = new NpgsqlCommand())
        {
            string svgContent = File.ReadAllText("F:\\BrowserDownloads\\mvvi3rdo9iilirsp73p.png");
            string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(svgContent));

            cmd.Connection = connection;
            cmd.CommandText = $"UPDATE users SET photo = (decode('{base64String}', 'base64'))  WHERE user_id = 157;";
            cmd.ExecuteNonQuery();
        }
    }

  
}