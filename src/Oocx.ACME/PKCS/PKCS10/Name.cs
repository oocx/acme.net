namespace Oocx.Pkcs
{
    public class Name : RDNSequence
    {
        public Name(params RelativeDistinguishedName[] names)
            : base(names) { }
    }
}