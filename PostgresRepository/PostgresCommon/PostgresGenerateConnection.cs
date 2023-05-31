using EF.Interfaces;
using Npgsql;
using System.Data;

namespace PostgresRepository.PostgresCommon;

public class PostgresGenerateConnection : IConnectionString
{

    private string? _connectionStringCache;
    public string? GenerateConenctionStringByLogin(string username, string password)
    {
        //если уже есть строка подключения - то не за чем заного ее проверять
        if (!string.IsNullOrWhiteSpace(_connectionStringCache)) return _connectionStringCache;

        string connectionString = $"Host=localhost;Database=MailerAdmin;Username='{username}';Password='{username}'";
        NpgsqlConnection connection = new NpgsqlConnection(connectionString);
        try
        {
            connection.Open();
            if (connection.State == ConnectionState.Open)
            {
                _connectionStringCache = connectionString;
                return _connectionStringCache;
            }
        }
        catch (NpgsqlException e)
        {
            //todo: log
            return null;
        }
        finally
        {
            connection.Close();
        }

        return null;
    }

    public NpgsqlConnection TryGetConnetion()
    {
        if (_connectionStringCache != null)
        {
            return new NpgsqlConnection(_connectionStringCache);
        }
        throw new NullReferenceException($"{nameof(_connectionStringCache)} is Null");
    }
}