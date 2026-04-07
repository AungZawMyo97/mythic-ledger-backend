using Microsoft.EntityFrameworkCore;
using mythic_ledger_backend.Application.Interfaces;
using mythic_ledger_backend.Domain.DbContexts;
using mythic_ledger_backend.Domain.Entities;

namespace mythic_ledger_backend.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CustomerRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<List<Customer>> GetAllByShopAdminIdAsync(string shopAdminUserId)
        {
            return await _applicationDbContext.Customers
                .Where(c => c.ShopAdminUserId == shopAdminUserId)
                .Include(c => c.Orders)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _applicationDbContext.Customers
                .Include(c => c.Orders)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(string id)
        {
            return await _applicationDbContext.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            var result = _applicationDbContext.Customers.Add(customer);
            await _applicationDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            var result = _applicationDbContext.Customers.Update(customer);
            await _applicationDbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteAsync(Customer customer)
        {
            _applicationDbContext.Customers.Remove(customer);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
