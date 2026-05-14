using System;
using Mango.Web.Services;
using Mango.Web.Models;
using Polly;
using Polly.Extensions.Http;
using static Mango.Web.Models.APIType;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace Mango.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

       AuthAPIBaseUrl = builder.Configuration["BaseURLs:AuthAPI"];
         CouponAPIBaseUrl = builder.Configuration["BaseURLs:CouponAPI"];
            //builder.Services.AddHttpClient("CouponApi", options =>
            //{
            //    options.BaseAddress = new Uri(builder.Configuration["CouponAPIHost:BaseURL"]);

            //}).AddTransientHttpErrorPolicy(opt =>

            //    opt.WaitAndRetryAsync(3, retryAttempt =>
            //    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))



            //).AddTransientHttpErrorPolicy(opt => opt.CircuitBreakerAsync(3, TimeSpan.FromSeconds(30)))
            //.SetHandlerLifetime(TimeSpan.FromMinutes(5));
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
            {
                options.LoginPath = "/Auth/Login";
                options.LogoutPath = "/Auth/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            });

            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient<ICouponService, CouponService>(options =>
            {
                options.BaseAddress = new Uri(CouponAPIBaseUrl);
            }).AddTransientHttpErrorPolicy(opt =>
                opt.WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            ).AddTransientHttpErrorPolicy(opt => opt.CircuitBreakerAsync(3, TimeSpan.FromSeconds(30)));
            
            builder.Services.AddHttpClient<IAuthService, AuthService>(opt => opt.BaseAddress = new Uri(AuthAPIBaseUrl))
                .AddTransientHttpErrorPolicy(opt =>
                
                    opt.WaitAndRetryAsync(3,retryattempt=> TimeSpan.FromSeconds(Math.Pow(2,retryattempt)))
                ).AddTransientHttpErrorPolicy(opt => opt.CircuitBreakerAsync(3, TimeSpan.FromSeconds(30)))
                .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            //since i am injecting base service inside couponservice so that needs to be registered in DI
            builder.Services.AddScoped<IBaseService, BaseService>();
            builder.Services.AddScoped<ICouponService, CouponService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenStoreProvider, TokenStoreProvider>();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Coupon}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
