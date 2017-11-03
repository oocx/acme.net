namespace Oocx.Pkcs
{
    public class AuthenticatedSafe : Sequence
    {
        public AuthenticatedSafe(params ContentInfo[] data)
            : base(data)
        {

        }
    }
}