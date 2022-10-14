using System;
using System.Collections.Generic;

namespace UltimateOrb {

	public partial interface IFunc<out TResult> {
		
		TResult Invoke();
	}
	
	public static partial class TFunc<TResult> {
		
		public static TResult Invoke<TFunc>(TFunc @this) where TFunc : IFunc<TResult> {
			return @this.Invoke();
		}
	}

	public partial interface IFunc<in T, out TResult> {
		
		TResult Invoke(T arg);
	}
	
	public static partial class TFunc<T, TResult> {
		
		public static TResult Invoke<TFunc>(TFunc @this, T arg) where TFunc : IFunc<T, TResult> {
			return @this.Invoke(arg);
		}
	}

	public partial interface IFunc<in T1, in T2, out TResult> {
		
		TResult Invoke(T1 arg1, T2 arg2);
	}
	
	public static partial class TFunc<T1, T2, TResult> {
		
		public static TResult Invoke<TFunc>(TFunc @this, T1 arg1, T2 arg2) where TFunc : IFunc<T1, T2, TResult> {
			return @this.Invoke(arg1, arg2);
		}
	}

	public partial interface IFunc<in T1, in T2, in T3, out TResult> {
		
		TResult Invoke(T1 arg1, T2 arg2, T3 arg3);
	}
	
	public static partial class TFunc<T1, T2, T3, TResult> {
		
		public static TResult Invoke<TFunc>(TFunc @this, T1 arg1, T2 arg2, T3 arg3) where TFunc : IFunc<T1, T2, T3, TResult> {
			return @this.Invoke(arg1, arg2, arg3);
		}
	}

	public partial interface IFunc<in T1, in T2, in T3, in T4, out TResult> {
		
		TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	}
	
	public static partial class TFunc<T1, T2, T3, T4, TResult> {
		
		public static TResult Invoke<TFunc>(TFunc @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TFunc : IFunc<T1, T2, T3, T4, TResult> {
			return @this.Invoke(arg1, arg2, arg3, arg4);
		}
	}

	public partial interface IFunc<in T1, in T2, in T3, in T4, in T5, out TResult> {
		
		TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
	}
	
	public static partial class TFunc<T1, T2, T3, T4, T5, TResult> {
		
		public static TResult Invoke<TFunc>(TFunc @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TFunc : IFunc<T1, T2, T3, T4, T5, TResult> {
			return @this.Invoke(arg1, arg2, arg3, arg4, arg5);
		}
	}

	public partial interface IFunc<in T1, in T2, in T3, in T4, in T5, in T6, out TResult> {
		
		TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	}
	
	public static partial class TFunc<T1, T2, T3, T4, T5, T6, TResult> {
		
		public static TResult Invoke<TFunc>(TFunc @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TFunc : IFunc<T1, T2, T3, T4, T5, T6, TResult> {
			return @this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
		}
	}

	public partial interface IFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, out TResult> {
		
		TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
	}
	
	public static partial class TFunc<T1, T2, T3, T4, T5, T6, T7, TResult> {
		
		public static TResult Invoke<TFunc>(TFunc @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) where TFunc : IFunc<T1, T2, T3, T4, T5, T6, T7, TResult> {
			return @this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
	}

	public partial interface IFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, out TResult> {
		
		TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
	}
	
	public static partial class TFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> {
		
		public static TResult Invoke<TFunc>(TFunc @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) where TFunc : IFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> {
			return @this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}
	}

	public partial interface IFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, out TResult> {
		
		TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
	}
	
	public static partial class TFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> {
		
		public static TResult Invoke<TFunc>(TFunc @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) where TFunc : IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> {
			return @this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
		}
	}

	public partial interface IFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, out TResult> {
		
		TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);
	}
	
	public static partial class TFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> {
		
		public static TResult Invoke<TFunc>(TFunc @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) where TFunc : IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> {
			return @this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
		}
	}

	public partial interface IFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, out TResult> {
		
		TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);
	}
	
	public static partial class TFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> {
		
		public static TResult Invoke<TFunc>(TFunc @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) where TFunc : IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> {
			return @this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
		}
	}

	public partial interface IFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, out TResult> {
		
		TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);
	}
	
	public static partial class TFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> {
		
