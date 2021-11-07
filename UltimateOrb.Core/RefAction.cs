using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {

    public delegate void RefAction<T>(ref T byref);

    public delegate void RefAction<T, TArg>(ref T byref, TArg arg);
}
