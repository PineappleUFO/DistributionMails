﻿using Core.Models;
using EF.Interfaces;
using Npgsql;
using PostgresRepository.PostgresCommon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Repositories
{
    public class TreeRepository : ITreeRepository
    {
        IConnectionString connectionString;
        public TreeRepository(IConnectionString connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<List<DistributionTreeElement>> GetTreeByMailId(int mailId)
        {


            //если по какой то причине строка подключения пустая
            if (connectionString == null)
                throw new Exception("Не задана строка подключения");

            var result = new List<DistributionTreeElement>();
            await using var connection = connectionString.TryGetConnetion();
            await connection.OpenAsync();

            try
            {
                await using var command = connection.CreateCommand();
                command.CommandText =
                    @"select t.id,
       t.id_mail,
       u.user_id,
       u.family,
       u.name,
       u.surname,
       (SELECT CONCAT(LEFT(u.name, 1), '.', LEFT(u.surname, 1),'.')) AS inicials,
       t.id_status,
       t.up_id,
       t.deadline,
       t.resolution,
       t.is_responsible,
       t.is_replying,
       t.date_add,
       t.log
from distribution_tree t
inner join users u on u.user_id = t.id_user
where t.id_mail = 2";

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var distr = new DistributionTreeElement();
                    var user = new User();
                    user.Id = Convert.ToInt32(reader["user_id"]);
                    user.Name = Convert.ToString(reader["name"]);
                    user.Family = Convert.ToString(reader["Family"]);
                    user.Surname = Convert.ToString(reader["Surname"]);
                    user.Inicials = Convert.ToString(reader["inicials"]);

                    distr.Id = Convert.ToInt32(reader["id"]);
                    distr.User = user;
                    distr.MailId = Convert.ToInt32(reader["id_mail"]);

                    int upId = default;
                    if(reader["up_id"] != DBNull.Value)
                        upId = Convert.ToInt32(reader["up_id"]);

                    distr.UpId = upId;

                    bool isResponible = false;
                    if (reader["is_responsible"] != DBNull.Value)
                        isResponible = Convert.ToBoolean(reader["is_responsible"]);

                    bool isReplying = false;
                    if (reader["is_replying"] != DBNull.Value)
                        isResponible = Convert.ToBoolean(reader["is_replying"]);

                    distr.IsResponsible = isResponible;
                    distr.IsReplying = isReplying;

                    DateTime? deadline = null;
                    if (reader["deadline"] != DBNull.Value)
                        deadline = Convert.ToDateTime(reader["deadline"]);

                    distr.DeadLine = deadline;

                    distr.Resolution = reader["resolution"].ToString();

                    distr.Log = reader["log"].ToString();

                    DateTime? date_add = null;
                    if (reader["date_add"] != DBNull.Value)
                        date_add = Convert.ToDateTime(reader["date_add"]);
                    distr.DateAdd = date_add;

                    result.Add(distr);
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
