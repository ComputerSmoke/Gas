using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gas.Chemistry;
using Stride.Engine;

namespace Gas.Containers
{
    public class Pressurizer : StartupScript
    {
        public Container container;
        public override void Start()
        {
            container.Add([(MoleculeTable.Oxygen, 100)]);
        }
    }
}
