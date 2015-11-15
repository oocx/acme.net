using System.Linq;

namespace Oocx.Asn1PKCS.Asn1BaseTypes
{
    public abstract class Asn1Container : IAsn1Element
    {        
        protected Asn1Container(byte tag, IAsn1Element[] children)
        {
            Tag = tag;
            Children = children;
            var childLength = children.Sum(c => c.Size);
            LengthBytes = childLength.ToEncodedLength().ToArray();
            Length = childLength;
            Size = Length + LengthBytes.Length + 1;
        }

        public  IAsn1Element[] Children { get; }

        public byte Tag { get; }

        public byte[] LengthBytes { get; }

        public int Length { get; }

        public int Size { get; protected set; }
    }
}