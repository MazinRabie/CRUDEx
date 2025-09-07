using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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

namespace CRUDEx.ServicesExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, WebApplicationBuilder builder)
        {
            //services.Configure<>();
            services.AddControllersWithViews(op =>
            {
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<AddResponseHeaderActionFilter>>();
                op.Filters.Add(new AddResponseHeaderActionFilter(logger, "GlobalHeader", "via GlobalScope Filter", 3));
            });
            services.AddScoped<ICountriesService, CountryService>();
            services.AddScoped<IPersonAdderService, PersonAdderService>();
            services.AddScoped<IPersonGetterService, PersonGetterServiceModified_OCP>();
            services.AddScoped<PersonGetterService, PersonGetterService>();
            services.AddScoped<IPersonUpdaterService, PersonUpdaterService>();
            services.AddScoped<IPersonDeleterService, PersonDeleterService>();
            services.AddScoped<IPersonSorterService, PersonSorterService>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddSingleton<SortFlags>();
            services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
            services.AddDbContext<MyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:Default"]);
            });
            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders | HttpLoggingFields.ResponsePropertiesAndHeaders;
            });
            builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);

            });
            return services;
        }

    }
}