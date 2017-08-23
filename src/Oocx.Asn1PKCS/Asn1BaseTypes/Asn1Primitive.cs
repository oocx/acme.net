using System.Collections.Generic;
using System.Linq;

namespace Oocx.Asn1PKCS.Asn1BaseTypes
{
    public abstract class Asn1Primitive : IAsn1Element
    {
        private byte[] data;

        public byte Tag { get; private set; }

        public int Length { get; private set; }

        public int Size { get; protected set; }

        public byte[] LengthBytes { get; private set; } = { 0 };

        public byte[] Data
        {
            get { return data; }
            set
            {
                data = value;
                LengthBytes = Data.Length.ToEncodedLength().ToArray();
                Length = data.Length;
                Size = Length + LengthBytes.Length + 1;
            }
        }

        protected Asn1Primitive(byte tag)
        {
            Tag = tag;
        }
    }

    public abstract class Asn1Primitive<T> : Asn1Primitive
    {
        public T UnencodedValue { get; protected set; }

        protected Asn1Primitive(byte tag) : base(tag)
        {
        }
    }
}
