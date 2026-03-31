using Microsoft.EntityFrameworkCore;
using mythic_ledger_backend.Application.Interfaces;
using mythic_ledger_backend.Domain.DbContexts;
using mythic_ledger_backend.Domain.Entities;

namespace mythic_ledger_backend.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UserRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _applicationDbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
    }

}
