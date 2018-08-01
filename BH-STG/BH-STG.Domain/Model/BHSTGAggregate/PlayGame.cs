using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    class PlayGame : IState
    {
        public string state
        {
            get;
            set;
        }

        public PlayGame()
        {
            state = "PLAYGAME";
        }

        public IState getState()
        {
            return this;
        }
    }
}
