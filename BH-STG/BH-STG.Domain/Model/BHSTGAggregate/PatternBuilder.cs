using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHSTG.Domain.Model.ValueObjects.BulletPatterns;
namespace BHSTG.Domain.Model.BHSTGAggregate
{
    public abstract class PatternBuilder : BulletPatterns
    {
        public BulletPatterns bulletPatterns;
    }
}
