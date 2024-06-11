using Rino.Domain.Entities;

namespace Rino.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> FindByUsernameAndPassword(LoginCommand loginCommand);
    }
}
