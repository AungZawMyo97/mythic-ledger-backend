using mythic_ledger_backend.Application.DTOs;
using mythic_ledger_backend.Application.Interfaces;
using mythic_ledger_backend.Domain;
using mythic_ledger_backend.Infrastructure.Repositories;
using Bcrypt = BCrypt.Net.BCrypt;

namespace mythic_ledger_backend.Application.Services
{
    public class ShopAdminService : IShopAdminService
    {
        private readonly IShopAdminRepository _shopAdminRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<ShopAdminService> _logger;

        public ShopAdminService(IShopAdminRepository shopAdminRepository, IUserRepository userRepository, ILogger<ShopAdminService> logger)
        {
            _shopAdminRepository = shopAdminRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<List<ShopAdminResponseDto>> GetAllShopAdmins()
        {
            _logger.LogInformation("Attempting to fetch all users with the role: {Role}", UserRole.SHOP_ADMIN);

            try
            {
                var shopAdmins = await _shopAdminRepository.GetAll(UserRole.SHOP_ADMIN);
                if (shopAdmins != null && shopAdmins.Any())
                {
                    _logger.LogInformation("Successfully retrieved {Count} Shop Admins from the database.", shopAdmins.Count());

                    return shopAdmins.Select(sa => new ShopAdminResponseDto
                    {
                        Id = sa.Id,
                        Email = sa.Email,
                        MustChangePassword = sa.MustChangePassword,
                        CreatedAt = sa.CreatedAt,
                        UpdatedAt = sa.UpdatedAt
                    }).ToList();

                }
                else
                {
                    _logger.LogWarning("Query executed successfully, but no Shop Admins were found.");
                    return new List<ShopAdminResponseDto>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "A database error occurred while trying to fetch Shop Admins.");
                throw;
            }
        }

        public async Task<ShopAdminResponseDto> CreateShopAdmin(CreateShopAdminRequestDto requestModel)
        {
            _logger.LogInformation("Attempting to create a new Shop Admin with email: {Email}", requestModel.Email);

            try
            {
                var existingUser = await _userRepository.GetByEmailAsync(requestModel.Email!);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("Email is already in use.");
                }

                var hashedPassword = Bcrypt.HashPassword(requestModel.Password);

                var newShopAdmin = await _shopAdminRepository.Create(new Domain.Entities.User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = requestModel.Email!,
                    PasswordHash = hashedPassword,
                    Role = UserRole.SHOP_ADMIN,
                    MustChangePassword = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });

                _logger.LogInformation("Successfully created a new Shop Admin with ID: {Id}", newShopAdmin.Id);
                return new ShopAdminResponseDto
                {
                    Id = newShopAdmin.Id,
                    Email = newShopAdmin.Email,
                    MustChangePassword = newShopAdmin.MustChangePassword,
                    CreatedAt = newShopAdmin.CreatedAt,
                    UpdatedAt = newShopAdmin.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "A database error occurred while trying to create a new Shop Admin.");
                throw;
            }
        }

        public async Task<ShopAdminResponseDto> UpdateShopAdmin(string id, UpdateShopAdminRequestDto requestModel)
        {
            _logger.LogInformation("Attempting to update an existing Shop Admin with ID: {Id}", id);

            try
            {
                var existingShopAdmin = await _userRepository.GetByIdAsync(id);
                if (existingShopAdmin == null)
                {
                    throw new InvalidOperationException("Shop Admin not found.");
                }

                if (!string.IsNullOrWhiteSpace(requestModel.Email))
                {
                    existingShopAdmin.Email = requestModel.Email;
                }

                if (requestModel.MustChangePassword)
                {
                    existingShopAdmin.MustChangePassword = requestModel.MustChangePassword;
                }

                existingShopAdmin.UpdatedAt = DateTime.Now;

                var updatedShopAdmin = await _shopAdminRepository.Update(existingShopAdmin);

                _logger.LogInformation("Successfully updated the Shop Admin with ID: {Id}", updatedShopAdmin.Id);

                return new ShopAdminResponseDto
                {
                    Id = updatedShopAdmin.Id,
                    Email = updatedShopAdmin.Email,
                    MustChangePassword = updatedShopAdmin.MustChangePassword,
                    CreatedAt = updatedShopAdmin.CreatedAt,
                    UpdatedAt = updatedShopAdmin.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "A database error occurred while trying to update the Shop Admin with ID: {Id}", id);
                throw;
            }
        }
    }
}
