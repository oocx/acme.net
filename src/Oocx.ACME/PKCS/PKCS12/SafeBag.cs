using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS12
{
    public class SafeBag : Sequence
    {
        public SafeBag(IBagType bag) : base(bag.Type, bag.Content)
        {

        }
    }
}