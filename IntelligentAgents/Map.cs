using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    internal class Map
    {
        Random r;
        public Village firstVillage { get; set; }
        public Village secondVillage { get; set; }
        public String[,] map { get; } 
        public int N { get; set; }
        public int M { get; set; }
        public int mapCost { get; set; }

        public Map(int n, int m, double x, int k, int y)
        {
            r = new Random();
            N = n;
            M = m;
            mapCost = y;
            map = generateMap(n,m,x,k);
        }
        private String[,] generateMap(int N, int M, double X, int K)
        {
            String[,] map = new String[N, M];
            List<int[]> locations = new List<int[]>();
            for (int i = 0; i < N; i++)
            {
                for (int j=0; j<M; j++)
                {
                    if (i == 0 && j == 0 || i == N - 1 && j == M - 1) continue;
                    locations.Add(new int[] {i,j});
                }
            }
            
            // Generate Villages
            firstVillage = new Village(new int[] {0,0});
            map[0, 0] = Constants.Village;

            secondVillage = new Village(new int[] {N-1,M-1});
            map[ N-1, M-1 ] = Constants.Village;

            //generate Agents
            firstVillage.generateAgents(K);
            secondVillage.generateAgents(K);

            //Generate Energy Pots
            for (int i = 0; i < locations.Count * X; i++)
            {
                int randomIndex = r.Next(0, locations.Count);
                int[] loacationIndex = locations[randomIndex];
                map[loacationIndex[0], loacationIndex[1]] = Constants.ENERGY_POTS;
                //map[loacationIndex[0], loacationIndex[1]] = Constants.getRandomResources();
                locations.RemoveAt(randomIndex);
            }

            // Generate Resources
            int bound = locations.Count / 2;
            int upperBound = (int)(locations.Count *0.98);

            for (int i = 0; i < r.Next(bound, upperBound); i++)
            {
                int randomIndex = r.Next(0, locations.Count);
                int[] loacationIndex = locations[randomIndex];
                map[loacationIndex[0], loacationIndex[1]] = Constants.CEREALS;
                //map[loacationIndex[0], loacationIndex[1]] = Constants.getRandomResources();
                locations.RemoveAt(randomIndex);
            }
            foreach (int[] location in locations)
            {
                map[location[0], location[1]] = Constants.NOTHING;
            }
            return map;
        }
        
        public Dictionary<String,Object> getNearbyCellsWithStep(int[] currentLocation,int step)
        {
            Dictionary<String,Object> result = new Dictionary<String,Object>();

            Dictionary<String, Object> element = getUpElement(currentLocation, step);
            if (element != null) result.Add("up", element);
            element = getDownElement(currentLocation, step);
            if (element != null) result.Add("down", element);
            element = getRightElement(currentLocation, step);
            if (element != null) result.Add("right", element);
            element = getLeftElement(currentLocation, step);
            if (element != null) result.Add("left", element);
            return result;
        }
        private int[] getUpPosition(int[] currentLocation, int step) {

            if(currentLocation[0] - step < 0)
            {
                return new int[] { currentLocation[0] - 1, currentLocation[1] };

            }
            return new int[] {currentLocation[0] - step ,  currentLocation[1]};
        }
        private int[] getDownPosition(int[] currentLocation, int step) {
            
            if (currentLocation[0] + step > N-1)
            {
                return new int[] { currentLocation[0] + 1, currentLocation[1] };

            }

            return new int[] { currentLocation[0] + step ,  currentLocation[1]  };
        }
        private int[] getRightPosition(int[] currentLocation, int step) {
            if (currentLocation[1] + step > M-1)
            {
                return new int[] { currentLocation[0], currentLocation[1] + 1 };

            }

            return new int[]  { currentLocation[0],  currentLocation[1] + step };
        }
        private int[] getLeftPosition(int[] currentLocation, int step) {
           
            if (currentLocation[1] - step < 0)
            {
                return new int[] { currentLocation[0], currentLocation[1] - 1 };

            }

            return new int[] { currentLocation[0], currentLocation[1] - step};
        }
        private Dictionary<String,Object> getDictionary(int[] position, String mapItem)
        {
            Dictionary<String, Object> item = new Dictionary<string, object>();
            item.Add("location", position);
            item.Add("item", mapItem);
            Console.WriteLine(position[0] +","+ position[1] +"  |"+mapItem+"|");
            return item;
        }
        
        private Dictionary<String, Object>? getUpElement(int[] currentLocation, int step)
        {
            try
            {
                int[] position = getUpPosition(currentLocation, step);
                String mapItem = map[position[0], position[1]];
                return getDictionary(position, mapItem);
            }
            catch
            {
                return null;
            }
        }
        private Dictionary<String, Object>? getDownElement(int[] currentLocation, int step) {
            try
            {
                int[] position = getDownPosition(currentLocation, step);
                String mapItem = map[position[0], position[1]];
                return getDictionary(position, mapItem);
            }
            catch
            {
                return null;
            }
        }
        private Dictionary<String, Object>? getRightElement(int[] currentLocation, int step) {
            try
            {
                int[] position = getRightPosition(currentLocation, step);
                String mapItem = map[position[0], position[1]];
                return getDictionary(position, mapItem);
            }
            catch
            {
                return null;
            }
        }
        private Dictionary<String, Object>? getLeftElement(int[] currentLocation, int step) {
            try
            {
                int[] position = getLeftPosition(currentLocation, step);
                String mapItem = map[position[0], position[1]];
                return getDictionary(position, mapItem);
            }
            catch
            {
                return null;
            }
        }

    }
}
