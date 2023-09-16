
using Bshare.Db;
using Microsoft.EntityFrameworkCore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Bshare
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Enable custom environment variables.
            builder.Configuration.AddEnvironmentVariables(prefix: "bshare_");

            // Store database connection string
            string? connectionString = null;

            // Create environment variable to check dev/prod/staging status
            IHostEnvironment currentEnvironment = builder.Environment;

            // Load different database connection string depending on dev or prod environment
            if (currentEnvironment.IsDevelopment())
            {
                connectionString = builder.Configuration.GetValue<string>("DevConnectionString");
            }
            else if (currentEnvironment.IsProduction())
            {
                connectionString = builder.Configuration.GetValue<string>("ProdConnectionString");
            }

            // Dependency injection for DbContext
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
                pattern: "{controller=FileUpload}/{action=Index}/{id?}");

            app.Run();
        }
    }
}