using System.ComponentModel.DataAnnotations;

namespace OrderEase.Models.Web
{
    public class OrderModel
    {
        [Required(ErrorMessage = "Не указан нужный артикул товара")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Не указано требуемое количество")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Не выбраны единицы измерения")]
        public string Unit { get; set; }

        [Required(ErrorMessage = "Не выбран поставщик товара")]
        public string ProviderName { get; set; }
    }
}
