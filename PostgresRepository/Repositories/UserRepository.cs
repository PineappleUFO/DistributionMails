using Core.Models;
using EF.Interfaces;
using Npgsql;
using PostgresRepository.Interfaces;
using PostgresRepository.PostgresCommon;

namespace EF.Repositories;

public class UserRepository : IUserRepository
{
    /// <summary>
    /// Попытка получить пользователя при его авторизации
    /// </summary>
    public async Task<User?> TryGetUserByLogin(string login, string password,IConnectionString connectionString, CancellationToken cancellationToken = default)
    {
     

        await Task.Delay(200, cancellationToken);
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
                return new User(reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    null,
                    reader.GetString(6),
                    new Dep(reader.GetInt32(7),reader.GetString(8),reader.GetString(9)),
                    new Position(reader.GetInt32(10),reader.GetString(11)));
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
        
        //если по какой то причине строка подключения пустая
        if (string.IsNullOrWhiteSpace(PostgresConnectionString.ConnectionString))
            throw new Exception("Не задана строка подключения");

        await using var connection = new NpgsqlConnection(PostgresConnectionString.ConnectionString);
        await connection.OpenAsync();

        List<User> result = new();
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
       p.position_name,
 (SELECT CONCAT(LEFT(u.name, 1), '.', LEFT(u.surname, 1),'.')) AS inicials
from users u,deps d,positions p
where u.id_dep = d.dep_id and
      p.position_id = u.id_position";

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var user =  new User(reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    null,
                    reader.GetString(6),
                    new Dep(reader.GetInt32(7),reader.GetString(8),reader.GetString(9)),
                    new Position(reader.GetInt32(10),reader.GetString(11)));

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
}