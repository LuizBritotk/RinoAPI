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
    public async Task<OperationResult> RegisterUserAsync(UserCreateCommand registerCommand)
    {
        if (registerCommand == null)
            throw new ArgumentNullException(nameof(registerCommand));

        var existingUser = await _userRepository.GetUserByUsername(registerCommand.Login);

        if (existingUser != null)
            return new OperationResult(false, "Username already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = registerCommand.Email,
            Login = registerCommand.Login,
            FirstName = registerCommand.FirstName,
            LastName = registerCommand.LastName,
            PhoneNumber = registerCommand.PhoneNumber,
            DateOfBirth = registerCommand.DateOfBirth,
            PasswordHash = registerCommand.PasswordHash
        };

        await _userRepository.CreateUser(user);

        user.TokenJWT = _jwtHandler.GenerateToken(user); 

        return new OperationResult(true, "User registered successfully");
    }
}
