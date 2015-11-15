namespace ACME.Protocol.Asn1
{
    public class Name : RDNSequence
    {
        public Name(params RelativeDistinguishedName[] name) : base(name)
        {            
        }   
    }
}