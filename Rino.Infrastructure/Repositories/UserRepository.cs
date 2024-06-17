using Rino.Domain.Entities;
using Rino.Domain.Interfaces;
using Rino.Domain.Repositories;
using Rino.Infrastructure.Data;
using Rino.Infrastructure.Utilities;
using System;
using System.Threading.Tasks;

namespace Rino.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher();
        }

        // Método para encontrar usuário por nome de usuário e senha
        public async Task<User> FindByUsernameAndPassword(LoginCommand loginCommand)
        {
            try
            {
                // Busca o usuário na lista de usuários mockados
                var user = MockUserData.Users
                    .FirstOrDefault(u => u.Login == loginCommand.Login);

                // Verifica se o usuário foi encontrado e se a senha corresponde
                if (user != null && _passwordHasher.VerifyPassword(loginCommand.Password, user.PasswordHash))
                    return await Task.FromResult(user);
                else
                    return await Task.FromResult<User>(null!);
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
                // Busca o usuário na lista de usuários mockados
                var user = MockUserData.Users.FirstOrDefault(u => u.Login == username);

                return await Task.FromResult(user!);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar usuário por nome de usuário: {ex.Message}");
                throw;
            }
        }


        public async Task CreateUser(User user)
        {
            try
            {
                // Adiciona o usuário à lista mockada (simulando a criação no banco de dados)
                MockUserData.Users.Add(user);
                await Task.Delay(100);

                Console.WriteLine("Usuário criado com sucesso:");
                Console.WriteLine($"ID: {user.Id}, Username: {user.Login}, Email: {user.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar usuário: {ex.Message}");
                throw;
            }
        }


        // Simulando a atualização de um usuário no banco de dados
        // Simulando uma operação assíncrona
        public async Task UpdateUser(User user)
        {
            try
            {
                // Encontra o usuário na lista mockada e atualiza suas informações
                var existingUser = MockUserData.Users.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser != null)
                {
                    existingUser.Email = user.Email;
                    existingUser.FirstName = user.FirstName;
                    existingUser.LastName = user.LastName;
                    existingUser.PhoneNumber = user.PhoneNumber;
                    existingUser.DateOfBirth = user.DateOfBirth;
                    existingUser.PasswordHash = user.PasswordHash;

                    await Task.Delay(100);

                    Console.WriteLine("Usuário atualizado com sucesso:");
                    Console.WriteLine($"ID: {user.Id}, Username: {user.Login}, Email: {user.Email}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar usuário: {ex.Message}");
                throw;
            }
        }
    }
}
