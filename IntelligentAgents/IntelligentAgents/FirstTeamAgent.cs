using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace IntelligentAgents
{
    internal class FirstTeamAgent : Agent
    {
        private readonly int LOW_ENERGY = 25;
        private int[] villageLoc;
        Random r;
        public int mapValue { get; set; }
        public int potValue { get; set; }
        public string qualify { get; set; }
        public int discoveredAreasCount { get; set; }

        public List<int[]> discoveredResources { get; set; }
        public List<int[]> discoveredEnergy { get; set; }
        public Dictionary<int, Object> discoveredAreas;
        public FirstTeamAgent(String name, int[] location, int M, int X, int Y, string qualify) : base(name, location)
        {
            mapValue = Y;
            potValue = X;
            LOW_ENERGY = (int)M / 4;
            energyPoint = 50;
            energyPotMultiplier = LOW_ENERGY;
            villageLoc = location;
            discoveredAreasCount = 0;
            discoveredAreas = new Dictionary<int, Object>();
            Dictionary<int, Boolean> discoveredAreasMap = new Dictionary<int, Boolean>();
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

        public int[] chooseTheCellBasedOn(Dictionary<String, Object> nearbyCells, String flag)
        {
            List<String> keys = new List<string>(nearbyCells.Keys);
            List<int[]> undescoveredAreas = new List<int[]>();
            List<int[]> areasWithResourcesToSave = new List<int[]>();

            // check if some of nearby cells are undescovered
            for (int i = 0; i < keys.Count; i++)
            {
                int[] location = (int[])((Dictionary<String, Object>)nearbyCells[keys[i]])["location"];
                if (!checkIfTheLocationExist(location))
                {
                    if (((String)((Dictionary<String, Object>)nearbyCells[keys[i]])["item"]).Equals(Constants.ENERGY_POTS))
                    {
                        saveLocationOfEnergy(location);
                    }

                    Console.WriteLine("Undescover: " + keys[i]);
                    if (((String)((Dictionary<String, Object>)nearbyCells[keys[i]])["item"]).Equals(flag))
                    {
                        areasWithResourcesToSave.Add(location);
                    }
                    undescoveredAreas.Add(location);
                }
            }

            // pick one of the underscovered Areas to move On
            if (undescoveredAreas.Count > 0)
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
                if (item != null) return saveLocationAsDiscovered((int[])item["location"]);

                // if all the areas are descovered pick a random one
                int idx = r.Next(0, keys.Count);
                Console.WriteLine("Picked: " + keys[idx]);
                item = (Dictionary<string, object>)nearbyCells[keys[idx]];
                return saveLocationAsDiscovered((int[])item["location"]);
            }
        }

        private Dictionary<String, Object> moveToTheEnemyVillage(Dictionary<String, Object> nearbyCells)
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
            Console.WriteLine("Discovered Areas (" + discoveredAreasCount + ")");
            List<int> keys = new List<int>(discoveredAreas.Keys);

            foreach (int k in keys)
            {
                StringBuilder stringBuilder = new StringBuilder();
                List<int> secondKeys = new List<int>(((Dictionary<int, Boolean>)discoveredAreas[k]).Keys);
                foreach (int secondK in secondKeys)
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
            for (int i = 0; i < inventory.Count; i++)
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
            foreach (String s in inventory)
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
            List<int> sameLocAgentEnemyVill = new List<int>();
            for (int i = 0; i < aEnemyVillage.Count; i++)
            {
                if (!aEnemyVillage[i].IsAlive) continue;
                if (aEnemyVillage[i].currentX == currentX && aEnemyVillage[i].currentY == currentY)
                {
                    sameLocAgentEnemyVill.Add(i);
                }
            }
            if (sameLocAgentEnemyVill.Count == 0) return;

            int resourcesForTrading = countResourcesForTrade();
            history.Add("(" + currentX + "," + currentY + ")" + " agent found another enemy agent");
            // if he hasnt resources to trade dont request
            if (resourcesForTrading < mapValue && resourcesForTrading < potValue) return;

            foreach (int idx in sameLocAgentEnemyVill)
            {
                // for each enemy agent

                switch (r.Next())
                {
                    case 0:
                        if (resourcesForTrading >= mapValue)
                        {
                            Console.WriteLine(name + " request map from " + aEnemyVillage[idx].name);
                            requestMapFromEnemyAgent(aEnemyVillage[idx]);
                        }
                        break;
                    case 1:
                        if (resourcesForTrading >= potValue)
                        {
                            Console.WriteLine(name + " request pot from " + aEnemyVillage[idx].name);

                            requestPotFromEnemyAgent(aEnemyVillage[idx]);
                        }
                        break;
                }
            }

        }

        private void requestPotFromEnemyAgent(FirstTeamAgent enemyAgent)
        {
            if (enemyAgent.responseForPot())
            {
                Console.WriteLine(name + " pot received");

                transferPotsTo(enemyAgent, potValue);
                // get one Pot
                inventory.Add(enemyAgent.getPot());
                history.Add("(" + currentX + "," + currentY + ")" + " agent pay and get enemy pot");
                return;
            }
            history.Add("(" + currentX + "," + currentY + ")" + " enemy decline the request for the pot");
        }

        private void transferPotsTo(FirstTeamAgent enemyAgent, int count)
        {
            // collect resources
            List<String> resources = new List<String>();
            List<int> idx = new List<int>();
            for (int i = 0; i < inventory.Count; i++)
            {
                if (!inventory[i].Equals(Constants.ENERGY_POTS)) continue;
                idx.Add(i);
                resources.Add(inventory[i]);
                if (resources.Count == count) break;
            }
            // remove them
            foreach (String r in resources)
            {
                inventory.Remove(r);
            }
            // pay
            for (int i = 0; i < idx.Count; i++)
            {
                enemyAgent.inventory.Add(resources[i]);
            }
        }

        private void requestMapFromEnemyAgent(FirstTeamAgent enemyAgent)
        {
            if (enemyAgent.responseForMap())
            {
                Console.WriteLine(enemyAgent.name + " accepted");
                transferResourcesTo(enemyAgent, mapValue);
                // get Compine Map
                discoveredAreas = compineData(enemyAgent.discoveredAreas, this.discoveredAreas);
                Console.WriteLine(name + " map is updated");
                history.Add("(" + currentX + "," + currentY + ")" + " agent pay and get enemy map");

                return;
            }
            Console.WriteLine(enemyAgent.name + " declined");
            history.Add("(" + currentX + "," + currentY + ")" + " enemy decline the request for the map");


        }

        private void transferResourcesTo(FirstTeamAgent enemyAgent, int count)
        {
            // collect resources
            List<String> resources = new List<String>();
            List<int> idx = new List<int>();
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].Equals(Constants.ENERGY_POTS)) continue;
                idx.Add(i);
                resources.Add(inventory[i]);
                if (resources.Count == count) break;
            }
            // remove them
            foreach (String r in resources)
            {
                inventory.Remove(r);
            }
            // pay
            for (int i = 0; i < idx.Count; i++)
            {
                enemyAgent.inventory.Add(resources[i]);
            }

        }

        private bool responseForPot()
        {
            if (getPosionCount() > 0)
            {
                return r.Next() % 100 < 50;
            }
            return false;
        }

        private string getPot()
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].Equals(Constants.ENERGY_POTS))
                {
                    inventory.RemoveAt(i);
                    return Constants.ENERGY_POTS;
                }
            }
            return null;
        }

        private int countResourcesForTrade()
        {
            int count = 0;
            foreach (String item in inventory)
            {
                if (item != Constants.ENERGY_POTS) { count++; }
            }
            return count;
        }

        private bool responseForMap()
        {
            return r.Next() % 100 < 50;
        }

        private void sameVillage(List<FirstTeamAgent> aSameVillage)
        {
            List<int> sameLocAgentSameVill = new List<int>();
            for (int i = 0; i < aSameVillage.Count; i++)
            {
                if (!aSameVillage[i].IsAlive) continue;
                if (aSameVillage[i].currentX == currentX && aSameVillage[i].currentY == currentY)
                {
                    sameLocAgentSameVill.Add(i);
                }
            }
            if (sameLocAgentSameVill.Count == 0) return;
            // if has agents 
            foreach (int idx in sameLocAgentSameVill)
            {
                if (aSameVillage[idx].name.Equals(name)) continue;
                Console.WriteLine("Exchange Knoledge between " + name + " - " + aSameVillage[idx].name);
                history.Add("(" + currentX + "," + currentY + ")" + " found playmate " + aSameVillage[idx].name + " and exchange discivered Areas, Energy and pot");

                // update their discover Areas
                Dictionary<int, Object> data = compineData(discoveredAreas, aSameVillage[idx].discoveredAreas);
                discoveredAreas = data;
                aSameVillage[idx].discoveredAreas = data;
                Console.WriteLine("Discovered Area compinded");

                if (aSameVillage[idx].qualify.Equals(qualify))
                {
                    // compine discover Resources
                    List<int[]> compineListResources = compineLists(discoveredResources, aSameVillage[idx].discoveredResources);
                    discoveredResources = compineListResources;
                    aSameVillage[idx].discoveredResources = compineListResources;
                    Console.WriteLine("List Resources compinded");
                }

                // compine discover Energy
                List<int[]> compineListEnergy = compineLists(discoveredEnergy, aSameVillage[idx].discoveredEnergy);
                if (discoveredEnergy.Count > 0)
                {
                    int x = 8;
                }
                discoveredEnergy = compineListEnergy;
                aSameVillage[idx].discoveredEnergy = compineListEnergy;
                Console.WriteLine("List Energy compinded");

                // give or get a posion if he need it
                exchangePosions(this, aSameVillage[idx]);
            }
        }

        public static List<int[]> compineLists(List<int[]> list, List<int[]> list1)
        {
            List<int[]> result = new List<int[]>();
            result.AddRange(list);
            foreach (int[] l1 in list1)
            {
                bool exists = false;
                foreach (int[] l in list)
                {
                    if (l[0] == l1[0] && l[1] == l1[1])
                    {
                        exists = true;
                    }
                }
                if (!exists)
                {
                    result.Add(l1);
                }
                exists = false;
            }
            return result;
        }

        private void exchangePosions(FirstTeamAgent currentAgent, FirstTeamAgent playmate)
        {
            if (currentAgent.getPosionCount() > 1 && playmate.getPosionCount() == 0)
            {
                currentAgent.givePosionToPlaymate(playmate);
            }
            else if (playmate.getPosionCount() > 1 && currentAgent.getPosionCount() == 0)
            {
                playmate.givePosionToPlaymate(currentAgent);
            }
        }

        private void givePosionToPlaymate(FirstTeamAgent playmate)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].Equals(Constants.ENERGY_POTS))
                {
                    inventory.RemoveAt(i);
                    break;
                }
            }
            playmate.inventory.Add(Constants.ENERGY_POTS);
            Console.WriteLine("Exchange Posions");
        }

        private int getPosionCount()
        {
            int countPosions = 0;
            foreach (String item in inventory)
            {
                if (item == Constants.ENERGY_POTS)
                {
                    countPosions++;
                }
            }
            return countPosions;
        }
        private Dictionary<int, Object> compineData(Dictionary<int, Object> data, Dictionary<int, Object> data1)
        {
            Dictionary<int, Object> compineData = new Dictionary<int, Object>();
            List<int> keys = new List<int>(data.Keys);
            List<int> keys1 = new List<int>(data1.Keys);

            // get all the data from the first dic
            foreach (int k in keys)
            {
                Dictionary<int, Boolean> compineDataKey = new Dictionary<int, Boolean>();
                Dictionary<int, Boolean> keyData = (Dictionary<int, Boolean>)data[k];
                if (data1.ContainsKey(k))
                {
                    // if the key of first dic exist on the sec disc merge the values
                    Dictionary<int, Boolean> key1Data = (Dictionary<int, Boolean>)data1[k];
                    compineDataKey = MergeDictionaries(keyData, key1Data);
                    compineData.Add(k, compineDataKey);
                }
                else
                {
                    // if the key doesnt exist on sec dic add to compine data the first dic data
                    compineData.Add(k, keyData);

                }
            }
            // remove all the keys of the sec dic keys to track if the sec dic has unique data
            foreach (int k in keys)
            {
                keys1.Remove(k);
            }
            // add the unuque data to the compine dic
            foreach (int k in keys1)
            {
                Dictionary<int, Boolean> keyData = (Dictionary<int, Boolean>)data1[k];
                compineData.Add(k, keyData);
            }

            return compineData;
        }
        private Dictionary<int, Boolean> MergeDictionaries(Dictionary<int, Boolean> me, Dictionary<int, Boolean> merge)
        {
            foreach (var item in merge.ToArray())
            {
                me[item.Key] = item.Value;
            }
            return me;
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
                stringBuilder.Append(" (" + location[0] + "," + location[1] + ") ");
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
                stringBuilder.Append(" (" + location[0] + "," + location[1] + ") ");
            }
            Console.WriteLine(stringBuilder);
            Console.WriteLine("__");
            return this.discoveredEnergy[0];
        }

        internal void saveLocationOfEnergy(int[] location)
        {
            for (int i = 0; i < discoveredEnergy.Count; i++)
            {
                if (discoveredEnergy[i][0] == location[0] && discoveredEnergy[i][1] == location[1]) return;
            }
            discoveredEnergy.Add(location);
        }
        internal List<int[]> removeResourceFromDiscoveredResources(int[] location)
        {
            for (int i = 0; i < discoveredResources.Count; i++)
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
