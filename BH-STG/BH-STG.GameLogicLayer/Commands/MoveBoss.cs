using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHSTG.Domain.Model.BHSTGAggregate;
using BHSTG.SharedKernel.BaseDomainObjects;
using BHSTG.SharedKernel.Command;

namespace BHSTG.GameLogicLayer.Commands
{
    public class MoveBoss : Command
    {
        protected override void Execute(Entity<int> gameObject)
        {
            // Cast entity object to a GameBoss to use it's methods
            GameBoss boss = gameObject as GameBoss;

            // Execution steps
            

        }
    }
}
