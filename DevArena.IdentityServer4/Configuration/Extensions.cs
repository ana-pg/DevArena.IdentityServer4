using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using DevArena.IdentityServer4.Data;

namespace DevArena.IdentityServer4.Configuration
{
    public static class Extensions
    {
        
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

                //seed IS4 data (from appsettings to db)
                data.Seed(configurationContext);
            }

            return host;
        }

        public static Microsoft.AspNetCore.Hosting.IWebHost SeedUsers(this Microsoft.AspNetCore.Hosting.IWebHost host)
        {
            using (var scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //seed users
                var customDbContext = scope.ServiceProvider.GetService<DevArenaDbContext>();
                if (customDbContext.Users.FirstOrDefault(_=>_.Username == "admin") == null)
                    customDbContext.Users.Add(new DevArenaUser() { Username = "admin", Password = "admin", SubjectId = Guid.NewGuid().ToString(), Role = 1 });
                if (customDbContext.Users.FirstOrDefault(_ => _.Username == "guest") == null)
                    customDbContext.Users.Add(new DevArenaUser() { Username = "guest", Password = "guest", SubjectId = Guid.NewGuid().ToString(), Role = 2 });

                customDbContext.SaveChanges();
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

