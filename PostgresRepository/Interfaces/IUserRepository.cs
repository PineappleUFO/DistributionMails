using Core.Models;
using EF.Interfaces;
using EF.Repositories;

namespace PostgresRepository.Interfaces;

public interface IUserRepository
{
    Task<User?> TryGetUserByLogin(string login, string password, IConnectionString connectionString,CancellationToken cancellationToken);
    Task<List<User?>> GetAllUsers();
}