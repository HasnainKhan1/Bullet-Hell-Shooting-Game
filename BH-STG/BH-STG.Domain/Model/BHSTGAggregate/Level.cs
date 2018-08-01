using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    public class Level : IState
    {
        public string state
        {
            get;
            set;
        }

        public string LevelFileName { get; set; }

        public Level()
        {
            state = "LEVEL";
            LevelFileName = "LevelOne";
        }

        public Level(string fileName)
        {
            state = "LEVEL";
            LevelFileName = fileName;
        }

        public IState getState()
        {
            return this;
        }
    }
}
