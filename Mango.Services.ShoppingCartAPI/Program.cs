
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

namespace Mango.Services.ShoppingCartAPI 
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            // builder.Services.AddOpenApi();

            builder.Services.AddDbContext<AppDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection_ShoppingCart_sql"));
            });
            builder.Services.AddAutoMapper(config =>
            {
                config.AddProfile<MappingConfig>();
            });

            builder.Services.AddHttpClient("ShoppingCartAPI", c =>
            {
                c.BaseAddress = new Uri("https://localhost:7003/");

            });


            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyShoppingCartAPI", Version = "v1" });

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



            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).
                AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,

                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["APISettings:JWTOptions:Issuer"],
                        ValidAudience = builder.Configuration["APISettings:JWTOptions:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["APISettings:JWTOptions:Secret"]!))
                    };
                });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