		public static TResult Invoke<TFunc>(TFunc @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) where TFunc : IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> {
			return @this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
		}
	}

	public partial interface IFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, out TResult> {
		
		TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);
	}
	
	public static partial class TFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> {
		
		public static TResult Invoke<TFunc>(TFunc @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) where TFunc : IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> {
			return @this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
		}
	}

	public partial interface IAction {

		void Invoke();
	}
	
	public static partial class TAction {
		
		public static void Invoke<TAction>(TAction @this) where TAction : IAction {
			@this.Invoke();
		}
	}

	public partial interface IAction<in T> {

		void Invoke(T arg);
	}
	
	public static partial class TAction<T> {
		
		public static void Invoke<TAction>(TAction @this, T arg) where TAction : IAction<T> {
			@this.Invoke(arg);
		}
	}

	public partial interface IAction<in T1, in T2> {

		void Invoke(T1 arg1, T2 arg2);
	}
	
	public static partial class TAction<T1, T2> {
		
		public static void Invoke<TAction>(TAction @this, T1 arg1, T2 arg2) where TAction : IAction<T1, T2> {
			@this.Invoke(arg1, arg2);
		}
	}

	public partial interface IAction<in T1, in T2, in T3> {

		void Invoke(T1 arg1, T2 arg2, T3 arg3);
	}
	
	public static partial class TAction<T1, T2, T3> {
		
		public static void Invoke<TAction>(TAction @this, T1 arg1, T2 arg2, T3 arg3) where TAction : IAction<T1, T2, T3> {
			@this.Invoke(arg1, arg2, arg3);
		}
	}

	public partial interface IAction<in T1, in T2, in T3, in T4> {

		void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	}
	
	public static partial class TAction<T1, T2, T3, T4> {
		
		public static void Invoke<TAction>(TAction @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TAction : IAction<T1, T2, T3, T4> {
			@this.Invoke(arg1, arg2, arg3, arg4);
		}
	}

	public partial interface IAction<in T1, in T2, in T3, in T4, in T5> {

		void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
	}
	
	public static partial class TAction<T1, T2, T3, T4, T5> {
		
		public static void Invoke<TAction>(TAction @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TAction : IAction<T1, T2, T3, T4, T5> {
			@this.Invoke(arg1, arg2, arg3, arg4, arg5);
		}
	}

	public partial interface IAction<in T1, in T2, in T3, in T4, in T5, in T6> {

		void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	}
	
	public static partial class TAction<T1, T2, T3, T4, T5, T6> {
		
		public static void Invoke<TAction>(TAction @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TAction : IAction<T1, T2, T3, T4, T5, T6> {
			@this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
		}
	}

	public partial interface IAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7> {

		void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
	}
	
	public static partial class TAction<T1, T2, T3, T4, T5, T6, T7> {
		
		public static void Invoke<TAction>(TAction @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) where TAction : IAction<T1, T2, T3, T4, T5, T6, T7> {
			@this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
	}

	public partial interface IAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8> {

		void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
	}
	
	public static partial class TAction<T1, T2, T3, T4, T5, T6, T7, T8> {
		
		public static void Invoke<TAction>(TAction @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) where TAction : IAction<T1, T2, T3, T4, T5, T6, T7, T8> {
			@this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}
	}

	public partial interface IAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9> {

		void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
	}
	
	public static partial class TAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> {
		
		public static void Invoke<TAction>(TAction @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) where TAction : IAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> {
			@this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
		}
	}

	public partial interface IAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10> {

		void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);
	}
	
	public static partial class TAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> {
		
		public static void Invoke<TAction>(TAction @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) where TAction : IAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> {
			@this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
		}
	}

	public partial interface IAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11> {

		void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);
	}
	
	public static partial class TAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> {
		
		public static void Invoke<TAction>(TAction @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) where TAction : IAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> {
			@this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
		}
	}

	public partial interface IAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12> {

		void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);
	}
	
	public static partial class TAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> {
		
		public static void Invoke<TAction>(TAction @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) where TAction : IAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> {
			@this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
		}
	}

	public partial interface IAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13> {

		void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);
	}
	
	public static partial class TAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> {
		
		public static void Invoke<TAction>(TAction @this, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) where TAction : IAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> {
			@this.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
		}
	}
}
