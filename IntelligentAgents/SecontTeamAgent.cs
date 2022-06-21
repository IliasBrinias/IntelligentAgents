using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    internal class SecondTeamAgent : Agent
    {
        public SecondTeamAgent(String name, int[] location, int M) : base(name, location)
        {
            energyPoint = M;
            energyPotMultiplier = M/4;
        }
        internal bool chechIfTheCellHasRecource(string mapCell)
        {
            return !mapCell.Equals(Constants.NOTHING) && !mapCell.Equals(Constants.Village);

        }

    }
}
