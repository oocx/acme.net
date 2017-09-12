namespace Oocx.Asn1PKCS.PKCS10
{
    public interface ICertificateRequestAsn1DEREncoder
    {
        CertificationRequest Encode(CertificateRequestData requestData);
        byte[] EncodeAsDER(CertificateRequestData requestData);
        string EncodeAsBase64(CertificateRequestData requestData);
        string EncodeAsBase64Url(CertificateRequestData requestData);
    }
}