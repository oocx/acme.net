using System;
using Autofac;

using Oocx.Acme.IIS;
using Oocx.Acme.Services;


namespace Oocx.Acme.Console
{
    class ContainerConfiguration
    {
        public IContainer Configure(Options options)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AcmeProcess>().As<IAcmeProcess>();
            builder.RegisterType<AcmeClient>().As<IAcmeClient>()
                .WithParameter("baseAddress", options.AcmeServer)
                .WithParameter("keyName", options.AccountKeyName )
                .SingleInstance();

            if ("user" == options.AccountKeyContainerLocation || "machine" == options.AccountKeyContainerLocation)
            {
                builder.RegisterType<KeyContainerStore>().As<IKeyStore>().WithParameter("storeType", options.AccountKeyContainerLocation);
            }
            else
            {
                builder.RegisterType<FileKeyStore>().As<IKeyStore>().WithParameter("basePath", options.AccountKeyContainerLocation ?? Environment.CurrentDirectory);
            }

            if ("manual-http-01".Equals(options.ChallengeProvider))
            {
                builder.RegisterType<ManualChallengeProvider>().As<IChallengeProvider>();                
            }
            else if ("iis-http-01".Equals(options.ChallengeProvider))
            {
                builder.RegisterType<IISChallengeProvider>().As<IChallengeProvider>();             
            }
            else
            {
                Log.Error($"unsupported challenge provider: {options.ChallengeProvider}");
                return null;
            }

            if ("iis" == options.ServerConfigurationProvider)
            {
                builder.RegisterType<IISServerConfigurationProvider>().As<IServerConfigurationProvider>();
            }
            else
            {
                builder.RegisterType<ManualServerConfigurationProvider>().As<IServerConfigurationProvider>();
            }

            return builder.Build();
        }        
    }
}