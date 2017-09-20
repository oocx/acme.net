using System;
using Autofac;

using Oocx.ACME.Client;
using Oocx.ACME.IIS;
using Oocx.ACME.Services;
using Oocx.Pkcs;
using Oocx.Pkcs.PKCS10;
using Oocx.Pkcs.PKCS12;
using static Oocx.ACME.Logging.Log;

namespace Oocx.ACME.Console
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

            if ("user".Equals(options.AccountKeyContainerLocation) || "machine".Equals(options.AccountKeyContainerLocation))
            {
                builder.RegisterType<KeyContainerStore>().As<IKeyStore>().WithParameter("storeType", options.AccountKeyContainerLocation);
            }
            else
            {
                builder.RegisterType<FileKeyStore>().As<IKeyStore>().WithParameter("basePath", options.AccountKeyContainerLocation ?? Environment.CurrentDirectory);
            }

            if ("manual-http-01".Equals(options.ChallengeProvider, StringComparison.OrdinalIgnoreCase))
            {
                builder.RegisterType<ManualChallengeProvider>().As<IChallengeProvider>();                
            }
            else if ("iis-http-01".Equals(options.ChallengeProvider, StringComparison.OrdinalIgnoreCase))
            {
                builder.RegisterType<IISChallengeProvider>().As<IChallengeProvider>();             
            }
            else
            {
                Error($"unsupported challenge provider: {options.ChallengeProvider}");
                return null;
            }

            if ("iis".Equals(options.ServerConfigurationProvider))
            {
                builder.RegisterType<IISServerConfigurationProvider>().As<IServerConfigurationProvider>();
            }
            else
            {
                builder.RegisterType<ManualServerConfigurationProvider>().As<IServerConfigurationProvider>();
            }

            builder.RegisterType<Pkcs12>().As<IPkcs12>();
            builder.RegisterType<Asn1Serializer>().As<IAsn1Serializer>();
            builder.RegisterType<CertificateRequestAsn1DEREncoder>().As<ICertificateRequestAsn1DEREncoder>();

            return builder.Build();
        }        
    }
}