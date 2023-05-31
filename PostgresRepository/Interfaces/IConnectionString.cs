using Npgsql;

namespace EF.Interfaces
{
    public interface IConnectionString
    {
        /// <summary>
        /// Сгенерировать строку подключения
        /// </summary>
        /// <param name="username">Логин бд</param>
        /// <param name="password">Пароль бд</param>
        string? GenerateConenctionStringByLogin(string username, string password);

        /// <summary>
        /// Попробовать получить объект подключения с кэшируемой стройкой подключения
        /// </summary>
        public NpgsqlConnection TryGetConnetion();
    }
}
