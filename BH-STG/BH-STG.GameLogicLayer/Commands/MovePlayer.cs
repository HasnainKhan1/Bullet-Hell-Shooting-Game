using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHSTG.SharedKernel.BaseDomainObjects;
using BHSTG.SharedKernel.Command;
using BHSTG.SharedKernel.GameDomainObjects;

namespace BHSTG.GameLogicLayer.Commands
{
    public class MovePlayer : Command
    {
        protected override void Execute(Entity<int> gameObject)
        {
            // TO DO: Add movement execution logic here
        }
    }
}
