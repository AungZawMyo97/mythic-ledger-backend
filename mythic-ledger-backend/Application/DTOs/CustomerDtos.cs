namespace mythic_ledger_backend.Application.DTOs;

public class CustomerResponseDto
{
    public string Id { get; set; } = null!;
    public string IngameId { get; set; } = null!;
    public string ZoneId { get; set; } = null!;
    public string ShopAdminUserId { get; set; } = null!;
    public int OrderCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateCustomerRequestDto
{
    public string? IngameId { get; set; }
    public string? ZoneId { get; set; }
}

public class UpdateCustomerRequestDto
{
    public string? IngameId { get; set; }
    public string? ZoneId { get; set; }
}
