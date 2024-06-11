using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;

namespace Rino.Infrastructure.Utilities
{
    public class PasswordHasher
    {
    
        // Método para desmascarar a senha usando SHA-256
        public string UnmaskPassword(string maskedPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Computa o hash SHA-256 da senha mascarada
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(maskedPassword));

                // Converte os bytes hash em uma string hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Método para mascarar a senha usando SHA-256
        public string MaskPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Computa o hash SHA-256 da senha
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Converte os bytes hash em uma string hexadecimal
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Método para verificar se a senha corresponde à senha mascarada
        public bool VerifyPassword(string password, string maskedPassword)
        {
            // Compara a senha mascarada fornecida com a senha mascarada armazenada
            return password == maskedPassword;
        }

    }
}
