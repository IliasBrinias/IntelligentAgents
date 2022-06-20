using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    internal class FirstTeamAgent : Agent
    {
        private readonly int LOW_ENERGY = 25;
        private int[] villageLoc;
        Random r;
        public string qualify { get; set; }
        public int discoveredAreasCount { get; set; }

        public List<int[]> discoveredResources { get; set; }
        public List<int[]> discoveredEnergy { get; set; }
        public Dictionary<int,Object> discoveredAreas;
        public FirstTeamAgent(int[] location, string qualify) : base(location)
        {
            villageLoc = location;
            discoveredAreasCount = 0;
            discoveredAreas = new Dictionary<int, Object>();
            Dictionary<int,Boolean> discoveredAreasMap = new Dictionary<int,Boolean>();
            discoveredAreasMap.Add(getCurrentPosition()[1], true);
            discoveredAreas.Add(getCurrentPosition()[0], discoveredAreasMap);
            this.qualify = qualify;
            discoveredResources = new List<int[]>();
            discoveredEnergy = new List<int[]>();
            r = new Random();
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
                inventory.Add((string)nextLocation["item"]);
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

        public int[] chooseTheCellBasedOn(Dictionary<String,Object> nearbyCells, String flag)
        {
            List<String> keys = new List<string>(nearbyCells.Keys);
            List<int[]> undescoveredAreas = new List<int[]>();
            List<int[]> areasWithResourcesToSave = new List<int[]>();

            // check if some of nearby cells are undescovered
            for ( int i = 0; i < keys.Count; i++)
            {
                int[] location = (int[])((Dictionary<String, Object>)nearbyCells[keys[i]])["location"];
                if (!checkIfTheLocationExist(location))
                {
                    if (((String)((Dictionary<String, Object>)nearbyCells[keys[i]])["item"]).Equals(Constants.ENERGY_POTS))
                    {
                        saveLocationOfEnergy(location);
                    }

                    Console.WriteLine("Undescover: "+keys[i]);
                    if (((String)((Dictionary<String, Object>)nearbyCells[keys[i]])["item"]).Equals(flag))
                    {
                        areasWithResourcesToSave.Add(location);
                    }
                    undescoveredAreas.Add(location);
                }
            }
            
            // pick one of the underscovered Areas to move On
            if(undescoveredAreas.Count > 0)
            {
                if (areasWithResourcesToSave.Count > 0)
                {
                    int idx = r.Next(0, areasWithResourcesToSave.Count);
                    int[] pickedLocation = areasWithResourcesToSave[idx];
                    areasWithResourcesToSave.RemoveAt(idx);
                    foreach (int[] l in areasWithResourcesToSave)
                    {
                        discoveredResources.Add(l);
                    }
                    return saveLocationAsDiscovered(pickedLocation);
                }
                else
                {
                    int idx = r.Next(0, undescoveredAreas.Count);
                    return saveLocationAsDiscovered(undescoveredAreas[idx]);
                }
            }
            else
            {
                Dictionary<String, Object> item;
                // if the nearby location were discovered try to move to the enemy village
                item = moveToTheEnemyVillage(nearbyCells);
                if(item!=null) return saveLocationAsDiscovered((int[])item["location"]);

                // if all the areas are descovered pick a random one
                int idx = r.Next(0, keys.Count);
                Console.WriteLine("Picked: " + keys[idx]);
                item = (Dictionary<string, object>)nearbyCells[keys[idx]];
                return saveLocationAsDiscovered((int[]) item["location"]);
            }
        }

        private Dictionary<String, Object>? moveToTheEnemyVillage(Dictionary<String, Object> nearbyCells)
        {
            Dictionary<String, Object> item;

            if (villageLoc[0] == 0 && villageLoc[1] == 0)
            {
                if (nearbyCells.ContainsKey("right") || nearbyCells.ContainsKey("down"))
                {
                    if (r.Next(0, 2) == 0)
                    {
                        try
                        {
                            item = (Dictionary<string, object>)nearbyCells["right"];
                        }
                        catch
                        {
                            item = (Dictionary<string, object>)nearbyCells["down"];
                        }
                    }
                    else
                    {
                        try
                        {
                            item = (Dictionary<string, object>)nearbyCells["down"];
                        }
                        catch
                        {
                            item = (Dictionary<string, object>)nearbyCells["right"];
                        }
                    }
                    return item;
                }
            }
            else
            {
                if (nearbyCells.ContainsKey("left") || nearbyCells.ContainsKey("up"))
                {
                    if (r.Next(0, 2) == 0)
                    {
                        try
                        {
                            item = (Dictionary<string, object>)nearbyCells["left"];
                        }
                        catch
                        {
                            item = (Dictionary<string, object>)nearbyCells["up"];
                        }
                    }
                    else
                    {
                        try
                        {
                            item = (Dictionary<string, object>)nearbyCells["up"];
                        }
                        catch
                        {
                            item = (Dictionary<string, object>)nearbyCells["left"];
                        }

                    }
                    return item;
                }
            }
            return null;
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
                    discoveredAreasCount++;
                }

            }
            else
            {
                l = new Dictionary<int, bool>();
                discoveredAreas.Add(location[0], l);
                discoveredAreasCount++;

            }
            Console.WriteLine("Discovered Areas ("+discoveredAreasCount+")");
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
        internal bool hasLowEnergy()
        {
            return energyPoint < LOW_ENERGY;
        }

        internal bool ifHasPosionUseIt()
        {
            for(int i=0; i < inventory.Count; i++)
            {
                if (inventory[i].Equals(Constants.ENERGY_POTS))
                {
                    Console.WriteLine("Posion Used");
                    increaseEnergy();
                    inventory.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        internal bool hasResourcesInInventory()
        {
            foreach(String s in inventory)
            {
                if (!s.Equals(Constants.ENERGY_POTS))
                {
                    return true;
                }
            }
            return false;
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

        internal void checkIfAnotherAgentHasSamePosition(List<FirstTeamAgent> aFirstVillage, List<FirstTeamAgent> aSecondVillage, int[] firstVillageLoc)
        {
            // check if the agent is on first Village
            if (firstVillageLoc[0] == villageLoc[0] && firstVillageLoc[1] == villageLoc[1])
            {
                sameVillage(aFirstVillage);
                enemyVillage(aSecondVillage);
            }
            else
            {
                sameVillage(aSecondVillage);
                enemyVillage(aFirstVillage);

            }

        }
        private void enemyVillage(List<FirstTeamAgent> aEnemyVillage)
        {
            List<FirstTeamAgent> sameLocAgentEnemyVill = new List<FirstTeamAgent>();
            foreach (FirstTeamAgent a in aEnemyVillage)
            {
                if (a.currentX == currentX && a.currentY == currentY)
                {
                    sameLocAgentEnemyVill.Add(a);
                }
            }

        }
        private void sameVillage(List<FirstTeamAgent> aSameVillage)
        {
            List<int> sameLocAgentSameVill = new List<int>();
            for (int i=0; i< aSameVillage.Count; i++)
            {
                if (aSameVillage[i].currentX == currentX && aSameVillage[i].currentY == currentY)
                {
                    sameLocAgentSameVill.Add(i);
                }
            }
            if (sameLocAgentSameVill.Count == 0) return;
            // if has agents 
            foreach(int i in sameLocAgentSameVill)
            {
                // add the agents discover map to current agent

            }

        }

        internal void addToInventory(string v)
        {
            inventory.Add(v);
        }

        internal bool hasDiscoveredResources()
        {
            return this.discoveredResources.Count > 0;
        }
        internal bool hasDiscoveredEnergy()
        {
            return this.discoveredEnergy.Count > 0;
        }

        internal int[] getTheLocationOfResource()
        {
            Console.WriteLine("__");
            Console.WriteLine("Discovered Resources");
            StringBuilder stringBuilder = new StringBuilder();
            foreach (int[] location in this.discoveredResources)
            {
                stringBuilder.Append(" (" + location[0] + "," + location[1]+") ");
            }
            Console.WriteLine(stringBuilder);
            Console.WriteLine("__");
            return this.discoveredResources[0];
        }
        internal int[] getTheLocationOfDiscoveredEnergy()
        {
            Console.WriteLine("__");
            Console.WriteLine("Discovered Energy");
            StringBuilder stringBuilder = new StringBuilder();
            foreach (int[] location in this.discoveredEnergy)
            {
                stringBuilder.Append(" (" + location[0] + "," + location[1]+") ");
            }
            Console.WriteLine(stringBuilder);
            Console.WriteLine("__");
            return this.discoveredEnergy[0];
        }

        internal void saveLocationOfEnergy(int[] location)
        {
            for(int i = 0; i < discoveredEnergy.Count; i++)
            {
                if (discoveredEnergy[i][0] == location[0] && discoveredEnergy[i][1] == location[1]) return;
            }
            discoveredEnergy.Add(location);
        }
        internal List<int[]> removeResourceFromDiscoveredResources(int[] location )
        {
            for(int i = 0; i < discoveredResources.Count; i++)
            {
                if (discoveredResources[i][0] == location[0] && discoveredResources[i][1] == location[1])
                {

                    Console.WriteLine("Discover Deleted At (" + location[0] + "," + location[1] + ")");
                    discoveredResources.RemoveAt(i);
                    break;
                }
            }
            return discoveredResources;
        }
        public void removeEnergyFromInventory(int[] l)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].Equals(Constants.ENERGY_POTS))
                {
                    inventory.RemoveAt(i);
                    return;
                }
            }
        }

        public List<int[]> removeDiscoveredEnergy(int[] l)
        {
            Console.WriteLine(discoveredEnergy.Count);
            for (int i = 0; i < discoveredEnergy.Count; i++)
            {
                if (discoveredEnergy[i][0] == l[0] && discoveredEnergy[i][1] == l[1])
                {
                    discoveredEnergy.RemoveAt(i);
                    break;
                }
            }
            return discoveredEnergy;

        }


    }
}
