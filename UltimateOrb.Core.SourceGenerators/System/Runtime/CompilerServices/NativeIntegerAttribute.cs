using System;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices {
	
	[CompilerGenerated]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
	public sealed class NativeIntegerAttribute : Attribute {
		
		public NativeIntegerAttribute() {
			this.TransformFlags = new bool[] {
				true
			};
		}

		public NativeIntegerAttribute(bool[] arg) {
			this.TransformFlags = arg;
		}

		public readonly bool[] TransformFlags;
	}
}
