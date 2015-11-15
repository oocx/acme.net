using System.Collections.Generic;
using System.Linq;

namespace ACME.Protocol.Asn1
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/bb540809(v=vs.85).aspx
    /// </summary>
    public class ObjectIdentifier : Asn1Primitive
    {                        
        public ObjectIdentifier(string id) : base(6)
        {
            var nodes = id.Split('.').Select(int.Parse).ToArray();
            Data = GetData(nodes).ToArray();
        }

        private IEnumerable<byte> GetData(int[] nodes)
        {
            yield return (byte)(nodes[0] * 40 + nodes[1]);
            foreach (var n in nodes.Skip(2))
            {
                if (n <= 127)
                {
                    yield return (byte)n;
                }
                else
                {
                    foreach (var b in n.ToVLQEncodedInt()) yield return b;
                }
            }
        }
    }
}