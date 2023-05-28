using Core.Models;

namespace PostgresRepository.Interfaces;

public interface IMailRepository
{
    Task<List<Mail>> GetAllMails();
}