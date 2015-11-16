using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

using CommandLine;
using Oocx.ACME.Client;
using Oocx.ACME.Services;

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
            foreach (var error in errors)
            {
                WriteLine(error);
            }
        }

        private static void PrintError(AcmeException ex)
        {
            WriteLine("error:");
            WriteLine(ex.Problem.Type);
            WriteLine(ex.Problem.Detail);
        }
    }
}

