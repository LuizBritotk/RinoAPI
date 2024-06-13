using Rino.Domain.Entities;
using Rino.Domain.Repositories;
using Rino.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace Rino.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para encontrar usuário por nome de usuário e senha
        public async Task<User> FindByUsernameAndPassword(LoginCommand loginCommand)
        {
            try
            {
                // Dados mockados para simular uma busca em um banco de dados
                var mockUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = "luizABrito@Hotmail.com",
                    Name = "Luiz Brito",
                    Phone = "11934907432",
                    Login = "luiz.brito",
                    PasswordHash = "6faf449387ac41c60ca335f2e4481e6d771a6741bfac8045c8cc0b1eeecac59d", // Senha: LuizAcessoAPI
                    TokenJWT = null
                };

                // Simulando a busca por login e senha
                if (loginCommand.Login == mockUser.Login && loginCommand.Password == mockUser.PasswordHash)
                    return await Task.FromResult(mockUser);
                else
                    return await Task.FromResult<User>(null);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Erro ao buscar usuário por nome de usuário e senha: {ex.Message}");
                throw; 
            }
        }

        // Método para obter usuário por nome de usuário
        public async Task<User> GetUserByUsername(string username)
        {
            try
            {
                // Dados mockados para simular uma busca em um banco de dados
                var mockUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = "Luana.Telis@Gmail.com",
                    Login = username,
                    PasswordHash = "fd4ecfddc1ba5b83254fc4e6ef7086a803e4d4983da4a5644ed7f9291c549beb", // Senha: LuanaAcessoAPI
                    TokenJWT = null
                };

                // Simulando a busca por nome de usuário
                if (username == mockUser.Login)
                    return await Task.FromResult(mockUser);
                else
                    return await Task.FromResult<User>(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar usuário por nome de usuário: {ex.Message}");
                throw;
            }
        }

        // Método para obter usuário por email
        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                // Dados mockados para simular uma busca em um banco de dados
                var mockUser = new User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    Login = "luizABrito@Hotmail.com",
                    PasswordHash = "6faf449387ac41c60ca335f2e4481e6d771a6741bfac8045c8cc0b1eeecac59d",
                    TokenJWT = null
                };

                // Simulando a busca por email
                if (email == mockUser.Email)
                    return await Task.FromResult(mockUser);
                else
                    return await Task.FromResult<User>(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar usuário por email: {ex.Message}");
                throw; 
            }
        }

        // Método para criar um novo usuário
        public async Task CreateUser(User user)
        {
            try
            {
                // Simulando a criação de um novo usuário no banco de dados
                await Task.Delay(100); // Simulando uma operação assíncrona

                Console.WriteLine("Usuário criado com sucesso:");
                Console.WriteLine($"ID: {user.Id}, Username: {user.Login}, Email: {user.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar usuário: {ex.Message}");
                throw;
            }
        }

        // Método para atualizar informações do usuário
        public async Task UpdateUser(User user)
        {
            try
            {
                // Simulando a atualização de um usuário no banco de dados
                await Task.Delay(100); // Simulando uma operação assíncrona

                Console.WriteLine("Usuário atualizado com sucesso:");
                Console.WriteLine($"ID: {user.Id}, Username: {user.Login}, Email: {user.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar usuário: {ex.Message}");
                throw;
            }
        }
    }
}
