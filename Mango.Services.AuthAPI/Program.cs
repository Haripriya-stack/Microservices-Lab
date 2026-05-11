
using Mango.Services.AuthAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders();

            builder.Services.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection_Coupon_sql"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            ApplyMigrations(app);
            app.Run();
        }
        public static void ApplyMigrations(WebApplication applninstance)
        {
            using var scope = applninstance.Services.CreateScope();
            var _dbinstance= scope.ServiceProvider.GetRequiredService<AppDBContext>();
            if(_dbinstance.Database.GetPendingMigrations().Count() > 0)
            {
                _dbinstance.Database.Migrate();
            }
        }
    }
}
