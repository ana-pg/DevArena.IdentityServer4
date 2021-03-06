﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace DevArena.IdentityServer4.Data
{
    public class ProfileService : IProfileService
    {
        private readonly IUserStore _users;

        public ProfileService(IUserStore users)
        {
            _users = users;
        }

        public virtual async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var user = await _users.FindBySubjectId(subjectId);

            //todo see requested scopes
            //reorganize scopes based on users role
            var roleCLaim = context.Subject.Claims.FirstOrDefault(_ => _.Type == "role");
            if (roleCLaim == null && user != null)
            {
                roleCLaim = new Claim("role", user.Role.ToString());
            }
            if (roleCLaim == null)
                roleCLaim = new Claim("role", "3");

            context.IssuedClaims.Add(roleCLaim);
            //else if (context.Client.AllowedGrantTypes.Contains("password"))
            //    context.IssuedClaims.Add(new Claim("role", "3"));

        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return;
        }
    }
}
