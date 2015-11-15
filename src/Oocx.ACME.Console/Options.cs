using CommandLine;
using CommandLine.Text;

namespace Oocx.ACME.Console
{
    class Options
    {
        [Option('s', "server", Default = "https://acme-v01.api.letsencrypt.org", HelpText = "Specifies the ACME server from which to request certificates.")]
        public string AcmeServer { get; set; }

        [Option('d', "domain", HelpText = "The domain name for which you want to request a certificate.", Required = true)]
        public string Domain { get; set; }

        [Option('a', "acceptTOS", HelpText = "Accept the terms of service of the ACME server")]
        public bool AcceptTermsOfService { get; set; }

        [Option('i', "ignoreSSLErrors", HelpText = "Ignore SSL certificate errors for the HTTPS connection to the ACME server (useful for debugging messages with fiddler)")]
        public bool IgnoreSSLCertificateErrors { get; set; }

        [Option('v', "verbose", HelpText = "Display verbose output")]
        public bool Verbose { get; set; }

        [Option('p', "password", HelpText = "The password used to protect the generated pfx file")]
        public string PfxPassword { get; set; }
    }
}