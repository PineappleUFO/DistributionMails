using Core.Models;

namespace EF.Interfaces;

public interface IUserRepository
{
    Task<User?> TryGetUserByLogin(string login, string password,CancellationToken cancellationToken);
}