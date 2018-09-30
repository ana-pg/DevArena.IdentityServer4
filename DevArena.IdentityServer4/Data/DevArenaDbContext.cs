using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevArena.IdentityServer4.Data
{
    public class DevArenaDbContext : DbContext
    {
        public DbSet<DevArenaUser> Users { get; set; }

        public DevArenaDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DevArenaUser>().HasKey(_ => _.Id);
        }

    }

    
}
