using Oocx.Pkcs;

namespace Oocx.Pkcs.PKCS12
{
    public class AuthenticatedSafe : Sequence
    {
        public AuthenticatedSafe(params ContentInfo[] data) : base(data)
        {

        }
    }
}