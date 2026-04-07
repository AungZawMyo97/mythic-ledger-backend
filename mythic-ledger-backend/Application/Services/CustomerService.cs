using mythic_ledger_backend.Application.DTOs;
using mythic_ledger_backend.Application.Interfaces;
using mythic_ledger_backend.Domain;

namespace mythic_ledger_backend.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<List<CustomerResponseDto>> GetAllCustomers(string userId, string userRole)
        {
            _logger.LogInformation("Fetching customers for user {UserId} with role {Role}", userId, userRole);

            try
            {
                var customers = userRole == UserRole.SUPER_ADMIN.ToString()
                    ? await _customerRepository.GetAllAsync()
                    : await _customerRepository.GetAllByShopAdminIdAsync(userId);

                return customers.Select(c => new CustomerResponseDto
                {
                    Id = c.Id,
                    IngameId = c.IngameId,
                    ZoneId = c.ZoneId,
                    ShopAdminUserId = c.ShopAdminUserId,
                    OrderCount = c.Orders?.Count ?? 0,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching customers.");
                throw;
            }
        }

        public async Task<CustomerResponseDto?> GetCustomerById(string id, string userId, string userRole)
        {
            _logger.LogInformation("Fetching customer {CustomerId} for user {UserId}", id, userId);

            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                if (customer == null) return null;

                // SHOP_ADMIN can only access their own customers
                if (userRole == UserRole.SHOP_ADMIN.ToString() && customer.ShopAdminUserId != userId)
                {
                    _logger.LogWarning("User {UserId} attempted to access customer {CustomerId} that does not belong to them.", userId, id);
                    return null;
                }

                return new CustomerResponseDto
                {
                    Id = customer.Id,
                    IngameId = customer.IngameId,
                    ZoneId = customer.ZoneId,
                    ShopAdminUserId = customer.ShopAdminUserId,
                    OrderCount = customer.Orders?.Count ?? 0,
                    CreatedAt = customer.CreatedAt,
                    UpdatedAt = customer.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching customer {CustomerId}.", id);
                throw;
            }
        }

        public async Task<CustomerResponseDto> CreateCustomer(string shopAdminUserId, CreateCustomerRequestDto requestModel)
        {
            _logger.LogInformation("Creating customer for shop admin {ShopAdminUserId}", shopAdminUserId);

            try
            {
                var newCustomer = await _customerRepository.CreateAsync(new Domain.Entities.Customer
                {
                    Id = Guid.NewGuid().ToString(),
                    IngameId = requestModel.IngameId!,
                    ZoneId = requestModel.ZoneId!,
                    ShopAdminUserId = shopAdminUserId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });

                _logger.LogInformation("Successfully created customer {CustomerId}", newCustomer.Id);

                return new CustomerResponseDto
                {
                    Id = newCustomer.Id,
                    IngameId = newCustomer.IngameId,
                    ZoneId = newCustomer.ZoneId,
                    ShopAdminUserId = newCustomer.ShopAdminUserId,
                    OrderCount = 0,
                    CreatedAt = newCustomer.CreatedAt,
                    UpdatedAt = newCustomer.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a customer.");
                throw;
            }
        }

        public async Task<CustomerResponseDto> UpdateCustomer(string id, string userId, string userRole, UpdateCustomerRequestDto requestModel)
        {
            _logger.LogInformation("Updating customer {CustomerId} for user {UserId}", id, userId);

            try
            {
                var existingCustomer = await _customerRepository.GetByIdAsync(id);
                if (existingCustomer == null)
                {
                    throw new InvalidOperationException("Customer not found.");
                }

                // SHOP_ADMIN can only update their own customers
                if (userRole == UserRole.SHOP_ADMIN.ToString() && existingCustomer.ShopAdminUserId != userId)
                {
                    throw new UnauthorizedAccessException("You do not have permission to update this customer.");
                }

                if (!string.IsNullOrWhiteSpace(requestModel.IngameId))
                {
                    existingCustomer.IngameId = requestModel.IngameId;
                }

                if (!string.IsNullOrWhiteSpace(requestModel.ZoneId))
                {
                    existingCustomer.ZoneId = requestModel.ZoneId;
                }

                existingCustomer.UpdatedAt = DateTime.Now;

                var updatedCustomer = await _customerRepository.UpdateAsync(existingCustomer);

                _logger.LogInformation("Successfully updated customer {CustomerId}", updatedCustomer.Id);

                return new CustomerResponseDto
                {
                    Id = updatedCustomer.Id,
                    IngameId = updatedCustomer.IngameId,
                    ZoneId = updatedCustomer.ZoneId,
                    ShopAdminUserId = updatedCustomer.ShopAdminUserId,
                    OrderCount = updatedCustomer.Orders?.Count ?? 0,
                    CreatedAt = updatedCustomer.CreatedAt,
                    UpdatedAt = updatedCustomer.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating customer {CustomerId}.", id);
                throw;
            }
        }

        public async Task DeleteCustomer(string id, string userId, string userRole)
        {
            _logger.LogInformation("Deleting customer {CustomerId} for user {UserId}", id, userId);

            try
            {
                var existingCustomer = await _customerRepository.GetByIdAsync(id);
                if (existingCustomer == null)
                {
                    throw new InvalidOperationException("Customer not found.");
                }

                // SHOP_ADMIN can only delete their own customers
                if (userRole == UserRole.SHOP_ADMIN.ToString() && existingCustomer.ShopAdminUserId != userId)
                {
                    throw new UnauthorizedAccessException("You do not have permission to delete this customer.");
                }

                await _customerRepository.DeleteAsync(existingCustomer);

                _logger.LogInformation("Successfully deleted customer {CustomerId}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting customer {CustomerId}.", id);
                throw;
            }
        }
    }
}
