using mythic_ledger_backend.Domain;
using mythic_ledger_backend.Domain.Entities;

namespace mythic_ledger_backend.Application.Interfaces
{
    public interface IShopAdminRepository
    {
        Task<List<User>> GetAll(UserRole role);
        Task<User> Create(User requestModel);
        Task<User> Update(User requestModel);
    }
}
