using NUnit.Framework;
using UltimateOrb.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

#if false
#if INDEPENDENT_XINTN_LIBRARY
#else
namespace UltimateOrb.Runtime.CompilerServices {
    using static InlineIL.IL;

#if INDEPENDENT_XINTN_LIBRARY
    internal
#else
    public
#endif
    static partial class Unsafe {

#pragma warning disable IDE0060 // Remove unused parameter
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T? UnboxNullable<T>(object? boxed) where T : struct {
            Emit.Ldarg(nameof(boxed)); // load the boxed object onto the stack
            Emit.Unbox(typeof(T?)); // unbox it to a controlled-mutability pointer of type T?
            Emit.Ret(); // return the value
            throw null!;
        }
    }
}
#endif

namespace UltimateOrb.Runtime.CompilerServices.Tests {




    [TestFixture()]
    public class UnsafeTests {






        [Test()]
        // [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void UnboxNullableTest() {
            var boxed = (object?)(int?)42;

            var ddddd = ValueTuple.Create(boxed);



            ref var v = ref Unsafe.UnboxNullable<int>(boxed);

            var sss = Unsafe.As<ValueTuple<object?>, IntPtr>(ref ddddd);

            Console.WriteLine(sss.ToString("X16"));
            unsafe {
                Console.WriteLine(new IntPtr(Unsafe.AsPointer(ref v)).ToString("X16"));
                Console.WriteLine(new IntPtr(Unsafe.AsPointer(ref ddddd)).ToString("X16"));
            }

            Console.WriteLine(Unsafe.Unbox<int>(boxed!));

            Console.WriteLine(v.Value);



            Assert.IsTrue(42 == v);

            Assert.AreEqual(42, v);
        }
    }
}
#endif
