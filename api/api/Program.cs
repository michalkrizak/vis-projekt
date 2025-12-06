
//using api.Data;
using api.Models;
using api.Services;
using api.BLL.Interfaces;
using api.DAL.Interfaces;
using api.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                 });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<VolejbalContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // DAL layer - Repository registrations
            builder.Services.AddScoped<ISestavaZapasuDao, SestavaZapasuRepository>();
            builder.Services.AddScoped<IZapasDao, ZapasRepository>();
            builder.Services.AddScoped<ISezonaDao, SezonaRepository>();
            builder.Services.AddScoped<ITymDao, TymRepository>();
            builder.Services.AddScoped<IHracDao, HracRepository>();
            builder.Services.AddScoped<ILoginDao, LoginRepository>();

            // BLL layer - Service registrations
            builder.Services.AddScoped<IZapasService, api.BLL.Services.ZapasService>();
            builder.Services.AddScoped<ILoginService, api.BLL.Services.LoginService>();
            
            // Legacy services (keeping for backward compatibility)
            builder.Services.AddScoped<ZapasService>();
            builder.Services.AddScoped<SqlService>();

            // Session Storage - druhý způsob ukládání dat
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseSession();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
