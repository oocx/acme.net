using System.Security.Cryptography;

namespace Oocx.ACME.Services
{
    public interface IKeyStore
    {
        RSA GetOrCreateKey(string keyName);
    }
}