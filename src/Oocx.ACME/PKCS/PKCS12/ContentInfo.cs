namespace Oocx.Pkcs
{
    public class ContentInfo : Sequence
    {
        public ContentInfo(IContent content) : base(content.Type, content.Content)
        {
        }
    }
}