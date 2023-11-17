namespace OrderEase.BLL.DTO
{
    public class ViewOrderDTO
    {
        public int? OrderId { get; set; }
        public int? OrderItemId { get; set; }
        public int? ProviderId { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime? Date { get; set; }
        public string? UserEmail { get; set; }
        public string? ProductName { get; set; }
        public decimal? Quantity { get; set; }
        public string? Unit { get; set; }
        public string? ProviderName { get; set; }
    }
}
