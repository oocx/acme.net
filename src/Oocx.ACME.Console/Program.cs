using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

using CommandLine;
using Oocx.ACME.Client;
using Oocx.ACME.Services;
using static  Oocx.ACME.Services.Log;

namespace Oocx.ACME.Console
{
    public class Program
    {
        
        public void Main(string[] args)
        {            
            Parser.Default.ParseArguments<Options>(args)                
                .WithNotParsed(ArgumentsError)
                .WithParsed(Execute);
        }
      
        private void Execute(Options options)
        {
            try
            {
                Log.Level = options.Verbosity;
                var process = new Process(options);
                process.Start().GetAwaiter().GetResult();
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

