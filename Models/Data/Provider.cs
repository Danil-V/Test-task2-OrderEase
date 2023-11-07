namespace OrderEase.Models.Data
{
    public class Provider
    {
        public int Id { get; set; }         //- Id(int)
        public string? Name { get; set; }    //- Name(nvarchar(max)) *используется для фильтрации

        public List<Order> Orders { get; set; }
        public Provider()
        {
            Orders = new List<Order>();
        }
    }
}
