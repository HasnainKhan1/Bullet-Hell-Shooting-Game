using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    public interface IState
    {
        string state
        {
            get;
            set;
        }

        IState getState();
    }
}
