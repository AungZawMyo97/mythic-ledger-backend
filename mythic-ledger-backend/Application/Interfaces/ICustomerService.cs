using mythic_ledger_backend.Application.DTOs;

namespace mythic_ledger_backend.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<List<CustomerResponseDto>> GetAllCustomers(string userId, string userRole);
        Task<CustomerResponseDto?> GetCustomerById(string id, string userId, string userRole);
        Task<CustomerResponseDto> CreateCustomer(string shopAdminUserId, CreateCustomerRequestDto requestModel);
        Task<CustomerResponseDto> UpdateCustomer(string id, string userId, string userRole, UpdateCustomerRequestDto requestModel);
        Task DeleteCustomer(string id, string userId, string userRole);
    }
}
