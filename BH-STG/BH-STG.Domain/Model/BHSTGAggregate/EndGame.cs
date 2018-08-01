using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    class EndGame : IState
    {
        public string state
        {
            get;
            set;
        }

        public EndGame()
        {
            state = "ENDGAME";
        }

        public IState getState()
        {
            return this;
        }
    }
}
