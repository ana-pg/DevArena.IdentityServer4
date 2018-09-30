using DevArena.IdentityServer4.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DevArena.IdentityServer4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).ConfigureIdentityServer().SeedUsers().Run();
        }

        public static IWebHost BuildWebHost(string[] args) => 
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
        
    }
}
