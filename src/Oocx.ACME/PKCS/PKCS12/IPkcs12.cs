using System.Security.Cryptography;

namespace Oocx.Asn1PKCS.PKCS12
{
    public interface IPkcs12
    {
        void CreatePfxFile(RSAParameters key, string pathToCertificate, string password, string pathToPfx);
    }
}