using DataAccess;
using JosephDanielBuhagiarEpSolution.Data; // Contains ApplicationDbContext, PollDbContext, IPollRepository, PollRepository
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JosephDanielBuhagiarEpSolution
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Retrieve the connection string from appsettings.json.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Register the Identity DbContext.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Configure Identity (login and registration).
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Register your custom project DbContext.
            // This context is used for your application-specific data (e.g., polls).
            builder.Services.AddDbContext<PollDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Register your repository for accessing project data.
            builder.Services.AddScoped<IPollRepository, PollRepository>();

            // Add memory cache services (required for session state)
            builder.Services.AddDistributedMemoryCache();

            // Add session services and configure options.
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);  // Session timeout.
                options.Cookie.HttpOnly = true;                  // Make the session cookie HTTP-only.
                options.Cookie.IsEssential = true;               // Mark session cookie as essential.
            });

            builder.Services.AddControllersWithViews();

            // Build the app after all service registrations.
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Enable session middleware. This must come before any middleware that uses session.
            app.UseSession();

            // Add Authentication and Authorization middleware.
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
