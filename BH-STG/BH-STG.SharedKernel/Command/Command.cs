using System;
using System.Collections.Generic;
using System.Text;
using BHSTG.SharedKernel.BaseDomainObjects;

namespace BHSTG.SharedKernel.Command
{
    public abstract class Command
    {
        protected Command()
        {

        }

        protected abstract void Execute(Entity<int> gameObject);
    }
}
