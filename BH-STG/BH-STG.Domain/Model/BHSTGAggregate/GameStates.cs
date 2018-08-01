using System.Collections.Generic;
using BHSTG.Domain.Model.BHSTGAggregate;
using BHSTG.SharedKernel.Enums.BHSTG.SharedKernel.Enums;
using BHSTG.SharedKernel.GameImages;
using BHSTG.Domain.Model.ValueObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using BHSTG.Domain.Model.ValueObjects.BulletPatterns;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    public class GameStates
    {
        public IState currentState;
        public string state;

        public GameStates()
        {
            currentState = new MainMenu();
            state = currentState.state;
        }

        public void changeToState(string newState)
        {
            switch (newState)
            {
                case "MAINMENU":
                    currentState = new MainMenu();                    
                    break;
                case "PLAYGAME":
                    currentState = new PlayGame();
                    break;
                case "PAUSEGAME":
                    currentState = new PauseGame();
                    break;
                case "ENDGAME":
                    currentState = new EndGame();
                    break;
                case "LEVEL":
                    currentState = new Level();
                    break;
                default:
                    break;
            }
            state = newState;
        }
    }
}
