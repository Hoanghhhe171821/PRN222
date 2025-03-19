using AssignmentPRN222.Models;
using Microsoft.EntityFrameworkCore;

namespace AssignmentPRN222
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            builder.Services.AddDbContext<ProjectPrn222Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            app.MapGet("/", () => "Hello World!");

            //test
            app.Run();
        }
    }
}
