using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniTransport.DAL.Database;
using UniTransport.DAL.Entities;
using UniTransport.DAL.Repository.Implementation;
using UniTransport.DAL.Repository.Interface;

namespace UniTransport.PLL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database Context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Authentication & Identity
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
             options =>
             {
                 options.LoginPath = new PathString("/Account/Login");
                 options.AccessDeniedPath = new PathString("/Account/AccessDenied");
             });

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Account/AccessDenied");
            });


            // Repositories and Services
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ITripRepository, TripRepository>();
             
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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
