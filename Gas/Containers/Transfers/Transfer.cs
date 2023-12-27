using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Gas.Chemistry;
using Stride.Engine;
using Valve.VR;

namespace Gas.Containers.Transfers
{
    public abstract class Transfer : StartupScript
    {
        const int SimulationRate = 100;

        protected Container container;
        public float Area { get; set; }
        public override void Start()
        {
            base.Start();
            container = Entity.GetParent().Get<Container>();
            Task.Run(LeakLoop);
        }
        //simulate gas transfer between containers
        private async void LeakLoop()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (; ; )
            {
                TransferGas(stopwatch);
                await Task.Delay(SimulationRate);
            }
        }
        //Perform one step of gas simulation
        private void TransferGas(Stopwatch timer)
        {
            Container otherContainer = Intersected();
            Container container1 = container.Order < otherContainer.Order ? container : otherContainer;
            Container container2 = container.Order < otherContainer.Order ? otherContainer : container;
            lock (container1)
            {
                lock (container2)
                {
                    double delta = timer.ElapsedTicks / Stopwatch.Frequency;
                    timer.Restart();
                    Exchange(container1, container2, Area, delta);
                }
            }
        }
        //Transfer gas between containers. Locks must be acquired first to avoid race condition if multithreading
        private static void Exchange(Container container1, Container container2, float area, double delta)
        {
            Diffuse(container1, container2, area, delta);
            Decompress(container1, container2, area, delta);
        }
        //Diffusion of molecules
        private static void Diffuse(Container container1, Container container2, float area, double delta)
        {
            HashSet<Molecule> molecules = container1.Molecules;
            molecules.UnionWith(container2.Molecules);
            List<(Molecule, double)> changes = [];
            foreach (Molecule molecule in molecules)
            {
                double dp = container1.PartialPressure(molecule) - container2.PartialPressure(molecule);
                double diffuseRate = Math.Sqrt(dp / molecule.MolarMass) * area * .01;
                double diffuseAmount = diffuseRate * delta;
                if (container1.MoleCount(molecule) + diffuseAmount < 0)
                    diffuseAmount = -container1.MoleCount(molecule);
                if (container2.MoleCount(molecule) - diffuseAmount < 0)
                    diffuseAmount = container2.MoleCount(molecule);
                changes.Add((molecule, diffuseAmount));
            }
            container1.Add(changes);
            container2.Add(changes);
        }

        //Pressure equalization
        private static void Decompress(Container container1, Container container2, float area, double delta)
        {
            double dp = container1.Pressure - container2.Pressure;
            //Flow rate is assumed to be constant factor of hole area and square root of pressure difference.
            double flowRate = Math.Sqrt(dp) * area * 13;
            double flowAmount = flowRate * delta;
            static void TransferMoles(Container source, Container dest, double amount)
            {
                var counts = source.MolePortion(amount);
                source.Remove(counts);
                dest.Add(counts);
            }
            if (flowAmount > 0)
                TransferMoles(container1, container2, flowAmount);
            else
                TransferMoles(container2, container1, -flowAmount);
        }
        //Get container that we transfer between
        protected abstract Container Intersected();
    }
}
