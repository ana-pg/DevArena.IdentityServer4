using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevArena.IdentityServer4.Data
{
    public class DevArenaUser
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long Role { get; set; }
        public string SubjectId { get; set; }
        public string Provider { get; set; }
        public string ProviderSubjectId { get; set; }
    }
}
