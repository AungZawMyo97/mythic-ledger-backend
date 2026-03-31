using mythic_ledger_backend.Application.DTOs;

namespace mythic_ledger_backend.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}
