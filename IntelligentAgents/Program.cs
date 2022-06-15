using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    public class Program
    {
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
            Boolean isOver = false;
            while (!isOver) {
                Console.WriteLine("----------------------------------------------------");
                isOver =villagesAgents(m, m.firstVillage);
                if (isOver) {
                    Console.WriteLine("First Village Win");
                    return;
                }
                //Console.WriteLine("second --");
                isOver = villagesAgents(m, m.secondVillage);
                if (isOver)
                {
                    Console.WriteLine("Second Village Win");
                    continue;
                }

                m.firstVillage.getStatus();
                m.secondVillage.getStatus();
                showStage(m);
                Console.WriteLine("----------------------------------------------------");

            }
        }
        private static Boolean villagesAgents(Map m, Village v)
        {
            //for every firstTeam Agent
            foreach (FirstTeamAgent a in v.firstTeam)
            {
                if (true) continue;
            }
            //for every Second Team Agent
            foreach (SecondTeamAgent a in v.secondTeam)
            {

                Dictionary<String, Object> nearbyCells = m.getNearbyCellsWithStep(a.getCurrentPosition(), a.move());

                if (!a.carry.Equals(""))
                {
                    Console.WriteLine("an agent curry something --");
                    //retund to Village
                    if (a.returnToVillage(v.location, nearbyCells))
                    {
                        Console.WriteLine("find Village --");
                        v.addResources(a.carry);
                        a.carry = "";
                        if (v.checkIfIsOver()) return true;
                    }
                    continue;
                }
                int[] newPosition = a.moveRandomly(nearbyCells);
                a.currentX = newPosition[0];
                a.currentY = newPosition[1];
                if (a.chechIfTheCellHasRecource(m.map[a.currentX, a.currentY]))
                {
                    a.carry = m.map[a.currentX, a.currentY];
                    m.map[a.currentX, a.currentY] = Constants.NOTHING;
                }
            }
            return false;

        }
        private static void showStage(Map m)
        {
            Dictionary<int, int> agentLocation = new Dictionary<int, int>();
            //foreach(Agent a in m.firstVillage.firstTeam){
            //    agentLocation.Add(a.currentX, a.currentY);
            //}
            foreach(Agent a in m.firstVillage.secondTeam){
                if (agentLocation.ContainsKey(a.currentX)) continue;
                agentLocation.Add(a.currentX, a.currentY);
            }
            //foreach(Agent a in m.secondVillage.firstTeam){
            //    if (agentLocation.ContainsKey(a.currentX)) continue;

            //    agentLocation.Add(a.currentX, a.currentY);
            //}
            foreach(Agent a in m.secondVillage.secondTeam){
                if (agentLocation.ContainsKey(a.currentX)) continue;

                agentLocation.Add(a.currentX, a.currentY);
            }
            ConsoleMessages.printBoard(m.map,agentLocation);


        }

    }
}
