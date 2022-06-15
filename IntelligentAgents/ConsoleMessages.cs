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
    }
}
