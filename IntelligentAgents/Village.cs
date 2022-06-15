using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    internal class Village
    {
        public readonly int goalWood = 1;
        public readonly int goalIron = 1;
        public readonly int goalCereal = 1;
        public readonly int goalGold = 1;

        private int currentWood;
        private int currentIron;
        private int currentCereal;
        private int currentGold;
        public List<FirstTeamAgent> firstTeam { get; set; }
        public List<SecondTeamAgent> secondTeam { get; set; }

        public int[] location { get; set; }

        public Village(int[] location)
        {
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
                firstTeam.Add(new FirstTeamAgent(this.location, Constants.getRandomResources()));
                secondTeam.Add(new SecondTeamAgent(this.location));
            }
        }
        public void addResources(String resources)
        {
            if (resources.Equals(Constants.GOLD))
            {
                addGold();
            }else if(resources.Equals(Constants.WOOD))
            {
                addWood();
            }
            else if (resources.Equals(Constants.IRON))
            {
                addIron();
            }
            else if (resources.Equals(Constants.CEREALS))
            {
                addCereal();
            }
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
            return (currentCereal == goalCereal &&
                currentGold == goalGold &&
                currentWood == goalWood &&
                currentIron == goalIron
                );
        }
    }
}
