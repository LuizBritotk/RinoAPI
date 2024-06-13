using System;
using System.Threading.Tasks;
using Rino.Domain.Entities;
using Rino.Domain.Interfaces;
using Rino.Domain.Repositories;
using Rino.Domain.Services;

namespace Rino.Domain.Handlers
{
    public class UserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IPasswordHasher _passwordHasher;

        public UserHandler(IUserRepository userRepository, IAuthService authService, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _authService = authService;
            _passwordHasher = passwordHasher;
        }

        // Método para autenticar um usuário
        public async Task<User> Authenticate(LoginCommand loginCommand)
        {
            return await _authService.LoginAsync(loginCommand);
        }

        // Método para registrar um novo usuário
        public async Task<OperationResult> Register(RegisterCommand registerCommand)
        {
            if (registerCommand.Password != registerCommand.ConfirmPassword)
                return new OperationResult { Success = false, Message = "As senhas não coincidem." };

            var existingUser = await _userRepository.GetUserByEmail(registerCommand.Email);
            if (existingUser != null)
                return new OperationResult { Success = false, Message = "Usuário já existe." };

            var hashedPassword = _passwordHasher.MaskPassword(registerCommand.Password);
            var user = new User
            {
                Email = registerCommand.Email,
                PasswordHash = hashedPassword,
            };

            await _userRepository.CreateUser(user);
            return new OperationResult { Success = true, Message = "Usuário registrado com sucesso." };
        }

        // Método para atualizar o perfil do usuário
        public async Task<OperationResult> UpdateProfile(string username, UpdateProfileCommand updateProfileCommand)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (user is null)
                return new OperationResult { Success = false, Message = "Usuário não encontrado." };

            user.Name  = updateProfileCommand.FullName;
            user.Phone = updateProfileCommand.PhoneNumber;

            await _userRepository.UpdateUser(user);
            return new OperationResult { Success = true, Message = "Perfil atualizado com sucesso." };
        }

        // Método para alterar a senha do usuário
        public async Task<OperationResult> ChangePassword(string username, ChangePasswordCommand changePasswordCommand)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (user is null)
                return new OperationResult { Success = false, Message = "Usuário não encontrado." };

            if (!_passwordHasher.VerifyPassword(changePasswordCommand.CurrentPassword, user.PasswordHash))
                return new OperationResult { Success = false, Message = "A senha atual está incorreta." };

            user.PasswordHash = _passwordHasher.MaskPassword(changePasswordCommand.NewPassword);
            await _userRepository.UpdateUser(user);

            return new OperationResult { Success = true, Message = "Senha alterada com sucesso." };
        }

        // Método para iniciar o processo de recuperação de senha
        public async Task<OperationResult> ForgotPassword(string email)
        {
            bool mailSend = false;

            try
            {
                var user = await _userRepository.GetUserByEmail(email);
                if (user is null)
                    return new OperationResult { Success = false, Message = "Usuário não encontrado." };

                // Gerar um token de redefinição de senha e enviar por email
                var resetToken = _passwordHasher.MaskPassword(user.Email + DateTime.Now.ToString());

                if (resetToken is null)
                    mailSend = true;

                if(mailSend)
                    return new OperationResult { Success = true, Message = "Token de redefinição de senha enviado." };
                else

                    return new OperationResult { Success = false, Message = "Erro ao gerar o redefinir a senha." };
            }
            catch (Exception)
            {

                throw;
            }

        }

        // Método para redefinir a senha do usuário
        public async Task<OperationResult> ResetPassword(ResetPasswordCommand resetPasswordCommand)
        {
            var user = await _userRepository.GetUserByEmail(resetPasswordCommand.Email);
            if (user == null)
                return new OperationResult { Success = false, Message = "Usuário não encontrado." };

            // Validar o token de redefinição 
            if (!_passwordHasher.VerifyPassword(user.Email + DateTime.Now.ToString(), resetPasswordCommand.Token))
                return new OperationResult { Success = false, Message = "Token de redefinição inválido ou expirado." };

            user.PasswordHash = _passwordHasher.MaskPassword(resetPasswordCommand.NewPassword);
            await _userRepository.UpdateUser(user);

            return new OperationResult { Success = true, Message = "Senha redefinida com sucesso." };
        }

        // Método para obter as informações do usuário
        public async Task<User> GetUserInfo(string username)
        {
            return await _userRepository.GetUserByUsername(username);
        }
    }
}
