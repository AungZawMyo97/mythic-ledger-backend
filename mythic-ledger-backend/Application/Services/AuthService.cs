using Microsoft.IdentityModel.Tokens;
using mythic_ledger_backend.Application.DTOs;
using mythic_ledger_backend.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bcrypt = BCrypt.Net.BCrypt;

namespace mythic_ledger_backend.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepository, IConfiguration config)
    {
        _userRepository = userRepository;
        _config = config;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null) return null;

        bool isPasswordValid = Bcrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid) return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpirationInMinutes"]!)),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new LoginResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role.ToString(),
            MustChangePassword = user.MustChangePassword,
            Token = tokenHandler.WriteToken(token)
        };
    }
}
