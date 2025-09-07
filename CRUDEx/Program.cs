using CRUDEx.Filters.ActionFilters;
using CRUDEx.ServicesExtensions;
using CRUDEx.SomeInitialData;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Repository;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using Services;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Middlewwares.Middleware;

// [assembly: InternalsVisibleTo(@"XUNIT_CRUD")]
namespace CRUDEx
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.ConfigureServices(builder);
            var app = builder.Build();
            app.UseSerilogRequestLogging();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseExceptionHandlingMiddleware();
            }
            app.UseHttpLogging();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllers();
            app.Run();
        }
    }
}
