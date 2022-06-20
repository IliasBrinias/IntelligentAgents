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
        public readonly int goalWood = 1;
        public readonly int goalIron = 5;
        public readonly int goalCereal = 1;
        public readonly int goalGold = 3;

        private int currentWood;
        private int currentIron;
        private int currentCereal;
        private int currentGold;
        public List<FirstTeamAgent> firstTeam { get; set; }
        public List<SecondTeamAgent> secondTeam { get; set; }

        public int[] location { get; set; }

        public Village(int[] location)
        {
            r = new Random();
            this.location = location;
            firstTeam = new List<FirstTeamAgent>();
            secondTeam = new List<SecondTeamAgent>();
            currentCereal = 0;
            currentGold = 0;
            currentIron = 0;
            currentWood = 0;
        }
        public void getStatus()
        {

            Console.WriteLine("Wood: " + currentWood + ", Iron: " + currentIron + ", Cereal: " + currentCereal + ", Gold: " + currentGold);
        }
        public void generateAgents(int K)
        {
            // Generates Agents
            for (int j = 0; j < K; j++)
            {
                firstTeam.Add(new FirstTeamAgent(location, Constants.getRandomResources(r.Next())));
                secondTeam.Add(new SecondTeamAgent(this.location));
            }
        }
        public List<String> addResources(List<String> resources)
        {
            List<String> newInventory = new List<string>();
            foreach(String r in resources)
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
        public bool addWood()
        {
            currentWood++;
            return checkIfIsOver();
        }

        public bool addIron()
        {
            currentIron++;
            return checkIfIsOver();

        }
        public bool addCereal()
        {
            currentCereal++;
            return checkIfIsOver();

        }
        public bool addGold()
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
