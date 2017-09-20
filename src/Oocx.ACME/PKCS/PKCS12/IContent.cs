namespace Oocx.Pkcs
{
    public interface IContent
    {
        ObjectIdentifier Type { get; }

        IAsn1Element Content { get; }
    }
}