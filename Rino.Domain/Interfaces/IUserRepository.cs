using Rino.Domain.Entities;

namespace Rino.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> FindByUsernameAndPassword(LoginCommand loginCommand);
        Task<User> GetUserByUsername(string username);
        Task CreateUser(User user);
        Task UpdateUser(User user);

    }
}
