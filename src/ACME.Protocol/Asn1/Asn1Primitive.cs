using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace ACME.Protocol.Asn1
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
}
