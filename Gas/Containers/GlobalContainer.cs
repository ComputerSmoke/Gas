﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gas.Containers
{
    public class GlobalContainer : Container
    {
        public GlobalContainer() : base() 
        {
            Volume = 1e12;
        }
        public override void Start()
        {
            base.Start();
            Universe.GlobalContainer = this;
        }
    }
}
