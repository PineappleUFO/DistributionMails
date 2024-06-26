﻿using Core.Models;
using EF.Interfaces;
using Npgsql;
using PostgresRepository.Interfaces;
using PostgresRepository.PostgresCommon;

namespace EF.Repositories;

public class PositionRepository:IPositionRepository
{
    IConnectionString connectionString;
    public PositionRepository(IConnectionString connectionString)
    {
        this.connectionString = connectionString;
    }
    /// <summary>
    /// Получить должность пользователя
    /// </summary>
    /// <param name="userId">id пользователя</param>
    public async Task<Position?> GetPositionByUserId(int userId)
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
                $@"select p.position_id, position_name from users u, positions p where u.id_position = p.position_id and u.user_id={userId}";
          
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                //todo: dep и position
                return new Position(reader.GetInt32(0), reader.GetString(1));
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