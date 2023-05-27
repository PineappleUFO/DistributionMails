using System.Data;
using Npgsql;

namespace PostgresRepository.PostgresCommon;

internal class PostgresGenerateConnection
{
    /// <summary>
    /// Попытка получить строку подключения к бд в зависимости от логина и пароля которые ввел пользователь
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="pass">Пароль</param>
    /// <returns>true-логин и пароль корректный</returns>
    internal bool TryCreateConnection(string login,string pass)
    {
        string connectionString = $"Host=localhost;Database=MailerAdmin;Username='{login}';Password='{pass}'";
        NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        try
        {
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                PostgresConnectionString.ConnectionString = connectionString;
                return true;
            }
        }
        catch (NpgsqlException e)
        {
            return false;
        }
        finally
        {
            connection.Close();
        }
       
        return false;
    }
}