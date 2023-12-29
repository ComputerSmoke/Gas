using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Particles;
using Stride.Particles.Components;

namespace Gas.Containers
{
    public class PressureVisualizer : StartupScript
    {
        Container container;
        ParticleSystemComponent particleSystem;
        public override void Start()
        {
            base.Start();
            container = Entity.Get<Container>();
            particleSystem = Entity.Get<ParticleSystemComponent>();
            UpdateParticleSystem();
        }
        private async void UpdateParticleSystem()
        {
            for(; ; )
            {
                var moles = container.TotalMoles();
                particleSystem.ParticleSystem.Emitters[0].MaxParticlesOverride = Math.Max(1, (int)moles);
                int colorModifier = (int)(1 - 1 / Math.Pow(2, container.Pressure)); 
                particleSystem.Color = new Color4(1-colorModifier, 0, colorModifier);
                await Task.Delay(100);
            }
        }
    }
}
