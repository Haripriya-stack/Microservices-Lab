
using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Services;
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
            builder.Services.AddSwaggerGen();
            

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders();

            builder.Services.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection_Auth_sql"));
            });

            builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("APISettings:JWTOptions"));
            
            builder.Services.AddScoped<IAuthService,AuthService>();
     

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "MyAuthAPI");

                });
               
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
