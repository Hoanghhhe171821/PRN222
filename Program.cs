using AssignmentPRN222.Interfaces;
using AssignmentPRN222.Models;
using AssignmentPRN222.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using AssignmentPRN222;
using Microsoft.AspNetCore.Identity.UI.Services;
using AssignmentPRN222.Services.Vnpay;
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
            builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IProvince, ProvinceRepository>();
            builder.Services.AddTransient<ICinema, CinemaRepository>();
            builder.Services.AddTransient<IMovie, MovieRepository>();
            builder.Services.AddTransient<IRoom, RoomRepository>();
            builder.Services.AddTransient<ISeat, SeatRepository>();
            builder.Services.AddTransient<IShowTime, ShowTimeRepository>();
            builder.Services.AddTransient<ISeatBooking, SeatBookingRepository>();
            builder.Services.AddTransient<IDiscount, DiscountRepository>();
            builder.Services.AddTransient<IOrder, OrderRepository>();
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

            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //test
            app.Run();
        }
    }
}
