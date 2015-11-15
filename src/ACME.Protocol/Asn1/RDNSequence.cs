namespace ACME.Protocol.Asn1
{
    public class RDNSequence : Sequence
    {
        public RDNSequence(params RelativeDistinguishedName[] names) : base(names)
        {
            
        }
    }
}