namespace OrderEase.BLL.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }                 // Id(int)
        public string? Number { get; set; }         // Number(nvarchar(max)) *редактируется* используется для фильтрации
        public DateTime? Date { get; set; }         // Date(datetime2(7)) *редактируется* используется для фильтрации 
        public string? UserEmail { get; set; }
        public int ProviderId { get; set; }
    }
}
