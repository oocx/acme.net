using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using CommandLine;
using Oocx.Acme.Services;
using static System.Console;

namespace Oocx.Acme.Console
{
    public class Program
    {
        async static Task Main(string[] args)
        {
            var parser = new Parser(config => {
              // config.EnableDashDash = true;
              config.CaseSensitive = true;
              config.IgnoreUnknownArguments = false;
              config.HelpWriter = Out;
            });

            if (args == null || args.Length == 0)
            {
                System.Console.WriteLine("enter a command");

                args = new[] { System.Console.ReadLine() };
            }

            var options = new Options();

            parser.ParseArguments(args, options);

            // TODO: Validate arguments

            try
            {
                await ExecuteAsync(options);
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);

                System.Console.ReadLine();
            }
        }

        private static async Task ExecuteAsync(Options options)
        {
            try
            {
                Log.Level = options.Verbosity;

                var configuration = new ContainerConfiguration();
                var container = configuration.Configure(options);
                if (container == null)
                {
                    return;
                }

                var process = container.Resolve<IAcmeProcess>(new NamedParameter("options",  options));

                await process.StartAsync();
            }
            catch (AggregateException ex)
            {
                var acmeEx = ex.InnerExceptions.OfType<AcmeException>().FirstOrDefault();
                if (acmeEx != null)
                {
                    PrintError(acmeEx);
                }
                else
                {
                    throw;
                }
            }
            catch (AcmeException ex)
            {
                PrintError(ex);
            }
        }

        private static void PrintError(AcmeException ex)
        {
            Log.Error("error:");
            Log.Error(ex.Problem.Type);
            Log.Error(ex.Problem.Detail);
        }
    }
}