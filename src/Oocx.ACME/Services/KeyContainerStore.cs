using System.Security.Cryptography;
using Oocx.ACME.Common;

namespace Oocx.ACME.Services
{
    public class KeyContainerStore : IKeyStore
    {
        private readonly CspProviderFlags flags;

        public KeyContainerStore(string storeType)
        {
            flags = "machine".Equals(storeType)
                ? CspProviderFlags.UseMachineKeyStore
                : CspProviderFlags.UseUserProtectedKey;

            Log.Verbose($"using key container, flags: {flags}");
        }

        public RSA GetOrCreateKey(string keyName)
        {
            Log.Verbose($"using key name {keyName}");

            var csp = new CspParameters {
                KeyContainerName = keyName,
                Flags = flags,
            };

            return new RSACryptoServiceProvider(2048, csp);
        }
    }
}