using System.Text;

namespace ACME.Protocol.Asn1
{
    public class PrintableString : Asn1Primitive
    {
        public PrintableString(string text) : base(0x13)
        {
            //TODO auf erlaubte Zeichen beschränken, siehe https://en.wikipedia.org/wiki/PrintableString
            Data = Encoding.ASCII.GetBytes(text);
        }
    }
}