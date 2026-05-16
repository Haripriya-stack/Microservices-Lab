using AutoMapper;
using Mango.Services.CouponApi;
using Mango.Services.CouponApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
//using Microsoft.OpenApi.Models;
using System.Text;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
      // builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen( options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyCouponAPI", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                BearerFormat = "JWT",
                Description = "Enter token required for authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"

            });
            options.AddSecurityRequirement(document => new() 
            { [new OpenApiSecuritySchemeReference("Bearer", document)] = [] });

        });
    

        string? secret = builder.Configuration.GetValue<string>("APISettings:JWTOptions:Secret") ;
        string? issuer = builder.Configuration.GetValue<string>("APISettings:JWTOptions:Issuer");
        string? audience = builder.Configuration.GetValue<string>("APISettings:JWTOptions:Audience");

        builder.Services.AddDbContext<AppDBContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection_Coupon_sql"));
        });
        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingConfig>();
        });

        builder.Services.AddHttpClient("CouponApi", c =>
        {
            c.BaseAddress = new Uri("https://localhost:7001/");
            
        });

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {

            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                
                ValidateIssuerSigningKey = false,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!)),
              
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    //Console.WriteLine(context.Exception.Message);
                    //return Task.CompletedTask;
                    throw context.Exception;
                }
            };

        });
        //builder.Services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        //    options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
        //});
        builder.Services.AddAuthorization();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
          // app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();

            //    options =>


            //{
            //    ///swagger/v1/swagger.json"
            //    options.SwaggerEndpoint("/openapi/v1.json", "MyCouponAPI");

            //});
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

