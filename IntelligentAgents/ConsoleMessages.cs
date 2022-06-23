using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    internal class ConsoleMessages
    {
        public static void printBoard(String[,] board, Dictionary<int,int> agentsLocations)
        {

            Console.WriteLine("-----------------");
            for(int i=0; i<board.GetLength(0);i++)
            {
                StringBuilder stringBuilder = new StringBuilder();
                // print board Line
                stringBuilder.Append("[");
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    stringBuilder.Append(board[i,j]);
                    if (j == board.GetLength(1) - 1) continue;
                    stringBuilder.Append(",");
                }
                stringBuilder.Append("]");
                stringBuilder.Append(" | ");
                // print Agents
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (agentsLocations.ContainsKey(i))
                    {
                        if (j == agentsLocations[i])
                        {
                            stringBuilder.Append("A");
                            if (j == board.GetLength(1) - 1) continue;
                            stringBuilder.Append(",");
                            continue;
                        }
                    }
                    stringBuilder.Append("_");
                    if (j == board.GetLength(1) - 1) continue;
                    stringBuilder.Append(",");
                }
                Console.WriteLine(stringBuilder);
            }
            Console.WriteLine("-----------------");
        }

        internal static void showDeadAgentHistory(List<Agent> deadAgents)
        {
            Console.WriteLine("-- History of dead agents");
            foreach (Agent a in deadAgents)
            {
                Console.WriteLine("-- " + a.name);
                foreach (String s in a.history)
                {
                    Console.WriteLine(s);

                }

            }
        }

        public static void showAgentHistory(Map m)
        {
            Console.WriteLine("-- History of agents");
            foreach (FirstTeamAgent a in m.firstVillage.firstTeam)
            {
                Console.WriteLine("-- " + a.name);
                foreach (String s in a.history)
                {
                    Console.WriteLine(s);

                }

            }
            foreach (SecondTeamAgent a in m.firstVillage.secondTeam)
            {
                Console.WriteLine("-- " + a.name);
                foreach (String s in a.history)
                {
                    Console.WriteLine(s);

                }

            }
            foreach (FirstTeamAgent a in m.secondVillage.firstTeam)
            {
                Console.WriteLine("-- " + a.name);
                foreach (String s in a.history)
                {
                    Console.WriteLine(s);

                }

            }
            foreach (SecondTeamAgent a in m.secondVillage.secondTeam)
            {
                Console.WriteLine("-- " + a.name);
                foreach (String s in a.history)
                {
                    Console.WriteLine(s);

                }

            }

        }
    }
}
