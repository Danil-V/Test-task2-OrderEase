using System;

namespace OrderEase.Models.Data
{
    public class Order
    {
        public int Id { get; set; }                 // Id(int)
        public string? Number { get; set; }         // Number(nvarchar(max)) *редактируется* используется для фильтрации
        public DateTime? Date { get; set; }         // Date(datetime2(7)) *редактируется* используется для фильтрации 
        public string? UserEmail { get; set; }


        public OrderItem? Item { get; set; }        // Внешний ключ (связь между таблицами один к одному:    OrderItemId)
        public Provider? Provider { get; set; }     // Внешний ключ (связь между таблицами один к одному:    ProviderId)
    }
}
