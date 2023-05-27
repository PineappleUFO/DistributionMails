using Core.Models;
using EF.Interfaces;
using EF.PostgresCommon;
using Npgsql;

namespace EF.Repositories;

public class UserRepository : IUserRepository
{
    /// <summary>
    /// Попытка получить пользователя при его авторизации
    /// </summary>
    public async Task<User?> TryGetUserByLogin(string login, string password,CancellationToken cancellationToken = default)
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
                $@"select u.user_id, family, name, surname, login, photo, phone, id_dep, id_position from users u where u.login = '{login}'";

            await using var reader =await command.ExecuteReaderAsync(cancellationToken);
            if(await reader.ReadAsync(cancellationToken))
            {
                //todo: dep и position
                return new User(reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    null,
                    reader.GetString(6)
                );
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