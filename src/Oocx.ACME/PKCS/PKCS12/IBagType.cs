namespace Oocx.Pkcs
{
    public interface IBagType
    {
        ObjectIdentifier Type { get; }

        IAsn1Element Content { get; }
    }
}