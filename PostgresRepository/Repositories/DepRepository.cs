using Core.Models;
using Npgsql;
using PostgresRepository.Interfaces;
using PostgresRepository.PostgresCommon;

namespace EF.Repositories;

public class DepRepository : IDepRepository
{
    /// <summary>
    /// Получить отдел пользователя
    /// </summary>
    /// <param name="userId">id пользователя</param>
    /// <returns></returns>
    public async Task<Dep?> GetDepByUserId(int userId)
    {
        //если по какой то причине строка подключения пустая
        if (string.IsNullOrWhiteSpace(PostgresConnectionString.ConnectionString))
            throw new Exception("Не задана строка подключения");

        await using var connection = new NpgsqlConnection(PostgresConnectionString.ConnectionString);
        await connection.OpenAsync();

        try
        {
            await using var command = connection.CreateCommand();
            command.CommandText =
                $@"select d.dep_id, dep_name, dep_about from deps d,users u where d.dep_id=u.id_dep and u.user_id='{userId}'";
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                //todo: dep и position
                return new Dep(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
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