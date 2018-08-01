using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHSTG.Domain.Model.BHSTGAggregate;
using BHSTG.Domain.Model.ValueObjects;
using BHSTG.Domain.Model.ValueObjects.BulletPatterns;
using BHSTG.SharedKernel.Enums;
using BHSTG.SharedKernel.Enums.BHSTG.SharedKernel.Enums;
using BHSTG.SharedKernel.GameImages;
using BHSTG.SharedKernel.GameDomainObjects;
using Microsoft.Xna.Framework;

namespace BHSTG.GameLogicLayer.Factory.ConcreteFactories
{
    public class BHSTGFactory : GameFactory
    {
        public Game GameObject { get; set; }

        private static Random _rand = new Random();

        private Vector2 GameBossStartingPosition { get; set; }

        public BHSTGFactory(Game gameObject)
        {
            GameObject = gameObject;
            GameBossStartingPosition = new Vector2(100, 100);
        }

        public override GameBoss CreateGameBoss()
        {
            var Boss = GameBossFactory.Create(
                GameArt.FinalBoss,
                new Vector2(1f, 1f),
                new Vector2(100f, 100f),
                BossTypes.FinalBoss,
                new List<BulletPatterns>()
                {
                    CreateBulletPattern(BulletPatternFactory.PatternTypes.Circular, GameBossStartingPosition),
                    CreateBulletPattern(BulletPatternFactory.PatternTypes.HalfCircle, GameBossStartingPosition),
                    CreateBulletPattern(BulletPatternFactory.PatternTypes.Player, GameBossStartingPosition)
                }
            );

            Boss.Start = Boss.Position;

            return Boss;
        }

        public override BulletSpawn CreateBulletSpawn()
        {

            int sideBoundLeft = 40;
            int sideBoundRight = 400;
            int topBound = 40;
            int bottomBound = 200;


            float xPosition = (float)_rand.Next(sideBoundLeft, sideBoundRight);
            float yPosition = (float)_rand.Next(topBound, bottomBound);

            return BulletSpawnFactory.Create(
                new Vector2(xPosition, yPosition),
                Color.White * 1f,
                5f,
                new List<BossBullet>()
                {
                    BlueBulletFactory.CreateBlueBullet(new Vector2(xPosition, yPosition)),
                    BlueBulletFactory.CreateBlueBullet(new Vector2(xPosition, yPosition)),
                    RedBulletFactory.CreateRedBullet(new Vector2(xPosition, yPosition)),
                    RedBulletFactory.CreateRedBullet(new Vector2(xPosition, yPosition)),
                    LaserBulletFactory.CreateLaserBullet(new Vector2(xPosition, yPosition)),
                    LaserBulletFactory.CreateLaserBullet(new Vector2(xPosition, yPosition))
                },
                new Vector2(1f, 1f),
                GameArt.BulletSpawn
                );
        }

        public override BulletPatterns CreateBulletPattern(object type, Vector2 currentPosition)
        {
            int sideBoundLeft = 40;
            int sideBoundRight = 400;
            int topBound = 40;
            int bottomBound = 200;


            float xPosition = (float)_rand.Next(sideBoundLeft, sideBoundRight);
            float yPosition = (float)_rand.Next(topBound, bottomBound);

            BulletPatterns temp = new CircularPattern(GameArt.RedBullet, currentPosition, 12);

            switch (type)
            {
                case BulletPatternFactory.PatternTypes.Circular:
                    temp = BulletPatternFactory.CreateCirclePattern(currentPosition, GameArt.RedBullet);
                    break;
                case BulletPatternFactory.PatternTypes.HalfCircle:
                    temp = BulletPatternFactory.CreateHalfCirclePattern(currentPosition, GameArt.BlueBullet);
                    break;
                case BulletPatternFactory.PatternTypes.Player:
                    temp = BulletPatternFactory.CreatePlayerBulletPattern(currentPosition, GameArt.LaserBullet);
                    break;
                case BulletPatternFactory.PatternTypes.Single:
                    temp = BulletPatternFactory.CreateSingleBulletPattern(currentPosition, GameArt.BlueBullet);
                    break;
                case BulletPatternFactory.PatternTypes.Random:
                    temp = BulletPatternFactory.CreateRandomBulletPattern(currentPosition, GameArt.BlueBullet);
                    break;
            }

            return temp;
        }

