namespace Oocx.Pkcs.PKCS12
{
    public class Pfx : Sequence
    {
        public Pfx(ContentInfo authSafe, MacData macData) 
            : base(new Integer(3), authSafe, macData)
        {
        }
    }
}