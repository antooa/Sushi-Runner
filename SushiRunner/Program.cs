using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SushiRunner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
         //"DefaultConnection": "Server=DESKTOP-765A09T;Database=testapp;Trusted_Connection=True;"
    }
}