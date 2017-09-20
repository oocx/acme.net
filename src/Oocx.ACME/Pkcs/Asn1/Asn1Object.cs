using System.Linq;

namespace Oocx.Pkcs
{
    public abstract class Asn1Object : IAsn1Element
    {
        private byte[] data;

        public byte Tag { get; private set; }

        public int Length => data.Length;

        public int Size => data.Length + LengthBytes.Length + 1;

        public byte[] LengthBytes { get; private set; } = { 0 };

        public byte[] Data
        {
            get { return data; }
            set
            {
                data = value;
                LengthBytes = Data.Length.ToEncodedLength().ToArray();
            }
        }

        protected Asn1Object(byte tag)
        {
            Tag = tag;
        }

        // public Encode(Stream stream) { } 

        // Tag + LengthBytes + Data
    }

    public abstract class Asn1Object<T> : Asn1Object
    {
        public T UnencodedValue { get; protected set; }

        protected Asn1Object(byte tag) : base(tag)
        {
        }
    }
}
