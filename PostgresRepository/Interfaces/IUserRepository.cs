using Core.Models;
using EF.Repositories;

namespace PostgresRepository.Interfaces;

public interface IUserRepository
{
    Task<User?> TryGetUserByLogin(string login, string password,CancellationToken cancellationToken);
}