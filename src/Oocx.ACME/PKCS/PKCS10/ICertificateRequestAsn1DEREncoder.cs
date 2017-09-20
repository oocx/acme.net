namespace Oocx.Pkcs.PKCS10
{
    public interface ICertificateRequestAsn1DerEncoder
    {
        CertificationRequest Encode(CertificateRequestData requestData);

        byte[] EncodeAsDer(CertificateRequestData requestData);

        string EncodeAsBase64(CertificateRequestData requestData);

        string EncodeAsBase64Url(CertificateRequestData requestData);
    }
}