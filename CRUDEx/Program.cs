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
            // app.Logger.LogDebug("this is for debug");
            // app.Logger.LogWarning("this is for waLogWarning");
            // app.Logger.LogInformation("this is for iLogInformation");
            // app.Logger.LogError("this is for eLogError");
            // app.Logger.LogCritical("this is for cLogCritical");
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
