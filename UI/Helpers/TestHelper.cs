using EF.Interfaces;
using PostgresRepository.PostgresCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Helpers
{
    //Удалить
    public static class TestHelper
    {
        private static IConnectionString connectionString;

        public static IConnectionString GetConnectionSingltone()
        {
            if (connectionString == null)
            {
                connectionString = new PostgresGenerateConnection();
            }

             return connectionString;

        }
    }
}
