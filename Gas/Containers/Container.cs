using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Gas.Containers
{
    public enum Gas
    {
        Hydrogen,
        Nitrogen,
        Oxygen,
        CarbonDioxide,
        CarbonMonoxide
    }
    public class Container : StartupScript
    {
        public double Volume { get; set; }
        public double Temperature { get; set; } = 293.15;
        public readonly double[] moleCounts = new double[Enum.GetNames(typeof(Gas)).Length];
        //Container ordering used to prevent deadlock
        public long Order { get; set; }
        static long nextOrder = 0;
        public const double O = 1e-9;
        //Pressure in kPa
        public double Pressure
        {
            get
            {
                //P = nRT/V
                return TotalMoles() * 8.314 * Temperature / Volume;
            }
        }
        public override void Start()
        {
            base.Start();
            Order = nextOrder;
            nextOrder++;
        }
        public double TotalMoles()
        {
            double totalMol = 0;
            foreach (double c in moleCounts)
                totalMol += c;
            return totalMol;
        }
        //Take n moles of the mixture in this container
        public double[] MolePortion(double n)
        {
            double total = TotalMoles();
            double[] res = new double[moleCounts.Length];
            if (total <= n)
            {
                for(int i = 0; i < moleCounts.Length; i++)
                    res[i] = moleCounts[i];
                return res;
            }
            for(int i = 0; i <  moleCounts.Length; i++)
                res[i] = n*moleCounts[i]/total;
            return res;
        }
    }
}
