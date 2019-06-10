using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HostedConsole
{
    public static class Program
    {
        public static Task Main(string[] args)
        {
            var parser = new CommandLineBuilder(
                new RootCommand
                {
                    Handler = CommandHandler.Create<IHost>(Run)
                })
                .UseDefaults()
                .UseHost(Host.CreateDefaultBuilder, host =>
                {
                    host.ConfigureHostConfiguration(config =>
                    {
                        config.AddUserSecrets(typeof(Program).Assembly, optional: true, reloadOnChange: true);
                    });
                    host.ConfigureServices(ConfigureServices);
                })
                .Build();

            return parser.InvokeAsync(args);
        }

        private static void Run(IHost host)
        {
            var serviceProvider = host.Services;
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(typeof(Program));

            logger.LogInformation("Hello World!");
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            // Add some useful services
        }
    }
}
