using System.Security.Cryptography;

namespace Oocx.Pkcs
{
    public interface IPkcs12
    {
        void CreatePfxFile(RSAParameters key, string pathToCertificate, string password, string pathToPfx);
    }
}