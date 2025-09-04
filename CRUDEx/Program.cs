using CRUDEx.Filters.ActionFilters;
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
            //builder.Services.Configure<>();
            builder.Services.AddControllersWithViews(op =>
            {
                var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<AddResponseHeaderActionFilter>>();
                op.Filters.Add(new AddResponseHeaderActionFilter(logger, "GlobalHeader", "via GlobalScope Filter", 3));
            });
            builder.Services.AddScoped<ICountriesService, CountryService>();
            builder.Services.AddScoped<IPersonService, PersonService>();
            builder.Services.AddScoped<ICountryRepository, CountryRepository>();
            builder.Services.AddScoped<IPersonRepository, PersonRepository>();
            builder.Services.AddSingleton<SortFlags>();
            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
            builder.Services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]);
            });
            builder.Services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders | HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);

            });
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
