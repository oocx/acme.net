namespace Oocx.Pkcs
{
    public class SafeBag : Sequence
    {
        public SafeBag(IBagType bag) : base(bag.Type, bag.Content)
        {

        }
    }
}