using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Threading {

    public struct SpinWait : IAction {

        public System.Threading.SpinWait Base;

        public int Count {

            get => Base.Count;
        }

        public bool NextSpinWillYield {

            get => Base.NextSpinWillYield;
        }

        public void Reset() => Base.Reset();

        public void SpinOnce() => Base.SpinOnce();

        public void SpinOnce(int sleep1Threshold) => Base.SpinOnce(sleep1Threshold);

        void IAction.Invoke() => SpinOnce();
    }
}
