using UltimateOrb.Typed_RefReturn_Wrapped_Huge.Collections.Generic;

namespace UltimateOrb.Plain.ValueTypes {

    public partial interface IStackEx<T> : IStack<T> {

        int Count { get; }

        long LongCount { get; }

        void IncreaseCapacity();

        ref T Push();

        void SetCapacity(int capacity);
    }
}
