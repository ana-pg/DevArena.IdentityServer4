using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Validation;

namespace DevArena.IdentityServer4.Data
{
    public class ResourceOwnerPasswordValidator :IResourceOwnerPasswordValidator
    {
        private readonly IUserStore _users;

        public ResourceOwnerPasswordValidator(IUserStore users)
        {
            _users = users;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _users.FindByUsername(context.UserName);

            if (user != null)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("role", user.Role.ToString()));
                context.Result = new GrantValidationResult(subject: user.SubjectId, authenticationMethod: "password",
                    claims: claims);
            }

        }
    }
}
