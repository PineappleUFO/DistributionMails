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

    /// <summary>
    /// Получить архив писем пользователя
    /// </summary>
    /// <param name="user">Пользователь чьи письма загружаем</param>
    Task<List<Mail>> GetArchiveUser(User user);

    /// <summary>
    /// Получить избранные письма пользователя
    /// </summary>
    /// <param name="user">Пользователь чьи письма загружаем</param>
    Task<List<Mail>> GetFavoriteUser(User user);

    /// <summary>
    /// Получить письма в зависимости от типа
    /// </summary>
    Task<List<Mail>> GetMailsByType(MailType type);

    /// <summary>
    /// Сколько писем у пользователя в работе
    /// </summary>
    int GetMailsInWork(int userId);

    /// <summary>
    /// Загрузить быстрые резолюции
    /// </summary>
    /// <returns></returns>
    Task<List<string>> GetFastResolution();

    /// <summary>
    /// Переместить пиьмо в архив
    /// </summary>
    void TransferToArchive(Mail mail, User user);
}