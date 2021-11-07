using System;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices {

	[CompilerGenerated]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	public sealed class NullableContextAttribute : Attribute {
		
		public NullableContextAttribute(byte arg) {
			this.Flag = arg;
		}

		public readonly byte Flag;
	}
}
