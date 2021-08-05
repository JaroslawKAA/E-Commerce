using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Codecool.CodecoolShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // -------- Configuration logging from appsettings.json
            
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            // -------- Configuration logging from script, from below code 
            
            // string outputTemplate = "{Timestamp} {Message}{NewLine:1}{Exception:1}";
            // Log.Logger = new LoggerConfiguration()
            //     .WriteTo.File("./logs/log-.log",
            //         outputTemplate: outputTemplate,
            //         rollingInterval: RollingInterval.Day)
            //     .MinimumLevel.Information()
            //     .Enrich.FromLogContext()
            //     .Enrich.WithMachineName()
            //     .Enrich.WithProcessId()
            //     .Enrich.WithThreadId()
            //     .CreateLogger();
            
            try
            {
                Log.Information("Application Starting.");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}