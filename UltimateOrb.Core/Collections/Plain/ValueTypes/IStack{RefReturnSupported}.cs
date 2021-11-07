namespace UltimateOrb.Typed_RefReturn_Wrapped_Huge.Collections.Generic {
    using Generic = UltimateOrb.Collections.Generic;

    public partial interface IStack<T> : Generic.IStack<T> {

        new ref T Peek();
    }

    public partial interface IStack_A1<T> : Generic.IStack_A1<T>, IStack<T> {
    }

    public partial interface IStack_B1<T> : Generic.IStack_B1<T>, IStack<T> {
    }

    public partial interface IStack_2_A1_B1_1<T> : Generic.IStack_2_A1_B1_1<T>, IStack_A1<T>, IStack_B1<T> {
    }
}
