using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.PKCS12
{
    public class ContentInfo : Sequence
    {
        public ContentInfo(IContent content) : base(content.Type, content.Content)
        {
        }
    }
}