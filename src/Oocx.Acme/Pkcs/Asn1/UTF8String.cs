using System.Text;

namespace Oocx.Pkcs
{
    public class UTF8String : Asn1Object
    {
        public UTF8String(string text) : base(12)
        {
            Data = Encoding.UTF8.GetBytes(text);
        }
    }
}