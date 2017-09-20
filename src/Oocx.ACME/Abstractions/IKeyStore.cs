using System.Security.Cryptography;

namespace Oocx.Acme.Services
{
    public interface IKeyStore
    {
        RSA GetOrCreateKey(string keyName);
    }
}