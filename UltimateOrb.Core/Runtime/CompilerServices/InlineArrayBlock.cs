using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;

namespace UltimateOrb.Runtime.CompilerServices {

    struct InlineArrayBlock1<T> {

        public T Value;
    }

    struct InlineArrayBlock2<T> {

        public InlineArrayBlock1<T> Lower;

        public InlineArrayBlock1<T> Upper;
    }

    struct InlineArrayBlock4<T> {

        public InlineArrayBlock2<T> Lower;

        public InlineArrayBlock2<T> Upper;
    }

    struct InlineArrayBlock8<T> {

        public InlineArrayBlock4<T> Lower;

        public InlineArrayBlock4<T> Upper;
    }

    struct InlineArrayBlock16<T> {

        public InlineArrayBlock8<T> Lower;

        public InlineArrayBlock8<T> Upper;
    }

    struct InlineArrayBlock32<T> {

        public InlineArrayBlock16<T> Lower;

        public InlineArrayBlock16<T> Upper;
    }

    struct InlineArrayBlock64<T> {

        public InlineArrayBlock32<T> Lower;

        public InlineArrayBlock32<T> Upper;
    }
}