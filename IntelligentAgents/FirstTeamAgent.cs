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
        public Dictionary<int,Object> discoveredAreas;
        public FirstTeamAgent(int[] location, string qualify) : base(location)
        {
            discoveredAreas = new Dictionary<int, Object>();
            Dictionary<int,Boolean> discoveredAreasMap = new Dictionary<int,Boolean>();
            discoveredAreasMap.Add(getCurrentPosition()[1], true);
            discoveredAreas.Add(getCurrentPosition()[0], discoveredAreasMap);
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
        public void addToDiscoveredList(int[] location)
        {
            foreach (int[] l in discoveredResources)
            {
                if (l[0] == location[0] && l[1] == location[1]) return;
            }
            discoveredResources.Add(location);
        }

        public int[] chooseTheCell(Dictionary<String,Object> nearbyCells)
        {
            List<String> keys = new List<string>(nearbyCells.Keys);
            List<int[]> undescoveredAreas = new List<int[]>();
            // check if some of nearby cells are undescovered
            for( int i = 0; i < keys.Count; i++)
            {
                int[] location = (int[])((Dictionary<String, Object>)nearbyCells[keys[i]])["location"];
                if (!checkIfTheLocationExist(location))
                {
                    Console.WriteLine("Undescover: "+keys[i]);
                    if (((String)((Dictionary<String, Object>)nearbyCells[keys[i]])["item"]).Equals(this.qualify))
                    {
                        Console.WriteLine("find the best Option Based on Agent Qualify: (" + location[0]+","+ location[1] + "):"+ this.qualify);
                        return saveLocationAsDiscovered(location);
                    }
                    undescoveredAreas.Add(location);
                    
                }
            }
            
            // pick one of the underscovered Areas to move On
            if(undescoveredAreas.Count > 0)
            {
                int idx = r.Next(0, undescoveredAreas.Count);
                Console.WriteLine("Picked: " + keys[idx]);
                return saveLocationAsDiscovered(undescoveredAreas[idx]);
            }
            else
            {
                // if all the areas are descovered pick a random one
                int idx = r.Next(0, keys.Count);
                Console.WriteLine("Picked: " + keys[idx]);
                Dictionary<String, Object> item = (Dictionary<string, object>)nearbyCells[keys[idx]];
                return saveLocationAsDiscovered((int[]) item["location"]);
            }
        }
        private int[] saveLocationAsDiscovered(int[] location)
        {
            Dictionary<int, Boolean> l = new Dictionary<int, bool>();
            if (discoveredAreas.ContainsKey(location[0]))
            {
                l = (Dictionary<int, Boolean>)discoveredAreas[location[0]];
                if (!l.ContainsKey(location[1]))
                {
                    l.Add(location[1], true);
                    discoveredAreas[location[0]] = l;
                }

            }
            else
            {
                l = new Dictionary<int, bool>();
                discoveredAreas.Add(location[0], l);
            }
            Console.WriteLine("Discovered Areas");
            List<int> keys = new List<int>(discoveredAreas.Keys);
            
            foreach(int k in keys)
            {
                StringBuilder stringBuilder = new StringBuilder();
                List<int> secondKeys = new List<int>(((Dictionary<int, Boolean>)discoveredAreas[k]).Keys);
                foreach(int secondK in secondKeys)
                {
                    stringBuilder.Append("(" + k + "," + secondK + ")");
                }
                Console.WriteLine(stringBuilder);

            }

            return location;
        }
        private Boolean checkIfTheLocationExist(int[] location)
        {
            
            if (discoveredAreas.ContainsKey(location[0]))
            {
                try
                {
                    return ((Dictionary<int, Boolean>)discoveredAreas[location[0]])[location[1]] == true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        public void findEnergyPots() { }
        public void buyMapFromAgent() { }
        public void buyEnergyPots() { }
        public void findGold() { }
        public void findResources() { }

        internal bool hasDiscoveredResources()
        {
            return this.discoveredResources.Count > 0;
        }

        internal int[] getTheLocationOfResource()
        {
            return this.discoveredResources[0];
        }
    }
}
