using Core.Models;
using EF.Interfaces;
using Npgsql;
using PostgresRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgresRepository.Repositories
{
    public class MailTypeRepository : IMailTypeRepository
    {
        IConnectionString connectionString;
        public MailTypeRepository(IConnectionString connectionString)
        {
           this.connectionString = connectionString;
        }
        public async Task<List<MailType>> GetTypesAccessByUser(User user)
        {
            //если по какой то причине строка подключения пустая
            if (connectionString == null)
                throw new Exception("Не задана строка подключения");

            List<MailType> result = new();
            await using var connection = connectionString.TryGetConnetion();
            await connection.OpenAsync();

            try
            {
                await using var command = connection.CreateCommand();
                command.CommandText =
                    $@"select mt.mail_type_id, mt.mail_type_name from mail_type mt
    inner join mail_type_access_user mtau on mt.mail_type_id = mtau.id_mail_type
    inner join users u on u.user_id = mtau.id_user
where u.user_id = {user.Id}";
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    MailType mailType = new();
                    mailType.Id = reader.GetInt32(0);
                    mailType.Name = reader.GetString(1);
                     result.Add(mailType);
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
            return result;
        }
    }
}
