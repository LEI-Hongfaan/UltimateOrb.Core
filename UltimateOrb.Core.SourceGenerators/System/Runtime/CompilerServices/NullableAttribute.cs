using System;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices {

	[CompilerGenerated]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
	public sealed class NullableAttribute : Attribute {
		
		public NullableAttribute(byte arg) {
			this.NullableFlags = new byte[] {
				arg
			};
		}

		public NullableAttribute(byte[] arg) {
			this.NullableFlags = arg;
		}

		public readonly byte[] NullableFlags;
	}
}
