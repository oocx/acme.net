namespace Oocx.Pkcs.PKCS10
{
    public class Name : RDNSequence
    {
        public Name(params RelativeDistinguishedName[] names)
            : base(names) { }
    }
}