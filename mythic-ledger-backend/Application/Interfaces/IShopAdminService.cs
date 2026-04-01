using mythic_ledger_backend.Application.DTOs;

namespace mythic_ledger_backend.Application.Interfaces
{
    public interface IShopAdminService
    {
        Task<List<ShopAdminResponseDto>> GetAllShopAdmins();
        Task<ShopAdminResponseDto> CreateShopAdmin(CreateShopAdminRequestDto requestModel);
        Task<ShopAdminResponseDto> UpdateShopAdmin(string id, UpdateShopAdminRequestDto requestModel);
    }
}