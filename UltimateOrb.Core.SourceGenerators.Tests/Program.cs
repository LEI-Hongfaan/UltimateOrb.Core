using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Core.SourceGenerators.Tests {

    class Program {

        static async Task<int> Main(string[] args) {
            new InterfaceExtensionsGeneratorTests().Test_InterfaceExtensionsGenerator();
            return 0;
        }
    }
}
