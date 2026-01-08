using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Runtime.CompilerServices.TypeTokens;

namespace UltimateOrb {

    public interface ICloneable<TSelf>
        : ICloneable
        where TSelf : ICloneable<TSelf> {

        public new TSelf Clone();
    }

    interface ICloneableFriend<TSelf> : ICloneable
        where TSelf : ICloneable {

        public static object Clone(ref TSelf @this) => @this.Clone();

        public static object Clone(TSelf @this) => @this.Clone();
    }

    public interface ICloneableNongeneric_Base<TSelf> : ICloneable
        where TSelf : ICloneable {

        internal object Clone_Base() => this.Clone();
    }

    readonly ref struct ThisWrapper<T> {

        private readonly ref T @this;

        public ThisWrapper(ref T @this) {
            this.@this = ref @this;
        }
    }

    public interface ICloneableDerivedByNongeneric<TSelf>
        : ICloneable<TSelf>, ICloneableNongeneric_Base<TSelf>
        where TSelf : ICloneableDerivedByNongeneric<TSelf> {

        TSelf ICloneable<TSelf>.Clone() {
            return (TSelf)Clone_Base();
        }
    }

    public interface ICloneableDerivedByGeneric<TSelf>
        : ICloneable<TSelf>
        where TSelf : ICloneableDerivedByGeneric<TSelf> {

        object ICloneable.Clone() {
            return this.Clone();
        }
    }
}
