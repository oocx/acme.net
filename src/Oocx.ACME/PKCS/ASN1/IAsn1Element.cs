namespace Oocx.Pkcs
{
    public interface IAsn1Element : IAsn1Entity
    {
        byte Tag { get; }

        byte[] LengthBytes { get; }

        /// <summary>
        /// The size of the encoded element (tag + lengthBytes.length + data.length)
        /// </summary>
        int Size { get; }
    }
}