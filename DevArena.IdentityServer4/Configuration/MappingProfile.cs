
using AutoMapper;
using IdentityServer4.Models;

namespace DevArena.IdentityServer4.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApiResource, global::IdentityServer4.EntityFramework.Entities.ApiResource>();
            CreateMap<IdentityResource, global::IdentityServer4.EntityFramework.Entities.IdentityResource>();
            CreateMap<Client, global::IdentityServer4.EntityFramework.Entities.Client>();

            CreateMap<global::IdentityServer4.EntityFramework.Entities.ApiResource, ApiResource>();
            CreateMap<global::IdentityServer4.EntityFramework.Entities.IdentityResource, IdentityResource > ();
            CreateMap<global::IdentityServer4.EntityFramework.Entities.Client, Client>();
        }
    }
}
