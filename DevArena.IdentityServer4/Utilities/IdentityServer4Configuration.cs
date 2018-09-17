using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DevArena.IdentityServer4.Utilities
{
    public class IdentityServer4Configuration
    {
        public IdentityServer4Configuration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.development.json");

            IConfigurationRoot configuration = builder.Build();

            ApiResources = new List<ApiResource>();
                //configuration.GetSection("IdentityServer4:ApiResources").Get<List<ApiResource>>();
            IdentityResources = configuration.GetSection("IdentityServer4:IdentityResources").Get<List<IdentityResource>>();
            Clients = new List<Client>();
            //configuration.GetSection("IdentityServer4:Clients").Get<List<Client>>();
        }

        public List<ApiResource> ApiResources { get; set; }

        public List<IdentityResource> IdentityResources { get; set; }

        public List<Client> Clients { get; set; }

        public async Task Seed(DbContext context)
        {
            await context.SaveChangesAsync();
        }
    }
}
