using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Internal {

    static class ThrowHelper {

        [DoesNotReturnAttribute()]
        public static NotSupportedException ThrowNotSupportedException_readonly() {
            throw new NotSupportedException("The value is read-only.");
        }
    }
}
