using NpgsqlTypes;

namespace mythic_ledger_backend.Domain
{
    public enum UserRole
    {
        [PgName("SUPER_ADMIN")]
        SUPER_ADMIN,

        [PgName("SHOP_ADMIN")]
        SHOP_ADMIN
    }
}
