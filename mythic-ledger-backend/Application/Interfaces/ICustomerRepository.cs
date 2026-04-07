using mythic_ledger_backend.Domain.Entities;

namespace mythic_ledger_backend.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllByShopAdminIdAsync(string shopAdminUserId);
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(string id);
        Task<Customer> CreateAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task DeleteAsync(Customer customer);
    }
}
