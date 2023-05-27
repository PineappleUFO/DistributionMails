using Core.Models;

namespace PostgresRepository.Interfaces;

public interface IPositionRepository
{
    Task<Position?> GetPositionByUserId(int userId);
}