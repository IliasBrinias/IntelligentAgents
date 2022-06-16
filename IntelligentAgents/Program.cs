using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    public class Program
    {
        static bool isOver = false;
        static void Main() {
            // N, M dim N,M>100x100
            // K agents
            // X Energy Pot value
            // Y Map Cost
//                Console.WriteLine(i);

            int N = 15;
            int M = 15;
            int K = 1;
            Map m = new Map(N, M, K);
            
            String[,] map = m.map;
            Dictionary<int, int> agentLocation = new Dictionary<int, int>();
            showStage(m);
            while (!isOver) {
                Console.WriteLine("----------------------------------------------------");
                checkResults(villagesAgents(m, m.firstVillage), "first Village");
                if (isOver) continue;

                //checkResults(villagesAgents(m, m.secondVillage), "Second Village");
                //if (isOver) continue;

                m.firstVillage.getStatus();
                m.secondVillage.getStatus();
                showStage(m);
                Console.WriteLine("----------------------------------------------------");

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
                //check if is alive
                if (!a.IsAlive)
                {
                    continue;
                }
                // get the cells that the agent can move base on step
                Dictionary<String, Object> nearbyCells = m.getNearbyCellsWithStep(a.getCurrentPosition(), a.move());
                // check the agent if is carrying something
                if (!a.carry.Equals(Constants.NO_CARRY))
                {
                    //retund to Village
                    if (a.returnToVillage(v.location, nearbyCells))
                    {
                        // when he is in Village add the Resource that he carried
                        v.addResources(a.carry);
                        a.carry = Constants.NO_CARRY;
                        // check if the Village is ok
                        if (v.checkIfIsOver())
                        {
                            isOver = true;
                            return Constants.VILLAGE_WIN;
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
                    // carry the resources and clear the cell of the map
                    a.carry = m.map[a.currentX, a.currentY];
                    m.map[a.currentX, a.currentY] = Constants.NOTHING;
                }
            }
            return "";
        }
        private static String FirstTeam(Map m, Village v)
        {
            foreach (FirstTeamAgent a in v.firstTeam)
            {
                //check if is alive
                if (!a.IsAlive)
                {
                    continue;
                }
                Console.WriteLine("Qualify: " + a.qualify);
                // get the cells that the agent can move base on step
                Dictionary<String, Object> nearbyCells = m.getNearbyCellsWithStep(a.getCurrentPosition(), a.move());
                // check the agent if is carrying something
                if (!a.carry.Equals(Constants.NO_CARRY))
                {
                    Console.WriteLine("Agent curry");
                    //retund to Village
                    if (a.returnToVillage(v.location, nearbyCells))
                    {
                        Console.WriteLine("Arrived to Village");
                        // when he is in Village add the Resource that he carried
                        v.addResources(a.carry);
                        a.carry = Constants.NO_CARRY;
                        // check if the Village is ok
                        if (v.checkIfIsOver())
                        {
                            isOver = true;
                            return Constants.VILLAGE_WIN;
                        }
                    }
                    else
                    {
                        ifTheMapHasResourcesSaveThem(m, a);
                    }
                    continue;
                }
                int[] newPosition;
                if (a.hasDiscoveredResources())
                {
                    Console.WriteLine("Agent has discoveredResources");
                    newPosition = a.getTheLocationOfResource();
                    if (a.moveToResource(newPosition, nearbyCells))
                    {
                        ifTheMapHasResourcesTakeIt(m, a);
                    }
                    else
                    {
                        ifTheMapHasResourcesSaveThem(m, a);
                    }
                }
                else
                {
                    // choose the new Position
                    newPosition = a.chooseTheCell(nearbyCells);
                    a.currentX = newPosition[0]; a.currentY = newPosition[1];

                }
                ifTheMapHasResourcesTakeIt(m, a);
            }
            return "";
        }

        private static void ifTheMapHasResourcesSaveThem(Map m, FirstTeamAgent a)
        {
            // chech if the current position has any resources
            if (a.chechIfTheCellHasRecource(m.map[a.currentX, a.currentY]))
            {
                // add it to the Discovered Resources list for next Time
                a.addToDiscoveredList(new int[] { a.currentX, a.currentY });
            }

        }
        private static void ifTheMapHasResourcesTakeIt(Map m, FirstTeamAgent a)
        {
            // chech if the current position has any resources
            if (a.chechIfTheCellHasRecource(m.map[a.currentX, a.currentY]))
            {
                a.carry = m.map[a.currentX, a.currentY];
                m.map[a.currentX, a.currentY] = Constants.NOTHING;
            }

        }
        private static String villagesAgents(Map m, Village v)
        {
            //for every firstTeam Agent
            if (FirstTeam(m, v).Equals(Constants.VILLAGE_WIN)) return Constants.VILLAGE_WIN;

            //for every Second Team Agent
            //if (SecondTeam(m, v).Equals(Constants.VILLAGE_WIN)) return Constants.VILLAGE_WIN;

            // clear all the agent that they died
            v.secondTeam.RemoveAll(WhereAgentIsDead);
            v.firstTeam.RemoveAll(WhereAgentIsDead);
            if (v.secondTeam.Count == 0)
            {
                // if they died return the message
                isOver = true;
                return Constants.AGIENTS_DIED;
            }
            return "";
        }
        private static void showStage(Map m)
        {
            Dictionary<int, int> agentLocation = new Dictionary<int, int>();
            foreach(Agent a in m.firstVillage.firstTeam){
                if (agentLocation.ContainsKey(a.currentX)) continue;
                agentLocation.Add(a.currentX, a.currentY);
            }
            //foreach(Agent a in m.firstVillage.secondTeam){
            //    if (agentLocation.ContainsKey(a.currentX)) continue;
            //    agentLocation.Add(a.currentX, a.currentY);
            //}
            foreach(Agent a in m.secondVillage.firstTeam){
                if (agentLocation.ContainsKey(a.currentX)) continue;

                agentLocation.Add(a.currentX, a.currentY);
            }
            //foreach(Agent a in m.secondVillage.secondTeam){
            //    if (agentLocation.ContainsKey(a.currentX)) continue;

            //    agentLocation.Add(a.currentX, a.currentY);
            //}
            ConsoleMessages.printBoard(m.map,agentLocation);


        }
        // Search predicate returns true if the agent is dead.
        private static bool WhereAgentIsDead(Agent a)
        {
            return a.IsAlive == false;
        }


    }
}
