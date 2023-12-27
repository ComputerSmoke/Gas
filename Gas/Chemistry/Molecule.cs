using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Gas.Chemistry
{
    public class Molecule
    {
        public readonly string Name;
        public readonly double MolarMass;
        public readonly (Element, int)[] structure;
        public Molecule((Element, int)[] structure, string name)
        {
            Name = name;
            foreach(var (element, count) in structure)
            {
                MolarMass += element.MolarMass * count;
            }
            this.structure = structure;
        }
    }
}
