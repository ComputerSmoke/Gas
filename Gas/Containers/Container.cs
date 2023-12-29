using Stride.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Gas.Chemistry;

namespace Gas.Containers
{
    public class IllegalValueException(string msg) : Exception(msg);
    public class Container : StartupScript
    {
        public double Volume { get; set; }
        public double Temperature { get; set; } = 293.15;
        readonly Dictionary<Molecule, double> moleCounts = [];
        //Container ordering used to prevent deadlock
        public long Order { get; set; }
        static long nextOrder = 0;
        public const double O = 1e-9;
        //Pressure in kPa
        public double Pressure
        {
            get
            {
                double total = 0;
                foreach (Molecule m in moleCounts.Keys)
                    total += PartialPressure(m);
                return total;
            }
        }
        //The molecules present in container
        public HashSet<Molecule> Molecules
        {
            get
            {
                return new (moleCounts.Keys.Where((mol, c) => c > 0));
            }
        }
        public override void Start()
        {
            base.Start();
            Order = nextOrder;
            nextOrder++;
        }
        public double MoleCount(Molecule molecule)
        {
            if(moleCounts.TryGetValue(molecule, out double count))
                return count;
            return 0;
        }
        public double PartialPressure(Molecule molecule)
        {
            return MoleCount(molecule) * 8.314 * Temperature / Volume;
        }
        public double TotalMoles()
        {
            double totalMol = 0;
            foreach (double c in moleCounts.Values)
                totalMol += c;
            return totalMol;
        }
        //Take n moles of the mixture in this container
        public List<(Molecule, double)> MolePortion(double n)
        {
            if (n < 0)
                throw new IllegalValueException("Cannot take negative portion of gas.");
            double total = TotalMoles();
            List<(Molecule, double)> res = [];
            if (total <= n)
            {
                foreach (var (molecule, count) in moleCounts)
                    res.Add((molecule, count));
                return res;
            }
            foreach (var (molecule, count) in moleCounts)
                res.Add((molecule, n*count/total));
            return res;
        }
        public void Add(List<(Molecule, double)> counts)
        {
            foreach(var (mol, count) in counts)
            {
                if(!moleCounts.ContainsKey(mol) && count < 0 || moleCounts.ContainsKey(mol) && moleCounts[mol] + count < 0)
                    throw new IllegalValueException("Not enough in moleCounts to remove this many molecules.");
                if (double.IsNaN(count))
                    throw new IllegalValueException("passed count is not a number.");
            }
            foreach(var (mol, count) in counts)
            {
                if (!moleCounts.ContainsKey(mol))
                    moleCounts[mol] = 0;
                moleCounts[mol] += count;
            }
        }
        public void Remove(List<(Molecule, double)> counts)
        {
            List<(Molecule, double)> negated = [];
            foreach (var (molecule, count) in counts)
                negated.Add((molecule, -count));
            Add(negated);
        }
    }
}
