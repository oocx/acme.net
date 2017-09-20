namespace Oocx.Pkcs
{
    public class ContentInfo : Sequence
    {
        public ContentInfo(ObjectIdentifier type, IAsn1Element content) 
            : base(type, content)
        {
        }

        // Type

        // Content
    }
}