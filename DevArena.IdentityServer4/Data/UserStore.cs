using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DevArena.IdentityServer4.Data
{
    public class UserStore : IUserStore
    {
        private readonly DevArenaDbContext _dbContext;

        public UserStore(DevArenaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ValidateCredentials(string username, string password)
        {
            var exists = await _dbContext.Users.AnyAsync(_ => _.Username == username && _.Password == password);
            return exists;
        }

        public async Task<DevArenaUser> FindBySubjectId(string subjectId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(_ => _.SubjectId == subjectId);
        }

        public async Task<DevArenaUser> FindByUsername(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(_ => _.Username == username);
        }

        public async Task<DevArenaUser> FindByExternalProvider(string provider, string subjectId)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(_ => _.ProviderSubjectId == subjectId && _.Provider == provider);
        }

        public async Task<DevArenaUser> AutoProvisionUser(string provider, string subjectId, List<Claim> claims)
        {
            DevArenaUser user = new DevArenaUser()
            {
                SubjectId = Guid.NewGuid().ToString(),
                ProviderSubjectId = subjectId,
                Password = "",
                Provider = provider,
                Role = 3,
                Username = claims.FirstOrDefault(_=>_.Type == "name")?.Value
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public Task<bool> SaveAppUser(DevArenaUser user)
        {
            return Task.FromResult<bool>(true);
        }
    }
}
