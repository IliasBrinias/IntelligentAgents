using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    internal class ConsoleMessages
    {
        public static StringBuilder printBoard(String[,] board, Dictionary<int,int> agentsLocations)
        {
            StringBuilder lines = new StringBuilder();
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
                lines.AppendLine(stringBuilder.ToString());
            }
            Console.WriteLine("-----------------");
            return lines;
        }

        internal static void showDeadAgentHistory(List<Agent> deadAgents)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-- History of dead agents");
            foreach (Agent a in deadAgents)
            {
                sb.AppendLine("-- " + a.name);
                foreach (String s in a.history)
                {
                    sb.AppendLine(s);
                }
            }
            saveDataToFile("DeadAgentHistory", sb);
        }
        
        public static void showAgentHistory(Map m)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-- History of agents");
            foreach (FirstTeamAgent a in m.firstVillage.firstTeam)
            {
                sb.AppendLine("-- " + a.name);
                foreach (String s in a.history)
                {
                    sb.AppendLine(s);

                }

            }
            foreach (SecondTeamAgent a in m.firstVillage.secondTeam)
            {
                sb.AppendLine("-- " + a.name);
                foreach (String s in a.history)
                {
                    sb.AppendLine(s);

                }

            }
            foreach (FirstTeamAgent a in m.secondVillage.firstTeam)
            {
                sb.AppendLine("-- " + a.name);
                foreach (String s in a.history)
                {
                    sb.AppendLine(s);

                }

            }
            foreach (SecondTeamAgent a in m.secondVillage.secondTeam)
            {
                sb.AppendLine("-- " + a.name);
                foreach (String s in a.history)
                {
                    sb.AppendLine(s);

                }

            }
            saveDataToFile("AgentHistory", sb);
        }
        private static void saveDataToFile(String fileName,StringBuilder sb)
        {
            clearDataFile(fileName);
            try
            {
                Directory.CreateDirectory(".\\Logs");
                //Pass the filepath and filename to the StreamWriter Constructor
                StreamWriter sw = new StreamWriter(".\\Logs\\"+fileName+".txt");
                string[] delim = { Environment.NewLine, "\n" };
                foreach (String s in sb.ToString().Split(delim, StringSplitOptions.None))
                {
                    sw.WriteLine(s);
                }
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        public static void clearDataFile(String fileName)
        {
            System.IO.File.WriteAllText(".\\Logs\\" + fileName + ".txt", string.Empty);
        }
        public static void appendDataToFile(String fileName, String[,] board, Dictionary<int, int> agentsLocations, int loop)
        {
            try
            {
                Directory.CreateDirectory(".\\Logs");
                //Pass the filepath and filename to the StreamWriter Constructor
    
                string[] delim = { Environment.NewLine, "\n" };
               
                using (StreamWriter w = File.AppendText(".\\Logs\\" + fileName + ".txt"))
                {
                    w.WriteLine("-----------------");
                    w.WriteLine("Loop: "+loop);
                    foreach(String s in printBoard(board, agentsLocations).ToString().Split(delim, StringSplitOptions.None))
                    {
                        w.WriteLine(s);
                    }
                    w.WriteLine("-----------------");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}
