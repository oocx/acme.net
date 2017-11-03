using System.Security.Cryptography;

namespace Oocx.Pkcs
{
    public class DigestAlgorithmIdentifier : AlgorithmIdentifier
    {
        public DigestAlgorithmIdentifier(Oid oid) : base(oid)
        {

        }
    }
}