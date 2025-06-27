using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PagamentosApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
                    webBuilder
                        .UseUrls($"http://0.0.0.0:{port}")
                        .UseStartup<Startup>();

                });
    }
}
