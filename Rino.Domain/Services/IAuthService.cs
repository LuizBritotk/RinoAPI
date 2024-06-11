using Rino.Domain.Entities;

namespace Rino.Domain.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginCommand loginCommand);
    }
}
