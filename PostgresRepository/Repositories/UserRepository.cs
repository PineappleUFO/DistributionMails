using Core.Models;
using Npgsql;
using PostgresRepository.Interfaces;
using PostgresRepository.PostgresCommon;

namespace EF.Repositories;

public class UserRepository : IUserRepository
{
    /// <summary>
    /// Попытка получить пользователя при его авторизации
    /// </summary>
    public async Task<User?> TryGetUserByLogin(string login, string password, CancellationToken cancellationToken = default)
    {
        if (!new PostgresGenerateConnection().TryCreateConnection(login, password)) return null;

    
        //если по какой то причине строка подключения пустая
        if (string.IsNullOrWhiteSpace(PostgresConnectionString.ConnectionString))
            throw new Exception("Не задана строка подключения");

        await using var connection = new NpgsqlConnection(PostgresConnectionString.ConnectionString);
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
}