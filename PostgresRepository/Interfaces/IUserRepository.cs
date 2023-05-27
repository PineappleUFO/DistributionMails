using Core.Models;

namespace EF.Interfaces;

public interface IUserRepository
{
    User TryGetUserByLogin(string login, string password);
}