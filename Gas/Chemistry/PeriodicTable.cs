using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gas.Chemistry
{
    static class PeriodicTable
    {
        public static Element Hydrogen = new (1.008, "H", 1);
        public static Element Nitrogen = new(14.007, "N", 7);
        public static Element Oxygen = new(15.999, "O", 8);
        public static Element Carbon = new(12.011, "C", 6);
    }
}
