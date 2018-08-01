using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHSTG.SharedKernel.BaseDomainObjects
{
    public abstract class Observer
    {
        public abstract void Update(Entity<int> entityToRemove);
    }
}
