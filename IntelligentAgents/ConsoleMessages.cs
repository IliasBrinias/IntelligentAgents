using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    internal class ConsoleMessages
    {
        public static void printBoard(int[,] board)
        {
            Console.WriteLine("-----------------");
            for(int i=0; i<board.GetLength(0);i++)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    stringBuilder.Append(board[i,j]);
                    stringBuilder.Append(" , ");
                }

                Console.WriteLine(stringBuilder);
            }
            Console.WriteLine("-----------------");
        }
    }
}
