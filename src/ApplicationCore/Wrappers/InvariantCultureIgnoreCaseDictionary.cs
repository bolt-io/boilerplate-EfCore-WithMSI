using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Wrappers
{
    public sealed class InvariantCultureIgnoreCaseDictionary<TValue> : Dictionary<string, TValue>
    {
        public InvariantCultureIgnoreCaseDictionary() : base(StringComparer.InvariantCultureIgnoreCase) { }
    }
}