using System;

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

namespace UltimateOrb {

    [ForceDiscard]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
    sealed class GenerateInterfaceGenericExtensionsForAttribute : Attribute {

        public GenerateInterfaceGenericExtensionsForAttribute(Type tInterface) {
            this.InterfaceType = tInterface;
        }

        public Type InterfaceType {

            get;
        }
    }
}
