using Stride.Engine;
using Stride.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gas.Containers.Transfers
{
    public class Leak : Transfer
    {
        private PhysicsComponent trigger;
        public override void Start()
        {
            base.Start();
            trigger = Entity.Get<PhysicsComponent>();
        }
        //Get smallest container which intersects this leak (and is not source of leak)
        protected override Container Intersected()
        {
            Container minContainer = Universe.GlobalContainer;
            foreach (Collision collision in trigger.Collisions)
            {
                var other = collision.ColliderA == trigger ? collision.ColliderB : collision.ColliderA;
                var otherContainers = other.Entity.GetAll<Container>();
                foreach (Container otherContainer in otherContainers)
                {
                    if (otherContainer == container)
                        continue;
                    if (minContainer == null || minContainer.Volume > otherContainer.Volume)
                        minContainer = otherContainer;
                }
            }
            return minContainer;
        }
    }
}
