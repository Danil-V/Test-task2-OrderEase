using OrderEase.Core.Models.Data;
using OrderEase.Core.Models.Web;
using OrderEase.DBEntity;

namespace OrderEase.WebServer.Services.AuthService
{
    public interface IAuthService
    {
        Task AuthenticateAsync(User user, HttpContext context);
        Task<User> LoginAsync(LoginModel model, AppDataContext data);
        Task RegistrationAsync(RegisterModel model, AppDataContext data);
        Task LogoutAsync(HttpContext context);
    }
}
