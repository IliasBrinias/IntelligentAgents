using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    public class Constants
    {
        public static String WOOD = "W";
        public static String IRON = "I";
        public static String GOLD = "G";
        public static String CEREALS = "C";
        public static String NOTHING = " ";
        public static String ENERGY_POTS = "E";
        public static String NO_CARRY = "NC";
        public static String AGIENTS_DIED = "AD";
        public static String VILLAGE_WIN = "VW";

        public static String Village = "V";

        public static String getRandomResources()
        {
            switch (new Random().Next(0, 4))
            {
                case 0:
                    return Constants.CEREALS;
                case 1:
                    return Constants.GOLD;
                case 2:
                    return Constants.IRON;
                case 3:
                    return Constants.WOOD;
            }
            return Constants.WOOD;
        }
    }
}
