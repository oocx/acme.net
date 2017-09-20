namespace Oocx.Pkcs.PKCS12
{
    public class ContentInfo : Sequence
    {
        public ContentInfo(IContent content) : base(content.Type, content.Content)
        {
        }
    }
}