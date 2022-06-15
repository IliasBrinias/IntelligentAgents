using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    internal class Resources : MapItem
    {
        enum ResourcesEnum{Wood, Iron, Gold, Cereals}
        public int type{ get; set; }
        public int quantity{ get; set; }
    }
}
