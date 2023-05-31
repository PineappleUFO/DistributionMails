using Core.Models;

namespace PostgresRepository.Interfaces;

public interface IMailRepository
{
    /// <summary>
    /// Получить все письма
    /// </summary>
    Task<List<Mail>> GetAllMails();

    /// <summary>
    /// Получить нераспределенные письма пользователя
    /// </summary>
    /// <param name="user">Пользователь чьи письма загружаем</param>
    Task<List<Mail>> GetDistributedToUser(User user);
}