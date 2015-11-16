using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using Oocx.ACME.Services;

namespace Oocx.ACME.Console
{
    public class Options
    {
        [Option('s', "server", Default = "https://acme-v01.api.letsencrypt.org", HelpText = "Specifies the ACME server from which to request certificates.")]
        public string AcmeServer { get; set; }
        
        [Value(0, HelpText = "The names of the domains for which you want to request a certificate.", Required = true, MetaName = "domains")]
        public IEnumerable<string> Domains { get; set; }

        [Option('a', "acceptTOS", HelpText = "Accept the terms of service of the ACME server")]
        public bool AcceptTermsOfService { get; set; }

        [Option('i', "ignoreSSLErrors", HelpText = "Ignore SSL certificate errors for the HTTPS connection to the ACME server (useful for debugging messages with fiddler)")]
        public bool IgnoreSSLCertificateErrors { get; set; }

        [Option('v', "verbosity", HelpText = "Configures the log level (verbose, info, warning, error, disabled).", Default = LogLevel.Info)]
        public LogLevel Verbosity { get; set; }

        [Option('p', "password", HelpText = "The password used to protect the generated pfx file")]
        public string PfxPassword { get; set; }

        [Option('t', "accountKeyContainerType", HelpText = "The base path or key container type that is used to store the acme registration key. 'user' will store the key in a user protected key container, 'machine' uses a machine container. All other inputs will be used as directory name.")]
        public string AccountKeyContainerType { get; set; }

        [Option('n', "accountKeyName", HelpText = "The name of the key file or key container used to store the acme registration key.", Default = "acme-key")]
        public string AccountKeyName { get; set; }
    }
}