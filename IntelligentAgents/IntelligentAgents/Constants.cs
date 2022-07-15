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
        public static String Village = "V";
        public static String ENERGY_POTS = "E";

        public static String NO_CARRY = "NC";
        public static String AGIENTS_DIED = "AD";
        public static String VILLAGE_WIN = "VW";

        public static String getRandomResources(int index)
        {
            if (index % 100 > 50)
            {
                if (index % 50 > 25)
                {
                    return Constants.CEREALS;
                }
                else
                {
                    return Constants.GOLD;
                }
            }
            else
            {
                if (index % 50 > 25)
                {
                    return Constants.IRON;
                }
                else
                {
                    return Constants.WOOD;
                }
            }
        }
    }
}
