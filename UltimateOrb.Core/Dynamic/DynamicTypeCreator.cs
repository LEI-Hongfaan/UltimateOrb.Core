using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace UltimateOrb.Dynamic {

    public class DynamicTypeCreator {

        [RequiresDynamicCode("")]
        public static Type CreateDynamicValueType() {
            var assemblyName = new AssemblyName("DynamicAssembly");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var modulusBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");

            var typeBuilder = modulusBuilder.DefineType("Dynamic", TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.SequentialLayout, typeof(ValueType));
            return typeBuilder.CreateType();
        }
    }
}
