using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gas.Chemistry
{
    public class Element(double MolarMass, string symbol, int atomicNumber)
    {
        public readonly double MolarMass = MolarMass;
        public readonly string symbol = symbol;
        public readonly int atomicNumber = atomicNumber;
    }
}
