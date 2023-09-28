
using Bshare.Db;
using Bshare.Repository;
using Bshare.Services;
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
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            // Dependency injection for IFileUpload repository
            builder.Services.AddScoped<IFilesUploadRepository, FilesUploadRepository>();

            // Dependency injection for IStoreFileService
            builder.Services.AddScoped<IStoreFileService, StoreFileService>();

            var app = builder.Build();

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
                    pattern: "{action}/{id?}",
                    defaults: new { controller = "FileUpload", action = "Upload" });

            app.Run();
        }
    }
}