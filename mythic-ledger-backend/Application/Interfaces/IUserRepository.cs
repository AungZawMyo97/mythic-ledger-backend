using mythic_ledger_backend.Domain.Entities;

namespace mythic_ledger_backend.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(string id);
}
