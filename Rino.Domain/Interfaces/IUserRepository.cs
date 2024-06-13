using Rino.Domain.Entities;

namespace Rino.Domain.Repositories
{
    public interface IUserRepository
    {

        // Método para buscar usuário por nome de usuário e senha
        Task<User> FindByUsernameAndPassword(LoginCommand loginCommand);
        // Método para obter usuário por nome de usuário
        Task<User> GetUserByUsername(string username);

        // Método para obter usuário por email
        Task<User> GetUserByEmail(string email);

        // Método para criar um novo usuário
        Task CreateUser(User user);

        // Método para atualizar informações do usuário
        Task UpdateUser(User user);

    }
}
