using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    class PauseGame : IState
    {
        public string state
        {
            get;
            set;
        }

        public PauseGame()
        {
            state = "PAUSEGAME";
        }

        public IState getState()
        {
            return this;
        }
    }
}
