using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    public class MainMenu : IState
    {
        public string state
        {
            get;
            set;
        }

        public MainMenu()
        {
            state = "MAINMENU";
        }

        public IState getState()
        {
            return this;
        }
    }
}
