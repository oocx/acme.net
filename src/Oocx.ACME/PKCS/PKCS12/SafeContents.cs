using Oocx.Pkcs.Asn1BaseTypes;

namespace Oocx.Pkcs.PKCS12
{
    public class SafeContents : Sequence
    {
        public SafeContents(params SafeBag[] contents) : base(contents)
        {

        }
    }
}