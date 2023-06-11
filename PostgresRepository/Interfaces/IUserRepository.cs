using Core.Models;

namespace PostgresRepository.Interfaces;

public interface IUserRepository
{
    Task<User?> TryGetUserByLogin(string login, string password, CancellationToken cancellationToken);
    Task<List<User?>> GetAllUsers();
    Task<List<User?>> GetUserByCount(int userId);
    Task<List<User?>> GetUserFromDep(int depId);
    Task<List<User?>> GetUsersFromReplacement(int userId);
    
}