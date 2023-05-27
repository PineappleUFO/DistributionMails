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
    public async Task<User?> TryGetUserByLogin(string login, string password,IPositionRepository? positionRepository,IDepRepository? depRepository,CancellationToken cancellationToken = default)
    {
        if (depRepository == null) return null;
        if (positionRepository == null) return null;
        if (!new PostgresGenerateConnection().TryCreateConnection(login, password)) return null;

        await Task.Delay(2000, cancellationToken);
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
                int userId = reader.GetInt32(0);
                return new User(userId,
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    null,
                    reader.GetString(6),
                    await depRepository.GetDepByUserId(userId),
                    await positionRepository.GetPositionByUserId(userId));
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