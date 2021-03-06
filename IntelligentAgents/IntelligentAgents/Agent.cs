using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    internal class Agent
    {
        public List<String> history { get; set; }
        public String name { get; set; }
        public List<String> inventory { get; set; }
        public Boolean IsAlive { get; set; }
        public int energyPoint { get; set; }
        public int currentX { get; set; }
        public int currentY { get; set; }
        public int[] getCurrentPosition() { return new[] { currentX, currentY }; }
        public int energyPotMultiplier { get; set; }
        Random r;
        public Agent(String name, int[] location)
        {
            history = new List<string>();
            this.name = name;
            inventory = new List<String>();
            IsAlive = true;
            this.currentX = location[0];
            this.currentY = location[1];
            r = new Random();
        }
        public int move()
        {
            energyPoint--;
            if (energyPoint <= 0) IsAlive = false;
            Console.WriteLine("Energy: " + energyPoint);
            return r.Next(1, 3);
        }
        internal int[] moveRandomly(Dictionary<string, object> nearbyCells)
        {
            List<String> options = new List<String>(nearbyCells.Keys);
            int index = r.Next(0, options.Count);
            Console.WriteLine(index + ":" + options[index]);

            return (int[])((Dictionary<string, object>)nearbyCells[options[index]])["location"];
        }
        internal Boolean returnToVillage(int[] villagePosition, Dictionary<String, Object> nearbyCells)
        {
            return goToLocation(villagePosition, nearbyCells);
        }

        internal Boolean moveToResource(int[] resourcePosition, Dictionary<String, Object> nearbyCells)
        {
            return goToLocation(resourcePosition, nearbyCells);
        }
        private Boolean goToLocation(int[] targetPosition, Dictionary<String, Object> nearbyCells)
        {
            Dictionary<String, Object> nextLocation = moveTo(targetPosition, nearbyCells);

            int[] newPosition = (int[])nextLocation["location"];

            currentX = newPosition[0]; currentY = newPosition[1];

            return targetPosition[0] == getCurrentPosition()[0] && targetPosition[1] == getCurrentPosition()[1];
        }
        public Dictionary<String, Object> moveTo(int[] targetPosition, Dictionary<String, Object> nearbyCells)
        {
            Console.WriteLine("!Move To (" + targetPosition[0] + "," + targetPosition[1] + ")");
            Console.WriteLine("targetPosition[0]: " + targetPosition[0]);
            Console.WriteLine("currentX: " + currentX);
            if (targetPosition[0] > currentX)
            {
                Console.WriteLine("DOWN ");

                return getLocationTo(nearbyCells, "down");

            }
            else if (targetPosition[0] < currentX)
            {
                Console.WriteLine("UP ");
                return getLocationTo(nearbyCells, "up");
            }
            else
            {
                Console.WriteLine("Same X");
                Console.WriteLine("targetPosition[1]: " + targetPosition[1]);
                Console.WriteLine("currentY: " + currentY);

                if (targetPosition[1] > currentY)
                {
                    Console.WriteLine("RIGHT ");
                    return getLocationTo(nearbyCells, "right");
                }
                else if (targetPosition[1] < currentY)
                {
                    Console.WriteLine("LEFT ");
                    return getLocationTo(nearbyCells, "left");
                }
                else
                {
                    Dictionary<String, Object> target = new Dictionary<string, object>();
                    target.Add("location", targetPosition);
                    return target;
                }
            }
        }
        private Dictionary<String, Object> getLocationTo(Dictionary<String, Object> nearbyCells, String move)
        {
            try
            {
                return (Dictionary<String, Object>)nearbyCells[move];
            }
            catch
            {
                Console.WriteLine("Cant go to " + move + " go to " + nearbyCells.First().Key);

                return (Dictionary<String, Object>)nearbyCells.First().Value;
            }
        }
        public void increaseEnergy()
        {
            Console.WriteLine("Energy increced from :" + energyPoint + " To: " + (energyPoint + energyPotMultiplier));
            energyPoint += energyPotMultiplier;
        }
    }
}
