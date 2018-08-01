using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    class selectDifficulty : IState
{
    public string state
    {
        get;
        set;
    }

    public selectDifficulty()
    {
        state = "LEVEL";
    }

    public IState getState()
    {
        return this;
    }
}
}
