namespace Oocx.ACME.Protocol
{
    public class CertificateResponse
    {
        public string Location { get; set; }

        public byte[] Certificate { get; set; }
    }
}