using Core.Models;
using EF.Interfaces;
using Npgsql;
using PostgresRepository.Interfaces;
using PostgresRepository.PostgresCommon;



public class DepRepository : IDepRepository
{
    IConnectionString connectionString;
    public DepRepository(IConnectionString connectionString)
    {
        this.connectionString = connectionString;
    }
    /// <summary>
    /// Получить отдел пользователя
    /// </summary>
    /// <param name="userId">id пользователя</param>
    /// <returns></returns>
    public async Task<Dep?> GetDepByUserId(int userId)
    {
        //если по какой то причине строка подключения пустая
        if (connectionString == null)
            throw new Exception("Не задана строка подключения");

        await using var connection = connectionString.TryGetConnetion();
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