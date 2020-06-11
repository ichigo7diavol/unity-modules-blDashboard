using System;
using System.Collections.Generic;
using System.Linq;

namespace blExternals.blReflection
{
    public static class Utilities
    {
        public static int GetHashCode(IEnumerable<Type> types)
            => string.Concat(types.Select(t => t.Name)).GetHashCode();
    }
}