namespace OrderEase.Models.Data
{
    public class OrderItem
    {

        public int Id { get; set; }                 // Id(int)
        public string? Name { get;set; }             // Name(nvarchar(max)) *редактируется* используется для фильтрации
        public decimal? Quantity { get; set; }       // Quantity decimal (18, 3) *редактируется 
        public string? Unit {  get; set; }           // Unit(nvarchar(max)) *редактируется* используется для фильтрации

        public int OrderId { get; set; }            // OrderId(int)
        public Order? Oreder { get; set; }
    }
}
