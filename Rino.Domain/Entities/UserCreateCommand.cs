using System;
using System.Globalization;
using System.Text;

namespace Rino.Domain.Entities
{
    public class UserCreateCommand
    {
        public Guid Id { get; private set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }

        public UserCreateCommand() => Id = Guid.NewGuid();

        public void GenerateLogin(string firstName, string lastName)
        {
            // Combina o primeiro nome e o último nome para formar o login
            string baseLogin = $"{firstName}.{lastName}".ToLower();

            // Remove caracteres especiais e espaços em branco
            baseLogin = RemoveSpecialCharacters(baseLogin);

            // Verifica se o login está vazio após a limpeza
            if (string.IsNullOrWhiteSpace(baseLogin)) 
                throw new ArgumentException("O nome e sobrenome resultam em um login vazio.");

            // Adiciona um sufixo numérico para garantir unicidade
            Login = GenerateUniqueLogin(baseLogin);
        }

        private string RemoveSpecialCharacters(string input)
        {
            // Remove caracteres especiais e espaços em branco
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char value in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(value);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(value);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private string GenerateUniqueLogin(string baseLogin)
        {
            // Verifica se já existe algum usuário com o mesmo login

            // Se não existir nenhum usuário com o mesmo login, retorna o login sem alterações
            if (CheckLoginAvailability(baseLogin))
                return baseLogin;

            // Adiciona sufixos numéricos até encontrar um login disponível
            int suffix = 1;
            string uniqueLogin = baseLogin;

            while (!CheckLoginAvailability(uniqueLogin))
            {
                uniqueLogin = $"{baseLogin}{suffix}";
                suffix++;
            }

            return uniqueLogin;
        }

        private bool CheckLoginAvailability(string login)
        {
            // Exemplo simples: verifica se o login já existe em uma lista mockada
            List<string> existingLogins = new List<string> { "joao.silva", "maria.souza", "ana.pereira" };

            return !existingLogins.Contains(login);
        }
    }
}
