using System.ComponentModel.DataAnnotations.Schema;

namespace mythic_ledger_backend.Domain.Entities
{
    public class Customer
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("ingameId")]
        public string IngameId { get; set; }

        [Column("zoneId")]
        public string ZoneId { get; set; }

        [Column("shopAdminUserId")]
        public string ShopAdminUserId { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public User ShopAdmin { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
