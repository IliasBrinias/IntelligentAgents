using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    public class Program
    {
        static List<Agent> deadAgents = new List<Agent>();
        static bool isOver = false;
        static List<String[,]> mapHistory = new List<string[,]>();
        private static void printList(List<int[]> ints)
        {
            foreach (int[] i in ints){
                Console.WriteLine("(" + i[0] + "," + i[1] + ")");
            }
        }
        static void Main() {
            // N, M dim N,M>100x100
            // K agents
            // P how many pots
            // X Energy Pot value
            // Y Map Cost
            Console.WriteLine("~ Intelligent Agents");
            int N = userInput("Enter the N Dimension (N>=100) :",100);
            int M = userInput("Enter the M Dimension (M>=100) :", 100);
            int K = userInput("Enter the number of agents for every team (5 <= agents <= 10) :",5,10);
            int Υ = userInput("Enter the value of map (1 <= cost <= 3) :", 1, 3);
            int X = userInput("Enter the value of Potion (1 <= cost <= 3) :", 1, 3);
            double P = 0.2;
            Map m = new Map(N, M, P, K, X, Υ);
            // run until any village win
            int loop = 0;
            ConsoleMessages.clearDataFile("mapHistory");
            while (!isOver) {
                Console.WriteLine(" Start Loop ----------------------------------------------------");
                // first village
                Console.WriteLine("!! First Village");
                checkResults(villagesAgents(m, m.firstVillage), "First Village");
                if (isOver)  break;
                // second village
                Console.WriteLine("!! Second Village");
                checkResults(villagesAgents(m, m.secondVillage), "Second Village");
                if (isOver) break;
                loop += 1;
                showStage(m, loop);
                Console.WriteLine(" Finish Loop ----------------------------------------------------");
                mapHistory.Append(m.map);
            }
            ConsoleMessages.showAgentHistory(m);
            ConsoleMessages.showDeadAgentHistory(deadAgents);
            m.firstVillage.getStatus();
            m.secondVillage.getStatus();

        }
        private static int userInput(String msg, int min)
        {
            int op = 0;
            string input;
            do
            {
                Console.WriteLine(msg);
                input = Console.ReadLine();
            } while (!int.TryParse(input, out op) || op<min);
            return op;
        }
        private static int userInput(String msg, int min, int max)
        {
            int op = 0;
            string input;
            do
            {
                Console.WriteLine(msg);
                input = Console.ReadLine();
            } while (!int.TryParse(input, out op) || (op < min && op > max));
            return op;
        }
        private static void printDic(Dictionary<int,Object> data)
        {
            foreach (int i in new List<int>(data.Keys))
            {
                Dictionary<int, Boolean> dicV = (Dictionary<int, bool>)data[i];
                StringBuilder stringBuilder = new StringBuilder();
                foreach (int y in new List<int>(dicV.Keys))
                {
                    stringBuilder.Append("(" + i + "," + y + ")");
                }
                Console.WriteLine(stringBuilder);

            }

        }
        private static void checkResults(String result, String villageName)
        {
            if (result.Equals(""))
            {
                return;
            }else if (result.Equals(Constants.VILLAGE_WIN))
            {
                Console.WriteLine("Village " + villageName + " win");
                return;
            }else if (result.Equals(Constants.AGIENTS_DIED))
            {
                Console.WriteLine("Village " + villageName + " loose, all the agients Died");


                return;
            }
        }
        private static String SecondTeam(Map m ,Village v)
        {
            //for every Second Team Agent
            foreach (SecondTeamAgent a in v.secondTeam)
            {
                
                Console.WriteLine("Second Team");

                // get the cells that the agent can move base on step
                Dictionary<String, Object> nearbyCells = m.getNearbyCellsWithStep(a.getCurrentPosition(), a.move());
                
                // check the agent if is has anything on the inventory
                
                if (a.inventory.Count>0)
                {
                    //retund to Village
                    if (a.returnToVillage(v.location, nearbyCells))
                    {
                        // when he is in Village add the Resource that he carried
                        v.addResources(a.inventory);
                        a.inventory.Clear();
                        a.history.Add("(" + a.currentX + "," + a.currentY + ")" + " arrived to Village");

                        // check if the Village is ok
                        if (v.checkIfIsOver())
                        {
                            isOver = true;
                            return Constants.VILLAGE_WIN;
                        }
                    }
                    else
                    {
                        if (m.map[a.currentX, a.currentY].Equals(Constants.ENERGY_POTS))
                        {
                            a.increaseEnergy();
                            m.map[a.currentX, a.currentY] = Constants.NOTHING;
                            a.history.Add("(" + a.currentX + "," + a.currentY + ")" + " found energy on the way back to village");

                        }
                        else
                        {
                            a.history.Add("(" + a.currentX + "," + a.currentY + ")" + " return to Village");
                        }
                    }
                    continue;
                }
                // get a random location from the nearby Cells
                int[] newPosition = a.moveRandomly(nearbyCells);
                a.currentX = newPosition[0]; a.currentY = newPosition[1];

                // chech if the current position has any resources
                if (a.chechIfTheCellHasRecource(m.map[a.currentX, a.currentY]))
                {
                    if(m.map[a.currentX, a.currentY].Equals(Constants.ENERGY_POTS)){
                        // if the cell has energy pot increase energy to the agent
                        a.increaseEnergy();
                        a.history.Add("(" + a.currentX + "," + a.currentY + ")" + " found energy");
                    }
                    else
                    {
                        a.inventory.Add(m.map[a.currentX, a.currentY]);
                        a.history.Add("(" + a.currentX + "," + a.currentY + ")" + " found resource " + m.map[a.currentX, a.currentY]);

                    }
                    m.map[a.currentX, a.currentY] = Constants.NOTHING;
                }
            }
            return "";
        }
        private static Boolean ifTheCellHasEnergyTakeIt(Map m, Agent a)
        {
            // if the cell has energy pot increase energy to the agent
            if (m.map[a.currentX, a.currentY] == Constants.ENERGY_POTS)
            {
                Console.WriteLine("Energy Found and Saved");
                try
                {
                    ((FirstTeamAgent)a).discoveredEnergy = ((FirstTeamAgent)a).removeDiscoveredEnergy(new int[] { a.currentX, a.currentY });
                    a.inventory.Add(m.map[a.currentX, a.currentY]);
                }
                catch
                {
                    a.increaseEnergy();

                }
                m.map[a.currentX, a.currentY] = Constants.NOTHING;

                return true;
            }

            return false;
        }
        private static String FirstTeam(Map m, Village v)
        {
            foreach (FirstTeamAgent a in v.firstTeam)
            {
                
                Console.WriteLine("First Team");
                Console.WriteLine("Name: "+a.name);
                // show Inventory
                printInventory(a);

                // get the cells that the agent can move base on step
                Dictionary<String, Object> nearbyCells = m.getNearbyCellsWithStep(a.getCurrentPosition(), a.move());

                //if the agent has low energy search for energy
                if (a.hasLowEnergy())
                {
                    // check if he has poisons in the inventory
                    if (!a.ifHasPosionUseIt())
                    {
                        Console.WriteLine("Search for Energy");
                        int[] newPositionForEnergy;
                        // check if he has any discovered location energy
                        if (a.hasDiscoveredEnergy())
                        {
                            // go to a discovered location
                            Console.WriteLine("Agent has discovered Energy");
                            newPositionForEnergy = a.getTheLocationOfDiscoveredEnergy();
                            // move to the Energy
                            if (a.moveToResource(newPositionForEnergy, nearbyCells))
                            {
                                a.removeDiscoveredEnergy(newPositionForEnergy);
                            }
                            else
                            {
                                a.history.Add("(" + a.currentX + "," + a.currentY + ")" + " searching for energy, going to " + "(" + newPositionForEnergy[0] +","+ newPositionForEnergy[1] + ")");
                            }
                        }
                        else
                        {
                            // choose the new Position
                            newPositionForEnergy = a.chooseTheCellBasedOn(nearbyCells, Constants.ENERGY_POTS);
                            a.currentX = newPositionForEnergy[0]; a.currentY = newPositionForEnergy[1];
                        }
                        // check if the new position has energy
                        if (ifTheCellHasEnergyTakeIt(m, a))
                        {
                            // use the posion
                            a.ifHasPosionUseIt();
                            a.history.Add("(" + a.currentX + "," + a.currentY + ")" + " energy found at " + "(" + newPositionForEnergy[0] + "," + newPositionForEnergy[1] + ")");

                        }
                        else
                        {
                            // if he has resources save it to the inventory
                            if(ifTheMapHasResourcesTakeIt(m, a))
                            {
                                a.history.Add("(" + a.currentX + "," + a.currentY + ")" + " resources found at " + "(" + newPositionForEnergy[0] + "," + newPositionForEnergy[1] + ")");
                            }
                        }
                        a.checkIfAnotherAgentHasSamePosition(m.firstVillage.firstTeam, m.secondVillage.firstTeam, m.firstVillage.location);

                        continue;
                    }
                    else
                    {
                        a.history.Add("(" + a.currentX + "," + a.currentY + ")" + " used a potion from inventory");
                    }

                }

                // check the agent if is carrying something
                if (a.hasResourcesInInventory())
                {
                    Console.WriteLine("Agent curry is returning");
                    //retund to Village
                    if (a.returnToVillage(v.location, nearbyCells))
                    {
                        a.history.Add("(" + a.currentX + "," + a.currentY + ")" + " arrived at Village with inventory items");

                        Console.WriteLine("Arrived to Village");
                        
                        // when he is in Village add the Resource that he carried and
                        // return the Energy posions that he had
                        a.inventory = v.addResources(a.inventory);

                        // check if the Village is ok
                        if (v.checkIfIsOver())
                        {
                            isOver = true;
                            return Constants.VILLAGE_WIN;
                        }
                    }
                    else
                    {
                        if (ifTheMapHasResourcesTakeIt(m, a))
                        {
                            a.history.Add("(" + a.currentX + "," + a.currentY + ")" + " resources found");

                        }
                        if(ifTheCellHasEnergyTakeIt(m, a))
                        {
                            a.history.Add("(" + a.currentX + "," + a.currentY + ")" + " energy found");

                        }
                    }
                    a.checkIfAnotherAgentHasSamePosition(m.firstVillage.firstTeam, m.secondVillage.firstTeam, m.firstVillage.location);

                    continue;
                }
                // if he has enough energy check if he has any discovered resources 
                int[] newPosition;
                if (a.hasDiscoveredResources())
                {
                    Console.WriteLine("Agent has discoveredResources");
                    newPosition = a.getTheLocationOfResource();
                    if (a.moveToResource(newPosition, nearbyCells))
                    {
                        a.removeResourceFromDiscoveredResources(newPosition);
                    }
                }
                else
                {
                    // choose the new Position
                    newPosition = a.chooseTheCellBasedOn(nearbyCells,a.qualify);
                    a.currentX = newPosition[0]; a.currentY = newPosition[1];

                }
                // if the cell has energy pot save it to the inventory
                if (ifTheCellHasEnergyTakeIt(m, a))
                {
                    a.history.Add("(" + a.currentX + "," + a.currentY + ")" + " agent found energy and save to inventory");
                }
                // if the cell has resorces save it to the inventory
                if (ifTheMapHasResourcesTakeIt(m, a))
                {
                    a.history.Add("(" + a.currentX + "," + a.currentY + ")" + " resources found");
                }
                a.checkIfAnotherAgentHasSamePosition(m.firstVillage.firstTeam, m.secondVillage.firstTeam, m.firstVillage.location);
            }
            return "";
        }
        private static void printInventory(FirstTeamAgent a)
        {
            Console.WriteLine("Qualify: " + a.qualify);
            Console.WriteLine("-");
            Console.WriteLine("Inventory");
            StringBuilder sb = new StringBuilder();
            foreach (String s in a.inventory)
            {
                sb.Append(s + ", ");
            }
            Console.WriteLine(sb);
            Console.WriteLine("-");


        }
        private static void ifTheMapHasResourcesSaveThem(Map m, FirstTeamAgent a)
        {
            // chech if the current position has any resources
            if (a.chechIfTheCellHasRecource(m.map[a.currentX, a.currentY]))
            {
                // add it to the Discovered Resources list for next Time
                a.addToDiscoveredList(new int[] { a.currentX, a.currentY });
            } else if (m.map[a.currentX, a.currentY].Equals(Constants.ENERGY_POTS))
            {
                a.saveLocationOfEnergy(new int[] { a.currentX, a.currentY });
            }

        }
        private static bool ifTheMapHasResourcesTakeIt(Map m, FirstTeamAgent a)
        {

            // chech if the current position has any resources
            if (a.chechIfTheCellHasRecource(m.map[a.currentX, a.currentY]))
            {
                Console.WriteLine("Resource Found on return save it to inventory");
                StringBuilder stringBuilder = new StringBuilder();
                foreach (String s in a.inventory)
                {
                    stringBuilder.Append(s + ",");
                }
                Console.WriteLine(stringBuilder);

                a.inventory.Add(m.map[a.currentX, a.currentY]);
                m.map[a.currentX, a.currentY] = Constants.NOTHING;
                // remove the resource if it was discovered
                a.removeResourceFromDiscoveredResources(new int[] { a.currentX, a.currentY });
                return true;
            }
            return false;

        }
        private static String villagesAgents(Map m, Village v)
        {
            //for every firstTeam Agent
            if (FirstTeam(m, v).Equals(Constants.VILLAGE_WIN)) return Constants.VILLAGE_WIN;

            //for every Second Team Agent
            if (SecondTeam(m, v).Equals(Constants.VILLAGE_WIN)) return Constants.VILLAGE_WIN;

            // clear all the agent that they died
            v.secondTeam.RemoveAll(WhereAgentIsDead);
            v.firstTeam.RemoveAll(WhereAgentIsDead);
            if (v.secondTeam.Count == 0 && v.firstTeam.Count == 0)
            {
                // if they died return the message
                isOver = true;
                return Constants.AGIENTS_DIED;
            }
            return "";
        }
        // Search predicate returns true if the agent is dead.
        private static bool WhereAgentIsDead(Agent a)
        {
            if (!a.IsAlive)
            {
                a.history.Add("Agent Died");
                deadAgents.Add(a);
            }
            return !a.IsAlive;
        }

        private static void showStage(Map m,int loop)
        {
            Dictionary<int, int> agentLocation = new Dictionary<int, int>();
            foreach(Agent a in m.firstVillage.firstTeam){
                if (agentLocation.ContainsKey(a.currentX)) continue;
                agentLocation.Add(a.currentX, a.currentY);
            }
            foreach(Agent a in m.firstVillage.secondTeam){
                if (agentLocation.ContainsKey(a.currentX)) continue;
                agentLocation.Add(a.currentX, a.currentY);
            }
            foreach(Agent a in m.secondVillage.firstTeam){
                if (agentLocation.ContainsKey(a.currentX)) continue;

                agentLocation.Add(a.currentX, a.currentY);
            }
            foreach(Agent a in m.secondVillage.secondTeam){
                if (agentLocation.ContainsKey(a.currentX)) continue;
                agentLocation.Add(a.currentX, a.currentY);
            }
            ConsoleMessages.appendDataToFile("mapHistory", m.map, agentLocation, loop);

        }

    }
}
