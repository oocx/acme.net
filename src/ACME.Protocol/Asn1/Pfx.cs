using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;

namespace ACME.Protocol.Asn1
{
    public class Pfx : Sequence
    {
        public Pfx(ContentInfo authSafe, MacData macData) : base(new Integer(3), authSafe, macData)
        {
        }
    }

    public class ContentInfo : Sequence
    {
        public ContentInfo(IContent content) : base(content.Type, content.Content)
        {
        }
    }

    public interface IContent
    {
        ObjectIdentifier Type { get; }

        IAsn1Element Content { get; }
    }

    public class Data
    {
        public Data(OctetString data)
        {
            Content = data;
        }

        ObjectIdentifier Type { get; } = new ObjectIdentifier(Oid.PKCS7.data);

        OctetString Content { get; }
    }

    public class EncryptedData
    {
        public EncryptedData(OctetString data)
        {
            Content = data;
        }

        ObjectIdentifier Type { get; } = new ObjectIdentifier(Oid.PKCS7.encryptedData);

        OctetString Content { get; }
    }

    public class MacData : Sequence
    {
        public MacData(DigestInfo mac, OctetString macSalt) : base(mac, macSalt, new Integer(1))
        {

        }
    }

    public class OctetString : Asn1Primitive
    {
        public OctetString(byte[] data) : base(0x04)
        {
            Data = data;
        }
    }

    public class DigestInfo : Sequence
    {
        public DigestInfo(DigestAlgorithmIdentifier digestAlgorithm, Digest digest) : base(digestAlgorithm, digest)
        {
            
        }
    }

    public class DigestAlgorithmIdentifier : AlgorithmIdentifier
    {
        public DigestAlgorithmIdentifier(string oid) : base(oid)
        {
            
        }
    }

    public class Digest :  OctetString
    {
        public Digest(byte[] data) : base(data)
        {
        }
    }


    public class AuthenticatedSafe : Sequence
    {
        public AuthenticatedSafe(params ContentInfo[] data) : base(data)
        {

        }
    }

    public class SafeContents : Sequence
    {
        public SafeContents(params SafeBag[] contents) : base(contents)
        {

        }
    }

    public interface IBagType
    {
        ObjectIdentifier Type { get; }

        IAsn1Element Content { get; }
    }

    public class SafeContentsBag : IBagType
    {
        public SafeContentsBag(IAsn1Element content) 
        {
            Content = content;
        }

        public ObjectIdentifier Type { get; } = new ObjectIdentifier(Oid.PKCS12.BagType.safeContentsBag);
        public IAsn1Element Content { get; }
    }

    public class CertBag : IBagType
    {
        public CertBag(IAsn1Element content)
        {
            Content = content;
        }

        public ObjectIdentifier Type { get; } = new ObjectIdentifier(Oid.PKCS12.BagType.certBag);
        public IAsn1Element Content { get; }
    }

    public class SafeBag : Sequence
    {
        public SafeBag(IBagType bag) : base(bag.Type, bag.Content)
        {

        }
    }

    public class RSAPrivateKey : Sequence
    {
        // http://blogs.msdn.com/b/shawnfa/archive/2005/11/17/493972.aspx
        public RSAPrivateKey(RSAParameters key) :
            this(new Integer(key.Modulus), new Integer(key.Exponent), new Integer(key.D), new Integer(key.P), new Integer(key.Q), new Integer(key.DP), new Integer(key.DQ), new Integer(key.InverseQ))
        {

        }

        public RSAPrivateKey(Integer modulus, Integer publicExponent, Integer privateExponent, Integer prime1, Integer prime2, Integer exponent1, Integer exponent2, Integer coefficient)
            : base(new Integer(0) /*TODO version*/, modulus, publicExponent, privateExponent, prime1, prime2, exponent1, exponent2, coefficient)
        {

        }
    }
}