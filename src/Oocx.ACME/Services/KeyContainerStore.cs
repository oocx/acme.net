using System.Security.Cryptography;

namespace Oocx.Acme.Services
{
    public class KeyContainerStore : IKeyStore
    {
        private readonly CspProviderFlags flags;

        public KeyContainerStore(string storeType)
        {
            flags = storeType == "machine"
                ? CspProviderFlags.UseMachineKeyStore
                : CspProviderFlags.UseUserProtectedKey;
        }

        public RSA GetOrCreateKey(string keyName)
        {
            return new RSACryptoServiceProvider(2048, new CspParameters {
                KeyContainerName = keyName,
                Flags = flags,
            });
        }
    }
}