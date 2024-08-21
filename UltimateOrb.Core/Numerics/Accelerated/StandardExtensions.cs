
using System.Runtime.Intrinsics;

namespace UltimateOrb.Numerics.Accelerated {

    public static partial class StandardExtensions {

        internal static QuaternionD AsQuaternion(this Vector256<double> xyzw) {
            return new QuaternionD(xyzw);
        }

        internal static Vector4D AsVector4(this Vector256<double> xyzw) {
            return new Vector4D(xyzw);
        }

        internal static Vector3D AsVector3(this Vector256<double> xyz_) {
            return new Vector3D(xyz_.GetLower(), xyz_.GetElement(2));
        }

        internal static Vector4D AsVector4(this QuaternionD xyzw) {
            return xyzw.XYZW;
        }

        internal static QuaternionD AsQuaternion(this Vector4D xyzw) {
            return new QuaternionD(xyzw);
        }

        internal static Vector3D AsVector3(this Vector4D xyz_) {
            return AsVector3(xyz_.ToVector256());
        }
    }
}
