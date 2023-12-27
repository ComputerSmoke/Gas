using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gas.Chemistry
{
    public static class MoleculeTable
    {
        public readonly static Molecule Hydrogen = new([(PeriodicTable.Hydrogen, 2)], "Hydrogen Gas");
        public readonly static Molecule Oxygen = new([(PeriodicTable.Oxygen, 2)], "Oxygen Gas");
        public readonly static Molecule Nitrogen = new([(PeriodicTable.Nitrogen, 2)], "Nitrogen Gas");
        public readonly static Molecule CarbonDioxide = new([(PeriodicTable.Carbon, 1), (PeriodicTable.Oxygen, 2)], "Carbon Dioxide");
        public readonly static Molecule CarbonMonoxide = new([(PeriodicTable.Carbon, 1), (PeriodicTable.Oxygen, 1)], "Carbon Monoxide");
    }
}
