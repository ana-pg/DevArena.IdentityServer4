using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace DevArena.IdentityServer4.Configuration
{
    public static class Extensions
    {
        /// <summary>
        /// 1. Migrates IdentityServer4 database scheme (using built-in migrations from
        /// IdentityServer4.EntityFramework NuGet package)
        /// 2. Seed IdentityServer4 configuration for SENG solutions (mapped from appsettings)
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static Microsoft.AspNetCore.Hosting.IWebHost ConfigureIdentityServer(this Microsoft.AspNetCore.Hosting.IWebHost host)
        {
            using (var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //create database scheme
                var persistedGrantContext = scope.ServiceProvider.GetService<PersistedGrantDbContext>();
                persistedGrantContext.Database.Migrate();


                var configurationContext = scope.ServiceProvider.GetService<ConfigurationDbContext>();
                configurationContext.Database.Migrate();

                //get data from appsettings
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();
                var data = new IdentityServerConfigurationHelper(configuration);

                //seed data
                data.Seed(configurationContext);
            }

            return host;
        }

        public static void UniqueAddRange(this IList<Claim> originalList, IEnumerable<Claim> claimsToAdd)
        {
            foreach (var claim in claimsToAdd)
            {
                if (originalList.FirstOrDefault(_ => _.Type == claim.Type) == null)
                    originalList.Add(claim);
            }
        }
    }
}

