using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    internal class Map
    {
        public String[,] map { get; } 
        public int N { get; set; }
        public int M { get; set; }

        public Map(int n, int m)
        {
            N = n;
            M = m;
            map = generateMap();
        }
        private String[,] generateMap()
        {

        }
    }
}
