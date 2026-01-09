using Social.Application.Interfaces;
using Social.Domain.Entities;
using BCrypt.Net;

namespace Social.Application.Services;

public class RegisterUserService
{
    private readonly IUserRepository _userRepository;

    public RegisterUserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> ExecuteAsync(string email, string password)
    {
        var existingUser = await _userRepository.GetByEmailAsync(email);
        if (existingUser != null)
            throw new Exception("User already exists");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User(email, passwordHash);
        await _userRepository.AddAsync(user);

        return user.Id;
    }
}
