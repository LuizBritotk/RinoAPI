using System;
using System.Collections.Generic;
using Rino.Domain.Interfaces;
using Rino.Infrastructure.Utilities;

namespace Rino.Domain.Entities
{
    public static class MockUserData
    {
        public static List<User> Users { get; private set; }

        static MockUserData()
        {
            var passwordHasher = new PasswordHasher();

            Users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "luizbritoti@hotmail.com",
                    FirstName = "Luiz",
                    LastName = "Americo Brito",
                    PhoneNumber = "11934907432",
                    Login = "luiz.brito",
                    PasswordHash = passwordHasher.MaskPassword("LuizAcessoAPI"),
                    DateOfBirth = new DateTime(1990, 5, 15)
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "luizbritoti@hotmail.com",
                    FirstName = "Admin",
                    LastName = "Admin",
                    PhoneNumber = "11987654322",
                    Login = "admin.admin",
                    PasswordHash = passwordHasher.MaskPassword("AdminAcessoAPI"),
                    DateOfBirth = new DateTime(1985, 7, 20)
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "luizbritoti@hotmail.com",
                    FirstName = "Pedro",
                    LastName = "Oliveira",
                    PhoneNumber = "11987654323",
                    Login = "pedro.oliveira",
                    PasswordHash = passwordHasher.MaskPassword("PedroAcessoAPI"),
                    DateOfBirth = new DateTime(1988, 9, 10)
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "luizbritoti@hotmail.com",
                    FirstName = "Ana",
                    LastName = "Santos",
                    PhoneNumber = "11987654324",
                    Login = "ana.santos",
                    PasswordHash = passwordHasher.MaskPassword("AnaAcessoAPI"),
                    DateOfBirth = new DateTime(1992, 3, 5)
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "luizbritoti@hotmail.com",
                    FirstName = "Carlos",
                    LastName = "Mendes",
                    PhoneNumber = "11987654325",
                    Login = "carlos.mendes",
                    PasswordHash = passwordHasher.MaskPassword("CarlosAcessoAPI"),
                    DateOfBirth = new DateTime(1983, 12, 8)
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "luizbritoti@hotmail.com",
                    FirstName = "Juliana",
                    LastName = "Lima",
                    PhoneNumber = "11987654326",
                    Login = "juliana.lima",
                    PasswordHash = passwordHasher.MaskPassword("JulianaAcessoAPI"),
                    DateOfBirth = new DateTime(1987, 10, 30)
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "luizbritoti@hotmail.com",
                    FirstName = "Fernando",
                    LastName = "Almeida",
                    PhoneNumber = "11987654327",
                    Login = "fernando.almeida",
                    PasswordHash = passwordHasher.MaskPassword("FernandoAcessoAPI"),
                    DateOfBirth = new DateTime(1995, 6, 18)
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "luizbritoti@hotmail.com",
                    FirstName = "Aline",
                    LastName = "Costa",
                    PhoneNumber = "11987654328",
                    Login = "aline.costa",
                    PasswordHash = passwordHasher.MaskPassword("AlineAcessoAPI"),
                    DateOfBirth = new DateTime(1986, 4, 12)
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "luizbritoti@hotmail.com",
                    FirstName = "Roberto",
                    LastName = "Santos",
                    PhoneNumber = "11987654329",
                    Login = "roberto.santos",
                    PasswordHash = passwordHasher.MaskPassword("RobertoAcessoAPI"),
                    DateOfBirth = new DateTime(1984, 11, 25)
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = "luizbritoti@hotmail.com",
                    FirstName = "Patrícia",
                    LastName = "Oliveira",
                    PhoneNumber = "11987654330",
                    Login = "patricia.oliveira",
                    PasswordHash = passwordHasher.MaskPassword("PatriciaAcessoAPI"),
                    DateOfBirth = new DateTime(1991, 8, 3)
                }
            };
        }
    }
}
