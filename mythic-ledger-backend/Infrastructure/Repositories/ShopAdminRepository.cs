using Microsoft.EntityFrameworkCore;
using mythic_ledger_backend.Application.Interfaces;
using mythic_ledger_backend.Domain;
using mythic_ledger_backend.Domain.DbContexts;
using mythic_ledger_backend.Domain.Entities;

namespace mythic_ledger_backend.Infrastructure.Repositories
{
    public class ShopAdminRepository : IShopAdminRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public ShopAdminRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<User>> GetAll(UserRole role)
        {
            return await _applicationDbContext.Users.Where(u => u.Role == role).ToListAsync();
        }

        public async Task<User> Create(User requestModel)
        {
            var result = _applicationDbContext.Users.Add(requestModel);
            await _applicationDbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<User> Update(User requestModel)
        {
            var result = _applicationDbContext.Users.Update(requestModel);
            await _applicationDbContext.SaveChangesAsync();

            return result.Entity;
        }
    }
}
