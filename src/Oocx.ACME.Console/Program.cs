using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using static System.Console;

using CommandLine;
using CommandLine.Text;
using Oocx.ACME.Client;
using Oocx.ACME.Common;
using static  Oocx.ACME.Common.Log;

namespace Oocx.ACME.Console
{
    public class Program
    {
        
        public void Main(string[] args)
        {
            Parser parser = new Parser(config =>
            {
              config.EnableDashDash = true;
              config.CaseSensitive = true;
              config.IgnoreUnknownArguments = false;
              config.HelpWriter = Out;
            });
            parser.ParseArguments<Options>(args)              
                .WithNotParsed(ArgumentsError)
                .WithParsed(Execute);
        }
      
        private void Execute(Options options)
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
                process.StartAsync().GetAwaiter().GetResult();
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

        private void ArgumentsError(IEnumerable<Error> errors)
        {
            WriteLine("To use this application before December 3rd 2015, you must be a member of the Let's Encrypt beta program.");
            WriteLine("You can get more information from https://letsencrypt.org/");
        }

        private static void PrintError(AcmeException ex)
        {
            Error("error:");
            Error(ex.Problem.Type);
            Error(ex.Problem.Detail);
        }
    }
}

