using System.Text;

namespace ACME.Protocol.Asn1
{
    public class UTF8String : Asn1Primitive
    {
        public UTF8String(string text) : base(12)
        {
            Data = Encoding.UTF8.GetBytes(text);
        }
    }
}