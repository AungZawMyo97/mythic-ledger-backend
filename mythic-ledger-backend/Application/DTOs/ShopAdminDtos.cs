using mythic_ledger_backend.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace mythic_ledger_backend.Application.DTOs;

public class ShopAdminResponseDto
{
    public string? Id { get; set; }
    public string? Email { get; set; }
    public bool MustChangePassword { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateShopAdminRequestDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class UpdateShopAdminRequestDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool MustChangePassword { get; set; }
}
