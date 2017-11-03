using System.Security.Cryptography;

namespace Oocx.Pkcs
{
    public static class Oids
    {
        public class Attribute
        {
            public static readonly Oid C  = new Oid("2.5.4.6");
            public static readonly Oid S  = new Oid("2.5.4.8");
            public static readonly Oid L  = new Oid("2.5.4.7");
            public static readonly Oid O  = new Oid("2.5.4.10");
            public static readonly Oid OU = new Oid("2.5.4.11");
            public static readonly Oid CN = new Oid("2.5.4.3");
        }

        public class Algorithm
        {
            public static readonly Oid RSA       = new Oid("1.2.840.113549.1.1.1");
            public static readonly Oid Sha1RSA   = new Oid("1.2.840.113549.1.1.5");
            public static readonly Oid Sha256RSA = new Oid("1.2.840.113549.1.1.11");
        }
        
        // PKCS#7 ----

        public static readonly Oid Data                   = new Oid("1.2.840.113549.1.7.1");
        public static readonly Oid SignedData             = new Oid("1.2.840.113549.1.7.2");
        public static readonly Oid EnvelopedData          = new Oid("1.2.840.113549.1.7.3");
        public static readonly Oid SignedAndEnvelopedData = new Oid("1.2.840.113549.1.7.4");
        public static readonly Oid DigestedData           = new Oid("1.2.840.113549.1.7.5");
        public static readonly Oid EncryptedData          = new Oid("1.2.840.113549.1.7.6");

        // PKCS#12 ----

        public class BagType
        {
            public static readonly Oid KeyBag               = new Oid("1.2.840.113549.1.12.10.1");
            public static readonly Oid Pkcs8ShroudedKeyBag  = new Oid("1.2.840.113549.1.12.10.2");
            public static readonly Oid CertBag              = new Oid("1.2.840.113549.1.12.10.3");
            public static readonly Oid CrlBag               = new Oid("1.2.840.113549.1.12.10.4");
            public static readonly Oid SecretBag            = new Oid("1.2.840.113549.1.12.10.5");
            public static readonly Oid SafeContentsBag      = new Oid("1.2.840.113549.1.12.10.6");
        }
        
    }
}