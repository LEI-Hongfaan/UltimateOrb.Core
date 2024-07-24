using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Core.SourceGenerators.Tests {



    class Program {

        static async Task<int> Main(string[] args) {

            await new UoGenerator1Tests().Test();

            await new ReplaceOperatorTests().TestReplaceLessThanWithGreaterThan();
            new FixedDecimal32GeneratorTests().Test_FixedDecimal32Generator();
            return 0;
        }
    }
}
