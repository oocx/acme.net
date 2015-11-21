using System;
using System.Collections.Generic;
using System.Linq;
using Oocx.Asn1PKCS.Asn1BaseTypes;

namespace Oocx.Asn1PKCS.Parser
{
    public class Asn1TagToClassMapper
    {
        private Dictionary<int, Type> map = new Dictionary<int, Type>();

        public Asn1TagToClassMapper()
        {
            //var asn1types =
                //typeof(IAsn1Element)..Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof (IAsn1Element)));


        }

        public Type GetType(int tag)
        {
            throw new NotImplementedException();
        }
    }
}