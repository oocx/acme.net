using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS12
{
    public class SafeContents : Sequence
    {
        public SafeContents(params SafeBag[] contents) : base(contents)
        {

        }
    }
}