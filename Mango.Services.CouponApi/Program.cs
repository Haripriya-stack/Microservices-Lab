using AutoMapper;
using Mango.Services.CouponApi;
using Mango.Services.CouponApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<AppDBContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection_Auth_sql"));
        });
        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingConfig>();
        });

        builder.Services.AddHttpClient("CouponApi", c =>
        {
            c.BaseAddress = new Uri("https://localhost:7001/");
            
        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "MyCouponAPI");

            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

