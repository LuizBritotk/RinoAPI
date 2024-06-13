using Rino.Domain.Entities;
using Rino.Domain.Interfaces;
using Rino.Domain.Repositories;
using Rino.Domain.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtHandler _jwtHandler;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUserRepository userRepository, IJwtHandler jwtHandler, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _jwtHandler = jwtHandler ?? throw new ArgumentNullException(nameof(jwtHandler));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    public async Task<User> LoginAsync(LoginCommand loginCommand)
    {
        if (loginCommand == null)
            throw new ArgumentNullException(nameof(loginCommand));

        var user = await _userRepository.FindByUsernameAndPassword(loginCommand);

        if (user is null || !_passwordHasher.VerifyPassword(loginCommand.Password, user.PasswordHash))
            return null!;

        user.TokenJWT = _jwtHandler.GenerateToken(user);
        return user;
    }
}
