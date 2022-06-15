using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    internal class Agent
    {
        Random r;
        public string carry { get; set; }
        public String name { get; set; }

        public string qualify { get; set; }

        public Agent(string name, string qualify)
        {
            this.name = name;
            this.qualify = qualify;
            r = new Random();
        }

        public int move()
        {
            return r.Next(1, 2);
        }
        public void findEnergyPots() { }
        public void buyMapFromAgent() { }
        public void buyEnergyPots() { }
        public void findGold() { }
        public void findResources() { }
    }
}
