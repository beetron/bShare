
using Bshare.Db;
using Microsoft.EntityFrameworkCore;

namespace Bshare
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Environment.SetEnvironmentVariable("bshare_connect",
            //     builder.Configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Enable environment variables.
            builder.Configuration.AddEnvironmentVariables(prefix: "bshare_");


            string connectionString = Environment.GetEnvironmentVariable("bshare_devconnect");
            //
            // if (currentEnvironment == "Development")
            // {
            //     connectionString = Environment.GetEnvironmentVariable("bshare_devconnect");
            // }
            // else if (currentEnvironment == "Production")
            // {
            //     connectionString = Environment.GetEnvironmentVariable("bshare_prodconnect");
            // }


            builder.Services.AddDbContext<BshareDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });


            var app = builder.Build();




            // Configure the HTTP request pipeline.
            // if (!app.Environment.IsDevelopment())
            // {
            //     app.UseExceptionHandler("/Home/Error");
            //     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //     app.UseHsts();
            // }

            if (app.Environment.IsProduction())
            {

                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else if (app.Environment.IsDevelopment())
            {

                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }





            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}