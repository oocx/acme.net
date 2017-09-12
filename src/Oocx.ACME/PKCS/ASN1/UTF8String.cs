using System.Text;

namespace Oocx.Asn1PKCS.Asn1BaseTypes
{
    public class UTF8String : Asn1Primitive
    {
        public UTF8String(string text) : base(12)
        {
            Data = Encoding.UTF8.GetBytes(text);
        }
    }
}