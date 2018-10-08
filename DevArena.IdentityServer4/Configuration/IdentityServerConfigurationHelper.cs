using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DevArena.IdentityServer4.Configuration
{
    public class IdentityServerConfigurationHelper
    {
        private const string IdentityServerResourcesSection = "IdentityServer4";
        private const string IdentityResourcesSubSection = "IdentityResources";
        private const string ApiResourcesSubSection = "ApiResources";
        private const string ClientsSubSection = "Clients";

        public IdentityServerConfigurationHelper() { }

        public IdentityServerConfigurationHelper(IConfiguration configuration)
        {
            var identityResourcesConfig = configuration.GetSection($"{IdentityServerResourcesSection}:{IdentityResourcesSubSection}");
            IdentityResources = identityResourcesConfig.Get<List<IdentityResource>>();

            var apiResourcesConfig = configuration.GetSection($"{IdentityServerResourcesSection}:{ApiResourcesSubSection}");
            ApiResources = apiResourcesConfig.Get<List<ApiResource>>();

            var clientsConfig = configuration.GetSection($"{IdentityServerResourcesSection}:{ClientsSubSection}");
            Clients = clientsConfig.Get<List<Client>>();
        }

        public List<IdentityResource> IdentityResources { get; set; }

        public List<ApiResource> ApiResources { get; set; }

        public List<Client> Clients { get; set; }

        public void Seed(IConfigurationDbContext context)
        {
            //identity resources
            if (IdentityResources != null)
                foreach (var item in IdentityResources)
                {
                    var existingIdentityResource = context.IdentityResources
                                                          .Include(_ => _.UserClaims)
                                                          .FirstOrDefault(_ => _.Name == item.Name);

                    if (existingIdentityResource == null)
                        context.IdentityResources.Add(item.ToEntity());
                    //else
                    //    Mapper.Map(item, existingIdentityResource);
                }

            //api resources
            if (ApiResources != null)
                foreach (var item in ApiResources)
                {
                    var existingApiResource = context.ApiResources.Include(_ => _.Scopes)
                                                                  .Include(_ => _.Secrets)
                                                                  .Include(_ => _.UserClaims)
                                                                  .FirstOrDefault(_ => _.Name == item.Name);
                    if (existingApiResource == null)
                        context.ApiResources.Add(item.ToEntity());
                    //else
                    //    Mapper.Map(item, existingApiResource);
                }

            //clients
            if (Clients != null)
                foreach (var item in Clients)
                {
                    foreach (var secret in item.ClientSecrets)
                    {
                        secret.Value = secret.Value.Sha256();
                    }
                    var existingClient = context.Clients.Include(_ => _.ClientSecrets)
                                                        .Include(_ => _.AllowedCorsOrigins)
                                                        .Include(_ => _.AllowedScopes)
                                                        .Include(_ => _.Claims)
                                                        .Include(_ => _.AllowedGrantTypes)
                                                        .Include(_ => _.RedirectUris)
                                                        .Include(_ => _.PostLogoutRedirectUris)
                                                        .FirstOrDefault(c => c.ClientId == item.ClientId);
                    if (existingClient == null)
                        context.Clients.Add(item.ToEntity());
                    //else
                    //    Mapper.Map(item, existingClient);


                }

            context.SaveChanges();
        }


    }
}

