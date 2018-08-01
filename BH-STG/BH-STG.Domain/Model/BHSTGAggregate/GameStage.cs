using System;
using System.Collections.Generic;
using System.Text;
using BHSTG.SharedKernel.GameDomainObjects;
using Microsoft.Xna.Framework;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    public class GameStage 
    {
        public DateTime LevelUpTimer { get; set; }
        public DateTime GameTime { get; set; }

        // TO DO: make value object
        //public LevelDifficulty LevelDifficulty { get; set; }  

        public int HighScore { get; set; }

        public int 
            Power { get; set; }
        public int CurrentPlayerScore { get; set; }


        //public sprite GameStageSprite { get; set; }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
