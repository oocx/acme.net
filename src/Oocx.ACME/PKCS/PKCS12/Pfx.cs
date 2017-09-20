namespace Oocx.Pkcs
{
    public class Pfx : Sequence
    {
        public Pfx(ContentInfo authSafe, MacData macData) 
            : base(new DerInteger(3), authSafe, macData)
        {
        }
    }
}