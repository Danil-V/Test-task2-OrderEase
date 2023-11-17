using Microsoft.AspNetCore.Http;
using OrderEase.BLL.DTO;
using OrderEase.DAL.Data.EF;
using OrderEase.DAL.Data.Models.Data;


namespace OrderEase.WebServer.PL.Services.AuthService
{
    public interface IAuthService
    {
        Task AuthenticateAsync(User user, HttpContext context);
        Task<User> LoginAsync(LoginDTO model, AppDataContext data);
        Task RegistrationAsync(RegisterDTO model, AppDataContext data);
        Task LogoutAsync(HttpContext context);
    }
}
