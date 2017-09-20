namespace Oocx.Pkcs.PKCS12
{
    public interface IContent
    {
        ObjectIdentifier Type { get; }

        IAsn1Element Content { get; }
    }
}