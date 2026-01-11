using Microsoft.AspNetCore.Hosting;
using P72.API.Trading;
using Zacanbot.ElasticLogger;

namespace P72.Api.Trading
{
    public static class Program
    {
    
public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<Startup>();
               webBuilder.UseIISIntegration();
           }).ConfigureLogging(builder =>
           {
               builder.SetMinimumLevel(LogLevel.Trace);
               //builder.AddLog4Net("log4net.config");
               builder.AddElasticLogger();
           });
}
}
