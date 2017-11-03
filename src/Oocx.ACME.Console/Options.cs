using System.Collections.Generic;

using CommandLine;

using Oocx.Acme.Services;

namespace Oocx.Acme.Console
{
    public class Options
    {
        [Option('s', "server", DefaultValue = "https://acme-v01.api.letsencrypt.org", HelpText = "Specifies the ACME server from which to request certificates.")]
        public string AcmeServer { get; set; }
        
        [Option('d', "domain", HelpText = "The names of the domains for which you want to request a certificate.", Required = true)]
        public IEnumerable<string> Domains { get; set; }

        [Option('a', "acceptTermsOfService", HelpText = "Accept the terms of service of the ACME server")]
        public bool AcceptTermsOfService { get; set; }

        [Option('t', "termsOfServiceUri", HelpText = "The uri of the terms of service that you accept.")]
        public string TermsOfServiceUri { get; set; }

        [Option("ignoreSSLErrors", HelpText = "Ignore SSL certificate errors for the HTTPS connection to the ACME server (useful for debugging messages with fiddler)")]
        public bool IgnoreSSLCertificateErrors { get; set; }

        [Option('v', "verbosity", HelpText = "Configures the log level (Verbose, Info, Warning, Error, Disabled - casing is important).", DefaultValue = LogLevel.Info)]
        public LogLevel Verbosity { get; set; }

        [Option('p', "password", HelpText = "The password used to protect the generated pfx file")]
        public string PfxPassword { get; set; }

        [Option('l', "accountKeyContainerLocation", HelpText = "The base path or key container location that is used to store the acme registration key. 'user' will store the key in a user protected key container, 'machine' uses a machine container. All other inputs will be used as directory name.")]
        public string AccountKeyContainerLocation { get; set; }

        [Option('n', "accountKeyName", HelpText = "The name of the key file or key container used to store the acme registration key.", DefaultValue = "acme-key")]
        public string AccountKeyName { get; set; }

        [Option('c', "challengeProvider", HelpText = "The type of web server integration to use for ACME challenges. Supported types are: 'manual-http-01' (no integration with http-01 challenge), 'iis-http-01' (IIS with http-01 challenge)", DefaultValue = "iis-http-01")]
        public string ChallengeProvider { get; set; }

        [Option('i', "serverConfigurationProvider", HelpText = "The type of web server configuration to use to install and configure certificates. Supported types are: 'manual' (no integration), 'iis' (installs certificates to localmachine\\my and configures IIS bindings)", DefaultValue = "iis")]
        public string ServerConfigurationProvider { get; set; }

        [Option('w', "iisWebSite", HelpText = "The IIS web site that should be configured to use the new certificate (used with --serverConfigurationProvider iis). If you do not specifiy a web site, the provider will try to find a matching site with a binding for your domain.")]
        public string IISWebSite { get; set; }

        [Option('b', "iisBinding", HelpText = "The IIS binding that should be configured to use the new certificate. Syntax: ip:port:hostname, for example '*:443:www.example.com' (used with --serverConfigurationProvider iis). If you do not specifiy a binding, the provider will try to find a matching binding. It will create a binding if no matching binding exists.")]
        public string IISBinding { get; set; }

        [Option('m', "contact", HelpText = "The contact information for the registration request. Example: mailto:you@example.com")]
        public string Contact { get; set; }

        [Option('j', "acceptInstructions", HelpText = "Automatically accept the instructions.")]
        public bool AcceptInstructions { get; set; }
    }
}