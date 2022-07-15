using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    internal class Village
    {
        Random r;
        public readonly int goalWood = 15;
        public readonly int goalIron = 8;
        public readonly int goalCereal = 4;
        public readonly int goalGold = 10;

        private int currentWood;
        private int currentIron;
        private int currentCereal;
        private int currentGold;

        String name;
        public List<FirstTeamAgent> firstTeam { get; set; }
        public List<SecondTeamAgent> secondTeam { get; set; }

        public int[] location { get; set; }

        public Village(String name, int[] location)
        {
            this.name = name;
            r = new Random();
            this.location = location;
            firstTeam = new List<FirstTeamAgent>();
            secondTeam = new List<SecondTeamAgent>();
            currentCereal = 0;
            currentGold = 0;
            currentIron = 0;
            currentWood = 0;
        }
        public void generateAgents(int K, int M, int X, int Y)
        {
            // Generates Agents
            for (int j = 0; j < K; j++)
            {
                firstTeam.Add(new FirstTeamAgent(name + "F" + j, location, M, X, Y, Constants.getRandomResources(r.Next())));
                secondTeam.Add(new SecondTeamAgent(name + "S" + j, this.location, M));
            }
        }

        public void getStatus()
        {

            Console.WriteLine("Wood: " + currentWood + ", Iron: " + currentIron + ", Cereal: " + currentCereal + ", Gold: " + currentGold);
        }
        public List<String> addResources(List<String> resources)
        {
            List<String> newInventory = new List<string>();
            foreach (String r in resources)
            {
                if (r.Equals(Constants.GOLD))
                {
                    addGold();
                }
                else if (r.Equals(Constants.WOOD))
                {
                    addWood();
                }
                else if (r.Equals(Constants.IRON))
                {
                    addIron();
                }
                else if (r.Equals(Constants.CEREALS))
                {
                    addCereal();
                }
                else if (r.Equals(Constants.ENERGY_POTS))
                {
                    newInventory.Add(Constants.ENERGY_POTS);
                }
            }
            return newInventory;
        }
        private bool addWood()
        {
            currentWood++;
            return checkIfIsOver();
        }

        private bool addIron()
        {
            currentIron++;
            return checkIfIsOver();

        }
        private bool addCereal()
        {
            currentCereal++;
            return checkIfIsOver();

        }
        private bool addGold()
        {
            currentGold++;
            return checkIfIsOver();

        }
        public Boolean checkIfIsOver()
        {
            getStatus();
            return (currentCereal >= goalCereal &&
                currentGold >= goalGold &&
                currentWood >= goalWood &&
                currentIron >= goalIron
                );
        }
    }
}
