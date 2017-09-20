namespace Oocx.Pkcs
{
    public class Oid
    {
        public class Attribute
        {
            public const string C = "2.5.4.6";
            public const string S = "2.5.4.8";
            public const string L = "2.5.4.7";
            public const string O = "2.5.4.10";
            public const string OU = "2.5.4.11";
            public const string CN = "2.5.4.3";
        }

        public class Algorithm
        {
            public const string RSA = "1.2.840.113549.1.1.1";
            public const string sha1RSA = "1.2.840.113549.1.1.5";
            public const string sha256RSA = "1.2.840.113549.1.1.11";
        }

        public class PKCS7
        {
            public const string data = "1.2.840.113549.1.7.1";
            public const string signedData = "1.2.840.113549.1.7.2";
            public const string envelopedData = "1.2.840.113549.1.7.3";
            public const string signedAndEnvelopedData = "1.2.840.113549.1.7.4";
            public const string digestedData = "1.2.840.113549.1.7.5";
            public const string encryptedData = "1.2.840.113549.1.7.6";
        }

        public class PKCS12
        {
            public class BagType
            {
                public const string keyBag = "1.2.840.113549.1.12.10.1";
                public const string pkcs8ShroudedKeyBag = "1.2.840.113549.1.12.10.2";
                public const string certBag = "1.2.840.113549.1.12.10.3";
                public const string crlBag = "1.2.840.113549.1.12.10.4";
                public const string secretBag = "1.2.840.113549.1.12.10.5";
                public const string safeContentsBag = "1.2.840.113549.1.12.10.6";
            }
        }
    }
}