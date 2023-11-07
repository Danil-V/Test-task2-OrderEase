namespace OrderEase.Models.Data
{
    public class User
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public int RoleId { get; set; }     // Внешний ключ (связь между таблицами один к одному)
        public Role? Role { get; set; }
    }
}
