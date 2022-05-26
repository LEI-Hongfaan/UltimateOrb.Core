using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {

    /// <summary>
    /// <para>
    /// Represents a Boolean (true or false) value. Internally, this type uses 8-bit representations.
    /// </para>
    /// <para>
    /// This type can handle non-canonical representations (values other than 0 or 1).<br />
    /// </para>
    /// </summary>
    /// <remarks>
    /// This type is blittable.<br />
    /// The size of a value of this type is the same as sizeof(byte).
    /// </remarks>
    public readonly partial struct Boolean8 {
    }

    /// <summary>
    /// <para>
    /// Represents a Boolean (true or false) value. Internally, this type uses an integer (<c><see cref="int" /></c>/<c><see cref="uint" /></c>).
    /// </para>
    /// <para>
    /// This type can handle non-canonical representations (values other than 0 or 1).<br />
    /// See <see cref="CanonicalIntegerBoolean" /> for a boolean type that handles canonical representations only but may have better performance.
    /// </para>
    /// </summary>
    /// <remarks>
    /// This type is blittable.<br />
    /// The size of a value of this type is the same as sizeof(int).
    /// </remarks>
    public readonly partial struct Boolean32 {
    }
}