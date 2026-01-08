using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

[assembly: CLSCompliantAttribute(true)]

[assembly: System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute("System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e")]
[assembly: System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute("System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e")]
[assembly: System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute("System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e")]
[assembly: System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute("System.Private.CoreLib, Version=9.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e")]

namespace UltimateOrb {

    [ForceDiscard]
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    sealed class ForceDiscardAttribute : Attribute {

        public ForceDiscardAttribute() {
        }
    }

    [DiscardableAfterILLink]
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    sealed class DiscardableAfterILLink : Attribute {

        public DiscardableAfterILLink() {
        }
    }
    
    [ForceDiscard]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, Inherited = false, AllowMultiple = false)]
    sealed class ILMethodBodyAttribute : Attribute {

        readonly string ilSourceCode;

        public ILMethodBodyAttribute(string ilSourceCode) {
            this.ilSourceCode = ilSourceCode;
        }

        public string ILSourceCode {

            get {
                return ilSourceCode;
            }
        }
    }
}