        //public BulletPatterns CreateBulletPattern(BulletPatternFactory.PatternTypes type, Vector2 Location)
        //{
        //    BulletPatterns temp = new Str
        //}



        public override GameStage CreateGameStage()
        {
            throw new NotImplementedException();
        }

        public override MainPlayer CreateMainPlayer()
        {
            //var player = MainPlayerFactory.CreateMainPlayer(

            //    //new List<BulletPatterns>()
            //    //{
            //    //    CreateBulletPattern(BulletPatternFactory.PatternTypes.Player)
            //    //}
            //    //);

            //return player;
            return MainPlayerFactory.CreateMainPlayer();
        }

        public override PlayerBullet CreatePlayerBullet(Vector2 playerPos)
        {
            return PlayerBulletFactory.CreatePlayerBullet(playerPos);
        }

        public override List<StandardEnemy> CreateStandardEnemyHorizontal(string bulletType, int num)
        {
            List<StandardEnemy> enemies = new List<StandardEnemy>();

            for (int i = 0; i < num; i++)
            {
                var sEnemy = StandardEnemyFactory.Create(
                GameArt.StandardEnemies,
                new Vector2(4f, 4f),
                new Vector2(10f + (100f * i), -10f - (30f * i)),
                new Vector2(150f + (55f * i), 200f),
                new List<MovePattern> { MovePattern.Straight, MovePattern.Delay, MovePattern.Straight }
                //new List<BulletPatterns>() {  }
               );

                if (bulletType != "")
                {
                    BulletPatternFactory.PatternTypes bType = BulletPatternFactory.PatternTypes.Single;
                    if (bulletType == "Circular") bType = BulletPatternFactory.PatternTypes.Circular;
                    else if (bulletType == "Signle") bType = BulletPatternFactory.PatternTypes.Single;
                    else if (bulletType == "HalfCircle") bType = BulletPatternFactory.PatternTypes.HalfCircle;
                    else if (bulletType == "Random") bType = BulletPatternFactory.PatternTypes.Random;

                    sEnemy.Bullets = new List<BulletPatterns>
                    {
                    CreateBulletPattern(bType, sEnemy.Position)
                    //CreateBulletPattern(BulletPatternFactory.PatternTypes.HalfCircle, oneEnemy.Position),
                    //CreateBulletPattern(BulletPatternFactory.PatternTypes.Random, oneEnemy.Position)
                    };
                }
                //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Single, sEnemy.Position));
                //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Single, sEnemy.Position));

                enemies.Add(sEnemy);
            }
            return enemies;

        }
        public override List<StandardEnemy> CreateStandardEnemyHorizontalShoot(string bulletType, int num)
        {
            List<StandardEnemy> enemies = new List<StandardEnemy>();

            for (int i = 0; i < 3; i++)
            {
                var sEnemy = StandardEnemyFactory.Create(
                GameArt.StandardEnemies,
                new Vector2(4f, 4f),
                new Vector2(400f + (50f * i), -10f),
                new Vector2(400f + (50f * i), 200f),
                new List<MovePattern> { MovePattern.Straight, MovePattern.Delay, MovePattern.Straight }
                //new List<BulletPatterns>() { }
               );
                if (bulletType != "")
                {
                    BulletPatternFactory.PatternTypes bType = BulletPatternFactory.PatternTypes.Single;
                    if (bulletType == "Circular") bType = BulletPatternFactory.PatternTypes.Circular;
                    else if (bulletType == "Signle") bType = BulletPatternFactory.PatternTypes.Single;
                    else if (bulletType == "HalfCircle") bType = BulletPatternFactory.PatternTypes.HalfCircle;
                    else if (bulletType == "Random") bType = BulletPatternFactory.PatternTypes.Random;

                    sEnemy.Bullets = new List<BulletPatterns>
                    {
                    CreateBulletPattern(bType, sEnemy.Position)
                    //CreateBulletPattern(BulletPatternFactory.PatternTypes.HalfCircle, oneEnemy.Position),
                    //CreateBulletPattern(BulletPatternFactory.PatternTypes.Random, oneEnemy.Position)
                    };
                }
                //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Single, sEnemy.Position));
                //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Single, sEnemy.Position));

                enemies.Add(sEnemy);
            }
            for (int i = 0; i < (num - 3) / 2; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        if (k == 1)
                        {
                            var sEnemy = StandardEnemyFactory.Create(
                           GameArt.StandardEnemies,
                           new Vector2(4f, 4f),
                           new Vector2(450f + ((50f * i + 50f * j) * (-1f)), -40f - (30f * i)),
                           new Vector2(450f + ((50f * i + 50f * j) * (-1f)), 200f),
                           new List<MovePattern> { MovePattern.Straight, MovePattern.Delay, MovePattern.Straight }
                           //new List<BulletPatterns>() { }
                          );
                            if (bulletType != "")
                            {
                                BulletPatternFactory.PatternTypes bType = BulletPatternFactory.PatternTypes.Single;
                                if (bulletType == "Circular") bType = BulletPatternFactory.PatternTypes.Circular;
                                else if (bulletType == "Signle") bType = BulletPatternFactory.PatternTypes.Single;
                                else if (bulletType == "HalfCircle") bType = BulletPatternFactory.PatternTypes.HalfCircle;
                                else if (bulletType == "Random") bType = BulletPatternFactory.PatternTypes.Random;

                                sEnemy.Bullets = new List<BulletPatterns>
                                {
                                CreateBulletPattern(bType, sEnemy.Position)
                                //CreateBulletPattern(BulletPatternFactory.PatternTypes.HalfCircle, oneEnemy.Position),
                                //CreateBulletPattern(BulletPatternFactory.PatternTypes.Random, oneEnemy.Position)
                                };
                            }
                            //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Single, sEnemy.Position));
                            //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Single, sEnemy.Position));
                            enemies.Add(sEnemy);
                        }
                        else
                        {
                            var sEnemy = StandardEnemyFactory.Create(
                            GameArt.StandardEnemies,
                            new Vector2(4f, 4f),
                            new Vector2(450f + (50f * i + 50f * j), -40f - (30f * i)),
                            new Vector2(450f + (50f * i + 50f * j), 200f),
                            new List<MovePattern> { MovePattern.Straight, MovePattern.Delay, MovePattern.Straight }
                            //new List<BulletPatterns>() { }
                           );

                            if (bulletType != "")
                            {
                                BulletPatternFactory.PatternTypes bType = BulletPatternFactory.PatternTypes.Single;
                                if (bulletType == "Circular") bType = BulletPatternFactory.PatternTypes.Circular;
                                else if (bulletType == "Signle") bType = BulletPatternFactory.PatternTypes.Single;
                                else if (bulletType == "HalfCircle") bType = BulletPatternFactory.PatternTypes.HalfCircle;
                                else if (bulletType == "Random") bType = BulletPatternFactory.PatternTypes.Random;

                                sEnemy.Bullets = new List<BulletPatterns>
                                {
                                CreateBulletPattern(bType, sEnemy.Position)
                                //CreateBulletPattern(BulletPatternFactory.PatternTypes.HalfCircle, oneEnemy.Position),
                                //CreateBulletPattern(BulletPatternFactory.PatternTypes.Random, oneEnemy.Position)
                                };
                            }
                            //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Single, sEnemy.Position));
                            //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Single, sEnemy.Position));
                            enemies.Add(sEnemy);
                        }
                    }
                }

            }
            return enemies;
        }
        public override List<StandardEnemy> CreateStandardEnemyQuaterRight(string bulletType, int num)
        {
            List<StandardEnemy> enemies = new List<StandardEnemy>();
            Random _rand = new Random();
            float startX = (float)_rand.Next((int)20f, (int)600f);
            for (int i = 0; i < num; i++)
            {
                var sEnemy = StandardEnemyFactory.Create(
                GameArt.StandardEnemies,
                new Vector2(4f, 4f),
                new Vector2(startX, -10f - (80f * i)),
                new Vector2(900f, 250f),
                new List<MovePattern> { MovePattern.QuarterTurnRight }
               //new List<BulletPatterns>() {  }
               );
                if (bulletType != "")
                {
                    BulletPatternFactory.PatternTypes bType = BulletPatternFactory.PatternTypes.Single;
                    if (bulletType == "Circular") bType = BulletPatternFactory.PatternTypes.Circular;
                    else if (bulletType == "Signle") bType = BulletPatternFactory.PatternTypes.Single;
                    else if (bulletType == "HalfCircle") bType = BulletPatternFactory.PatternTypes.HalfCircle;
                    else if (bulletType == "Random") bType = BulletPatternFactory.PatternTypes.Random;

                    sEnemy.Bullets = new List<BulletPatterns>
                    {
                    CreateBulletPattern(bType, sEnemy.Position)
                    //CreateBulletPattern(BulletPatternFactory.PatternTypes.HalfCircle, oneEnemy.Position),
                    //CreateBulletPattern(BulletPatternFactory.PatternTypes.Random, oneEnemy.Position)
                    };
                }
                //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Single, sEnemy.Position));
                //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Random, sEnemy.Position));
                enemies.Add(sEnemy);
            }
            return enemies;
        }
        public override List<StandardEnemy> CreateStandardEnemyQuaterLeft(string bulletType, int num)
        {
            List<StandardEnemy> enemies = new List<StandardEnemy>();
            Random _rand = new Random();
            float startX = (float)_rand.Next((int)20f, (int)600f);
            for (int i = 0; i < num; i++)
            {
                var sEnemy = StandardEnemyFactory.Create(
                GameArt.StandardEnemies,
                new Vector2(4f, 4f),
                new Vector2(startX, -10f - (80f * i)),
                new Vector2(-100f, 250f),
                new List<MovePattern> { MovePattern.QuarterTurnLeft }
               //new List<BulletPatterns>() {  }
               );
                if (bulletType != "")
                {
                    BulletPatternFactory.PatternTypes bType = BulletPatternFactory.PatternTypes.Single;
                    if (bulletType == "Circular") bType = BulletPatternFactory.PatternTypes.Circular;
                    else if (bulletType == "Signle") bType = BulletPatternFactory.PatternTypes.Single;
                    else if (bulletType == "HalfCircle") bType = BulletPatternFactory.PatternTypes.HalfCircle;
                    else if (bulletType == "Random") bType = BulletPatternFactory.PatternTypes.Random;

                    sEnemy.Bullets = new List<BulletPatterns>
                    {
                    CreateBulletPattern(bType, sEnemy.Position)
                    //CreateBulletPattern(BulletPatternFactory.PatternTypes.HalfCircle, oneEnemy.Position),
                    //CreateBulletPattern(BulletPatternFactory.PatternTypes.Random, oneEnemy.Position)
                    };
                }
                //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Single, sEnemy.Position));
                //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Random, sEnemy.Position));
                enemies.Add(sEnemy);
            }
            return enemies;

        }
        public override List<StandardEnemy> CreateStandardEnemyQuater(float x, int num, MovePattern mp)
        {
            List<StandardEnemy> enemies = new List<StandardEnemy>();
            if (mp == MovePattern.QuarterTurnRight)
            {
                for (int i = 0; i < num; i++)
                {
                    var sEnemy = StandardEnemyFactory.Create(
                    GameArt.StandardEnemies,
                    new Vector2(4f, 4f),
                    new Vector2(x, -10f - (80f * i)),
                    new Vector2(900f, 250f),
                    new List<MovePattern> { MovePattern.QuarterTurnRight }
                    //new List<BulletPatterns>() {  }
                   );

                    //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Single, sEnemy.Position));
                    //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Random, sEnemy.Position));
                    enemies.Add(sEnemy);
                }
            }
            else if (mp == MovePattern.QuarterTurnLeft)
            {
                for (int i = 0; i < num; i++)
                {
                    var sEnemy = StandardEnemyFactory.Create(
                    GameArt.StandardEnemies,
                    new Vector2(4f, 4f),
                    new Vector2(x, -10f - (80f * i)),
                    new Vector2(-100f, 250f),
                    new List<MovePattern> { MovePattern.QuarterTurnLeft }
                    //new List<BulletPatterns>() {  }
                   );

                    //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Single, sEnemy.Position));
                    //sEnemy.Bullets.Add(CreateBulletPattern(BulletPatternFactory.PatternTypes.Random, sEnemy.Position));
                    enemies.Add(sEnemy);
                }
            }

            return enemies;
        }
        public override List<StandardEnemy> CreateButterflyEnemy(string bulletType, int num)
        {
            List<StandardEnemy> bEnemy = new List<StandardEnemy>();
            var oneEnemy = StandardEnemyFactory.Create(
                    GameArt.ButterflyEnemies,
                    new Vector2(1.5f, 2f),
                    new Vector2(_rand.Next(150, 750), -10f),
                    new Vector2(1f, 1f),
                    new List<MovePattern> { MovePattern.Butterfly }
                    //new List<BulletPatterns>() {  }
                );


            if (bulletType != "")
            {
                BulletPatternFactory.PatternTypes bType = BulletPatternFactory.PatternTypes.Single;
                if (bulletType == "Circular") bType = BulletPatternFactory.PatternTypes.Circular;
                else if (bulletType == "Signle") bType = BulletPatternFactory.PatternTypes.Single;
                else if (bulletType == "HalfCircle") bType = BulletPatternFactory.PatternTypes.HalfCircle;
                else if (bulletType == "Random") bType = BulletPatternFactory.PatternTypes.Random;

                oneEnemy.Bullets = new List<BulletPatterns>
                {
                CreateBulletPattern(bType, oneEnemy.Position)
                //CreateBulletPattern(BulletPatternFactory.PatternTypes.HalfCircle, oneEnemy.Position),
                //CreateBulletPattern(BulletPatternFactory.PatternTypes.Random, oneEnemy.Position)
                };
            }
            bEnemy.Add(oneEnemy);
            return bEnemy;
        }

        public override List<StandardEnemy> CreateStandardEnemy(string moveType, string bulletType, int num)
        {
            List<StandardEnemy> senemies = new List<StandardEnemy>();
            switch (moveType)
            {
                case "HorizontalShoot":
                    senemies = CreateStandardEnemyHorizontalShoot(bulletType, num);
                    break;
                case "QuarterTurnRight":
                    senemies = CreateStandardEnemyQuaterRight(bulletType, num);
                    break;
                case "QuarterTurnLeftt":
                    senemies = CreateStandardEnemyQuaterLeft(bulletType, num);
                    break;
                case "HorizontalLine":
                    senemies = CreateStandardEnemyHorizontal(bulletType, num);
                    break;
                case "Butterfly":
                    senemies = CreateButterflyEnemy(bulletType, num);
                    break;
                default:
                    break;
            }
            return senemies;
        }
        public override BlueBullet CreateBlueBullet()
        {
            return BlueBulletFactory.CreateBlueBullet(new Vector2(100f, 100f));
        }

        public override EnemyBullet CreateEnemyBullet()
        {
            throw new NotImplementedException();
        }

        public override LaserBullet CreateLaserBullet()
        {
            return LaserBulletFactory.CreateLaserBullet(new Vector2(100f, 100f));
        }

        public override RedBullet CreateRedBullet()
        {
            return RedBulletFactory.CreateRedBullet(new Vector2(100f, 100f));
        }
    }
}
