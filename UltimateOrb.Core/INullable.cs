using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;

namespace UltimateOrb {

    public interface INullable {

        bool HasValue {

            get;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        protected InvalidOperationException ThrowInvalidOperationException() {
            _ = default(int?)!.Value;
            throw null!;
        }
    }

    public partial interface INullable<T> : INullable {

        T Value {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => HasValue ? GetValueOrDefault() : throw ThrowInvalidOperationException();
        }

        T GetValueOrDefault();

        T GetValueOrDefault(T defaultValue) => HasValue ? GetValueOrDefault() : defaultValue;
    }
}
