namespace mythic_ledger_backend.Domain.Entities
{
    public class Order
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string OrderTypeId { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal NetProfit { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
