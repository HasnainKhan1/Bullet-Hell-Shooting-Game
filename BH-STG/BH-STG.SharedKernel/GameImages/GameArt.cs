using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BHSTG.SharedKernel.GameImages
{
    public static class GameArt
    {
        public static Texture2D MainPlayer { get; private set; }
        public static Texture2D StandardEnemies { get; private set; }
        public static Texture2D ButterflyEnemies { get; private set; }

        public static Texture2D FinalBoss { get; private set; }
        public static Texture2D RedBullet { get; private set; }
        public static Texture2D BlueBullet { get; private set; }
        public static Texture2D LaserBullet { get; private set; }
        public static Texture2D Pointer { get; private set; }
        public static Texture2D BulletSpawn { get; set; }
        public static Texture2D GameBackground { get; set; }

        public static Texture2D StarLives { get; set; }

        // Game Menu Buttons
        public static Texture2D StartGameButton { get; private set; }
        public static Texture2D LevelSelectButton { get; private set; }
        public static Texture2D InstructionsButton { get; private set; }
        public static Texture2D GameOverBackground { get; private set; }

        // In Game Buttons
        public static Texture2D InGamePlayButton { get; private set; }
        public static Texture2D InGameRestartButton { get; private set; }
        public static Texture2D InGameMainMenuButton { get; private set; }

        // Level Select Buttons
        public static Texture2D Level1 { get; private set; }
        public static Texture2D Level2 { get; private set; }
        public static Texture2D Level3 { get; private set; }
        public static Texture2D SelectLevel { get; private set; }
        

        public static void Load(Game root)
        {
            MainPlayer = root.Content.Load<Texture2D>("GameArt/sship60");
            StandardEnemies = root.Content.Load<Texture2D>("GameArt/demontwo");
            ButterflyEnemies = root.Content.Load<Texture2D>("GameArt/cherrybomb");
            FinalBoss = root.Content.Load<Texture2D>("GameArt/flying-demon");
            Pointer = root.Content.Load<Texture2D>("GameArt/Pointer");

            // Bullet textures and spawn point textures
            BulletSpawn = root.Content.Load<Texture2D>("GameArt/Wanderer");
            BlueBullet = root.Content.Load<Texture2D>("GameArt/Bullet");
            RedBullet = root.Content.Load<Texture2D>("GameArt/Bullet");
            LaserBullet = root.Content.Load<Texture2D>("GameArt/Bullet");
            GameBackground = root.Content.Load<Texture2D>("GameArt/spaceBackground");

            StarLives = root.Content.Load<Texture2D>("GameArt/red-star-small");

            // GameButtons
            StartGameButton = root.Content.Load<Texture2D>("GameArt/start-game-button");
            LevelSelectButton = root.Content.Load<Texture2D>("GameArt/level-select-button");
            InstructionsButton = root.Content.Load<Texture2D>("GameArt/instructions-button");

            // In Game Buttons
            InGamePlayButton = root.Content.Load<Texture2D>("GameArt/play-game");
            InGameMainMenuButton = root.Content.Load<Texture2D>("GameArt/main-menu");
            InGameRestartButton = root.Content.Load<Texture2D>("GameArt/restart-game");

            GameOverBackground = root.Content.Load<Texture2D>("GameArt/gameover_background");

            // Level Buttons
            Level1 = root.Content.Load<Texture2D>("GameArt/level1");
            Level2 = root.Content.Load<Texture2D>("GameArt/level2");
            Level3 = root.Content.Load<Texture2D>("GameArt/level3");
            SelectLevel = root.Content.Load<Texture2D>("GameArt/select-level");


        }


    }
}
