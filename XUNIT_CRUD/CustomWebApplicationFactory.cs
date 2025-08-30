using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDEx;
using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace XUNIT_CRUD
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseEnvironment("Test");
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<MyDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<MyDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDBForTesting");
                });

            });


            // builder.ConfigureServices(services =>
            // {
            //     var descripter = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<MyDbContext>));
            //     if (descripter != null)
            //     {
            //         services.Remove(descripter);
            //         services.AddDbContext<MyDbContext>(options =>
            //         {
            //             options.UseInMemoryDatabase("IntegrationTestingDatabase");
            //         });
            //     }
            // });
        }
    }
}