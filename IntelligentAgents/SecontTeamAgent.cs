using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentAgents
{
    internal class SecondTeamAgent : Agent
    {
        public SecondTeamAgent(int[] location) : base(location)
        {

        }
        internal bool chechIfTheCellHasRecource(string mapCell)
        {
            return !mapCell.Equals(Constants.NOTHING) && !mapCell.Equals(Constants.Village) && !mapCell.Equals(Constants.NOTHING);

        }

    }
}
