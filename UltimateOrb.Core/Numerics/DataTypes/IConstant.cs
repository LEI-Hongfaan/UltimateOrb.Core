using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics.DataTypes {

    [Experimental("UoWIP_GenericMath")]
    public interface IConstant<TSelf, out T> {

        public static abstract T Value { get; }
    }
}
