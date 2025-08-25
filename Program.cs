using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;
using AssignmentPRN222.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using AssignmentPRN222;
using Microsoft.AspNetCore.Identity.UI.Services;
using AssignmentPRN222.Services.Vnpay;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
namespace AssignmentPRN222
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<ProjectPrn222Context>(options =>
                        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddIdentity<UserProfile, IdentityRole>()
                                .AddEntityFrameworkStores<ProjectPrn222Context>()
                                .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IProvince, ProvinceRepository>();
            builder.Services.AddTransient<ICinema, CinemaRepository>();
            builder.Services.AddTransient<IMovie, MovieRepository>();
            builder.Services.AddTransient<IRoom, RoomRepository>();
            builder.Services.AddTransient<ISeat, SeatRepository>();
            builder.Services.AddTransient<IShowTime, ShowTimeRepository>();
            builder.Services.AddTransient<ISeatBooking, SeatBookingRepository>();
            builder.Services.AddTransient<IDiscount, DiscountRepository>();
            builder.Services.AddScoped<IOrder, OrderRepository>();
            builder.Services.AddTransient<IUser, UserRepository>();
            builder.Services.AddSingleton<IEmailSender, FakeEmailSender>();

            builder.Services.AddScoped<IVnPayService, VnPayService>();
            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;
                if (response.StatusCode == 404)
                {
                    var tempData = context.HttpContext.RequestServices
                                    .GetService<ITempDataDictionaryFactory>()
                                    .GetTempData(context.HttpContext);
                    tempData["Message"] = "Trang bạn truy cập không tồn tại";
                    response.Redirect("/Home/Index");
                }
            });
            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=MovieList}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
