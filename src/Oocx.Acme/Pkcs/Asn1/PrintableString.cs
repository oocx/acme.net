using System.Text;

namespace Oocx.Pkcs
{
    internal class PrintableString : Asn1Object
    {
        public PrintableString(string text) : base(0x13)
        {
            // TODO auf erlaubte Zeichen beschränken, siehe https://en.wikipedia.org/wiki/PrintableString
            Data = Encoding.ASCII.GetBytes(text);
        }
    }
}