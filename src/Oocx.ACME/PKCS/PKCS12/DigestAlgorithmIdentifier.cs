using System.Security.Cryptography;

namespace Oocx.Pkcs.PKCS12
{
    public class DigestAlgorithmIdentifier : AlgorithmIdentifier
    {
        public DigestAlgorithmIdentifier(Oid oid) : base(oid)
        {

        }
    }
}