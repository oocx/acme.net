using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using CommandLine;
using Oocx.ACME.Client;
using Oocx.ACME.Common;
using static System.Console;
using static Oocx.ACME.Common.Log;

namespace Oocx.ACME.Console
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

            var options = new Options();

            parser.ParseArguments(args, options);

            // TODO: Validate arguments

            await ExecuteAsync(options);
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
            Error("error:");
            Error(ex.Problem.Type);
            Error(ex.Problem.Detail);
        }
    }
}