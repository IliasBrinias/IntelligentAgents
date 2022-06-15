// See https://aka.ms/new-console-template for more information
using IntelligentAgents;


Random r = new Random();
int N = 30;//Dim
int M = 30;//Dim
int K = 5; //Agents per Team
if (4 > K || K > 10) Console.WriteLine("K must be [4,10]");
if (N > 100 || N<0) Console.WriteLine("N must be [0,100]");
if (M > 100 || M<0) Console.WriteLine("M must be [0,100]");

int[,] board = new int[N,M];
for (int i = 0; i < N; i++) {
    for (int j = 0; j < M; j++)
    {
        board[i,j] = i+j;
    }
}
List<Agent> firstTeam = new List<Agent>();
List<Agent> secondTeam = new List<Agent>();
for (int i = 0; i < K; i++)
{
    Agent agentSpecialized = new Agent(i.ToString(), Qualify.Specialized.ToString());
    firstTeam.Add(agentSpecialized);
    Agent agentSearcher = new Agent(i.ToString(), Qualify.Specialized.ToString());
    agentSearcher.carry = r.Next(0, 3).ToString();
    secondTeam.Add(agentSearcher);
}

ConsoleMessages.printBoard(board);