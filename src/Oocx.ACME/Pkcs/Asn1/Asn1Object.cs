using System.Linq;

namespace Oocx.Pkcs
{
    public abstract class Asn1Object : IAsn1Element
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

        protected Asn1Object(byte tag)
        {
            Tag = tag;
        }
    }

    public abstract class Asn1Object<T> : Asn1Object
    {
        public T UnencodedValue { get; protected set; }

        protected Asn1Object(byte tag) : base(tag)
        {
        }
    }
}
