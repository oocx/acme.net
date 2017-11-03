namespace Oocx.Pkcs
{
    public class MacData : Sequence
    {
        public MacData(DigestInfo mac, OctetString macSalt) 
            : base(mac, macSalt, new DerInteger(1))
        {

        }
    }
}