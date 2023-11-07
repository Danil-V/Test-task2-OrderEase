using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OrderEase.Data;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);
            var connection = builder.Configuration.GetConnectionString("DefaultConnection");            // получаем строку подключения из файла конфигурации appsetting.json

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddDbContext<AppDataContext>(options => options.UseSqlite(connection));
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)       // схема аутентификации - с помощью Cookies
                            .AddCookie(options => options.LoginPath = "/login");                        // подключение аутентификации с помощью Cookie

            builder.Services.AddAuthorization();                                                        // добавление сервисов авторизации

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Подключаем систему маршрутизации в конвейер обработки запроса Middlewear:
            app.UseEndpoints(endpoints => { endpoints.MapControllerRoute("default", "{controller=Home}/{action=StartPage}/{id?}"); });

            app.Run();
        }
        catch (Exception ex)
        { Console.WriteLine(ex.Message); }
    }
}