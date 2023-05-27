using Core.Models;

namespace PostgresRepository.Interfaces;

public interface IDepRepository
{
    Task<Dep?> GetDepByUserId(int userId);
}