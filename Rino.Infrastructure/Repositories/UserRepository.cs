using Rino.Infrastructure.Utilities;
using System;
using System.Threading.Tasks;
using Rino.Domain.Entities;
using Rino.Domain.Repositories;
using Rino.Infrastructure.Data;

namespace Rino.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        // Simulação do contexto do banco de dados
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> FindByUsernameAndPassword(LoginCommand loginCommand)
        {
            // Dados mockados para simular uma busca em um banco de dados
            var mockUser = new User
            {
                Id = UuidGenerator.GenerateUuid(), // Gerando UUID
                Email = "luizABrito@Hotmail.com",
                Login = "luiz.brito",
                PasswordHash = "6faf449387ac41c60ca335f2e4481e6d771a6741bfac8045c8cc0b1eeecac59d", //LuizAcessoAPI
                TokenJWT = null
            };

            // Simulando a busca por login e senha
            if (loginCommand.Login == mockUser.Login && loginCommand.Password == mockUser.PasswordHash)
                return await Task.FromResult(mockUser);
            else
                return await Task.FromResult<User>(null);
        }
    }
}
