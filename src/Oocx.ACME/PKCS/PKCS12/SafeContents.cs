namespace Oocx.Pkcs.PKCS12
{
    public class SafeContents : Sequence
    {
        public SafeContents(params SafeBag[] contents) : base(contents)
        {

        }
    }
}