namespace Oocx.Pkcs.PKCS12
{
    public class SafeBag : Sequence
    {
        public SafeBag(IBagType bag) : base(bag.Type, bag.Content)
        {

        }
    }
}