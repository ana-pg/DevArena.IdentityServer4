using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevArena.IdentityServer4.Data
{
    public interface IUserStore
    {
        Task<bool> ValidateCredentials(string email, string password);

        Task<DevArenaUser> FindBySubjectId(string subjectId);

        Task<DevArenaUser> FindByUsername(string email);

        Task<DevArenaUser> FindByExternalProvider(string provider, string subjectId);

        //Task<DevArenaUser> AutoProvisionUser(string provider, string subjectId, List<Claim> claims);

        Task<bool> SaveAppUser(DevArenaUser user);
    }
}
