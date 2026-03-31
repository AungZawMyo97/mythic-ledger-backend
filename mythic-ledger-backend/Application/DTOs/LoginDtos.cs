namespace mythic_ledger_backend.Application.DTOs;

public class LoginRequestDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginResponseDto
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public required bool MustChangePassword { get; set; }
    public required string Token { get; set; }
}
