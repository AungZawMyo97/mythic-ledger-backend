using System.ComponentModel.DataAnnotations.Schema;

namespace mythic_ledger_backend.Domain.Entities
{
    public class User
    {
        [Column("id")]
        public string Id { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("passwordHash")]
        public string PasswordHash { get; set; }
        [Column("role")]
        public UserRole Role { get; set; }
        [Column("mustChangePassword")]
        public bool MustChangePassword { get; set; } = true;
        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }
        [Column("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
