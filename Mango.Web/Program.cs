using System;
using Mango.Web.Services;
using Polly;
using Polly.Extensions.Http;

namespace Mango.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            Mango.Web.Models.APIType.APIBaseUrl = builder.Configuration["CouponAPIHost:BaseURL"];
            //builder.Services.AddHttpClient("CouponApi", options =>
            //{
            //    options.BaseAddress = new Uri(builder.Configuration["CouponAPIHost:BaseURL"]);

            //}).AddTransientHttpErrorPolicy(opt =>

            //    opt.WaitAndRetryAsync(3, retryAttempt =>
            //    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))



            //).AddTransientHttpErrorPolicy(opt => opt.CircuitBreakerAsync(3, TimeSpan.FromSeconds(30)))
            //.SetHandlerLifetime(TimeSpan.FromMinutes(5));

            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient<ICouponService, CouponService>(options =>
            {
                options.BaseAddress = new Uri(builder.Configuration["CouponAPIHost:BaseURL"]);
            }).AddTransientHttpErrorPolicy(opt =>
                opt.WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            ).AddTransientHttpErrorPolicy(opt => opt.CircuitBreakerAsync(3, TimeSpan.FromSeconds(30)));
            //since i am injecting base service inside couponservice so that needs to be registered in DI
            builder.Services.AddScoped<IBaseService, BaseService>();
            builder.Services.AddScoped<ICouponService, CouponService>();


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
