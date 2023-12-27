using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Gas.Containers
{
    public class Connection : Transfer
    {
        public Container Destination { get; set; }
        //The destination of the transfer
        protected override Container Intersected()
        {
            return Destination;
        }
    }
}
