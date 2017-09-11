using System;
using System.Security.Cryptography;

namespace Oocx.Asn1PKCS.PKCS10
{
    public class CertificateRequestData
    {
        public CertificateRequestData(string commonName, RSAParameters key)
        {
            Key = key;
            CN = commonName ?? throw new ArgumentNullException(nameof(commonName));
        }

        /// <summary>
        /// Country
        /// </summary>
        /// <see cref="http://www.alvestrand.no/objectid/2.5.4.6.html"/>
        public string C { get; set; }

        /// <summary>
        /// State or Province Name
        /// </summary>
        /// <see cref="http://www.alvestrand.no/objectid/2.5.4.8.html"/>
        public string S { get; set; }

        /// <summary>
        /// Locality Name
        /// </summary>
        /// <see cref="http://www.alvestrand.no/objectid/2.5.4.7.html"/>
        public string L { get; set; }

        /// <summary>
        /// Organization Name
        /// </summary>
        /// <see cref="http://www.alvestrand.no/objectid/2.5.4.10.html"/>
        public string O { get; set; }

        /// <summary>
        /// Organizational Unit Name
        /// </summary>
        /// <see cref="http://www.alvestrand.no/objectid/2.5.4.11.html"/>
        public string OU { get; set; }

        /// <summary>
        /// Common Name
        /// </summary>
        /// <see cref="http://www.alvestrand.no/objectid/2.5.4.3.html"/>
        public string CN { get; }

        public RSAParameters Key { get; }
    }


    //public abstract class Asn1ComplexType : IAsn1Entity
    //{
    //    protected Asn1ComplexType(params IAsn1Entity[] children)
    //    {
    //        Children = children;
    //    }

    //    public IAsn1Entity[] Children { get; }
    //}
}