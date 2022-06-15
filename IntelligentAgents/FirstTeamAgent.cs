using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    internal class FirstTeamAgent : Agent
    {
        Random r;
        public string qualify { get; set; }
        public List<int[]> discoveredResources { get; set; }

        public FirstTeamAgent(int[] location, string qualify) : base(location)
        {
            this.qualify = qualify;
            discoveredResources = new List<int[]>();
            r= new Random();
        }
        public int[] movedToDiscoveredResources(Dictionary<String, Object> nearbyCells)
        {
            if (discoveredResources.Count == 0) return null;
            Dictionary<String, Object> nextLocation = moveTo(discoveredResources[0], nearbyCells);
            int[] newPosition = (int[])nextLocation["location"];
            currentX = newPosition[0];
            currentY = newPosition[1];
            if (discoveredResources[0] == getCurrentPosition())
            {
                carry = (string) nextLocation["item"];
            }
            return newPosition;
        }



        internal bool chechIfTheCellHasRecource(string mapCell)
        {
            return mapCell.Equals(this.qualify);

        }


        public int[] chooseTheCell(Dictionary<String,Object> dictionary)
        {
            List<String> keys = new List<string>(dictionary.Keys);
            Dictionary<String, Object> item = (Dictionary<string, object>) dictionary[keys[r.Next(0, keys.Count)]];
            return (int[]) item["location"];
        }

        public void findEnergyPots() { }
        public void buyMapFromAgent() { }
        public void buyEnergyPots() { }
        public void findGold() { }
        public void findResources() { }
    }
}
