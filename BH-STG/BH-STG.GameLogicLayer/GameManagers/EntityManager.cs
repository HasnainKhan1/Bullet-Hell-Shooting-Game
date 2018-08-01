using BHSTG.SharedKernel.BaseDomainObjects;
using System.Collections.Generic;
using System.Linq;
using BHSTG.Domain.Model.BHSTGAggregate;
using BHSTG.Domain.Model.ValueObjects;
using BHSTG.GameLogicLayer.Factory;
using BHSTG.GameLogicLayer.Factory.ConcreteFactories;
using BHSTG.SharedKernel.Enums;
using BHSTG.SharedKernel.Enums.BHSTG.SharedKernel.Enums;
using BHSTG.SharedKernel.GameImages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using BHSTG.Domain.Interfaces;
using BHSTG.Domain.Model.ValueObjects.BulletPatterns;
using BHSTG.GameLogicLayer.DomainLogic.Repositories;

/*
 * This code takes care of queuing up any entities added during updating in the game in a separate list, 
 * and adds them after it finishes updating the existing entities to the current list.
 */

namespace BHSTG.GameLogicLayer.GameManagers
{
    public sealed class EntityManager : Observer
    {
        private static readonly EntityManager instance = new EntityManager();
        public EntityManager(){}

        public static List<Entity<int>> Entities = new List<Entity<int>>();
        public static List<Entity<int>> EntitiesFromJson = new List<Entity<int>>();


        public static bool IsUpdating;
        public static List<Entity<int>> AddedEntities = new List<Entity<int>>();
        public static List<Entity<int>> EntitiesToRemove = new List<Entity<int>>();
        public static TimeSpan normalEnemySpawnTime;
        public static TimeSpan enemySpawnTime;
        public static TimeSpan previousSpawnTime;
        public static TimeSpan playerStartNoDamage;
        public static TimeSpan playerInvulnerable;
        public static List<float> gameWaveTimes;
        public static List<float> enemyIntervals;
        public static List<float> waveStartTimes;
        public static List<float> waveEndTimes;

        public static bool playerStatusChecked;
        public static GameFactory GameFactory { get; set; }

        public static int Count => Entities.Count;

        public static string FileName { get; set; }

        
        public static Game GameRoot { get; set; }
        public static EntityManager Instance {
            get
            {
                return instance;
            }
        }

        public static GameWavesRepository WavesParser { get; set; }

        public static void InitializeGame(Game gameRoot, string filename)
        {
            GameRoot = gameRoot;
            GameFactory = new BHSTGFactory(gameRoot);
            GameArt.Load(gameRoot);

            
            playerInvulnerable = TimeSpan.FromSeconds(2.0f);
            playerStatusChecked = false;

            
            // TO DO: from the main menu, if level selector is chosen, pass
            // the file name to the entity manager here.
            //FileName = "LevelOne";

            // Parse/setup waves for the game from JSON files
            WavesParser = new GameWavesRepository(gameRoot, filename);
            WavesParser.ReadGameLevels();

            WavesParser.SetGameLevels();

            EntitiesFromJson = WavesParser.Entities;

            // Add observer to each of the entities
            foreach (var entity in EntitiesFromJson)
                entity.Add(Instance);

            // HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE HERE
            // Get game wave time based on the file name passed when creating a new game wave repository
            gameWaveTimes = WavesParser.GetWaveTimeFrame();
            enemyIntervals = WavesParser.GetIntervals();
            waveStartTimes = WavesParser.GetWaveStartTimes();
            waveEndTimes = WavesParser.GetWaveEndTimes();
            

            //Console.WriteLine("GAME WAVE TIMES: {0}, {1}, {2}", gameWaveTime[0], gameWaveTime[1], gameWaveTime[2]);
            //Console.WriteLine("GAME WAVE TIMES: {0}, {1}, {2}", enemyIntervals[0], enemyIntervals[1], enemyIntervals[2]);

            // TO DO: move the entities created from the JSON file 
            // to an active list of entities based on the 
        }

        public void SetFileName(string filename)
        {
            FileName = filename;
        }

        public static void Add(Entity<int> entity)
        {
            if (!IsUpdating)
            {
                switch (entity)
                {
                    case GameBoss gb:
                        gb = GameFactory.CreateGameBoss();
                        gb.Add(Instance);
                        Entities.Add(gb);
                        break;
                    case BulletSpawn bs:
                        bs = GameFactory.CreateBulletSpawn();
                        bs.Add(Instance);
                        Entities.Add(bs);
                        break;
                    case MainPlayer mp:
                        mp = GameFactory.CreateMainPlayer();
                        Entities.Add(mp);
                        //move to front of list
                        //var temp = Entities[Entities.Count - 1];
                       // Entities[Entities.Count - 1] = Entities[0];
                       // Entities[0] = temp;
                        mp.Add(Instance);
                        break;
                    case StandardEnemy be:
                        var bList = GameFactory.CreateButterflyEnemy("Random", 1);
                        be = bList[0];
                        Entities.Add(be);
                        be.Add(Instance);
                        break;
                    case PlayerBullet pb:
                        pb = GameFactory.CreatePlayerBullet(Entities[0].Position);
                        Entities.Add(pb);
                        pb.Add(Instance);
                        break;
                        //case BulletPatterns bp:
                        //    bp = GameFactory.CreateBulletPattern(BulletPatternFactory.PatternTypes.Circular);
                        //    Entities.Add(bp);
                        //    break;
                }
            }
                
            else
                AddedEntities.Add(entity);
        }

        public static void AddStandardEnemyHorizontal(string bulletType, int num)
        {
            List<StandardEnemy> ses = GameFactory.CreateStandardEnemyHorizontal(bulletType, num);
            if (!IsUpdating)
            {
                foreach (var se in ses)
                {
                    Entities.Add(se);
                    se.Add(Instance);
                }
            }
            else
            {
                foreach (var se in ses)
                {
                    AddedEntities.Add(se);
                }
            }
        }
        public static void AddStandardEnemyHorizontalShoot(string bulletType, int num)
        {
            List<StandardEnemy> ses = GameFactory.CreateStandardEnemyHorizontalShoot(bulletType, num);
            if (!IsUpdating)
            {
                foreach (var se in ses)
                {
                    Entities.Add(se);
                    se.Add(Instance);
                }
            }
            else
            {
                foreach (var se in ses)
                {
                    AddedEntities.Add(se);
                }
            }
        }

        public static void AddStandardEnemyQuater(string bulletType, int num)
        {
            Random _rand = new Random();
            int direction = _rand.Next(2) % 2;
            List<StandardEnemy> ses = new List<StandardEnemy>();
            switch (direction)
            {
                case 0:
                    ses = GameFactory.CreateStandardEnemyQuaterRight("", 5);
                    break;
                case 1:
                    ses = GameFactory.CreateStandardEnemyQuaterLeft("", 5);
                    break;
                default:
                    break;
            }

            if (!IsUpdating)
            {
                foreach (var se in ses)
                {
                    Entities.Add(se);
                    se.Add(Instance);
                }
            }
            else
            {
                foreach (var se in ses)
                {
                    AddedEntities.Add(se);
                }
            }
        }
        public static void AddStandardEnemyQuater(float x, int num, MovePattern mp)
        {
            List<StandardEnemy> ses = GameFactory.CreateStandardEnemyQuater(x, num, mp);
            if (!IsUpdating)
            {
                foreach (var se in ses)
                {
                    Entities.Add(se);
                    se.Add(Instance);
                }
            }
            else
            {
                foreach (var se in ses)
                {
                    AddedEntities.Add(se);
                }
            }
        }

        public static void AddButterflyEnemy(string bulletType, int num)
        {
            List<StandardEnemy> bEnemy = GameFactory.CreateButterflyEnemy(bulletType, 1);
            if (!IsUpdating)
            {
                foreach (var be in bEnemy)
                {
                    Entities.Add(be);
                    be.Add(Instance);
                }
            }
            else
            {
                foreach (var be in bEnemy)
                {
                    AddedEntities.Add(be);
                }
            }
        }

        public static void ClearScreen()
        {
            Entities.Clear();
        }


        public static void Update(GameTime gameTime, float timer)
        {
            IsUpdating = true;

            //CleanList();

            foreach (var entity in Entities)
            {
              //  var collision = CheckCollision(entity);

                switch (entity)
                {
                    case MainPlayer mp:
                        //check if player has been hit.
                        if (mp.Invulnerable == true)
                        {
                            if (playerStatusChecked == false)
                            {
                                playerStatusChecked = true;
                                playerStartNoDamage = gameTime.TotalGameTime;
                            }
                            
                            if (gameTime.TotalGameTime - playerStartNoDamage >= playerInvulnerable)
                            {
                                mp.Invulnerable = false;
                                playerStatusChecked = false;
                            }

                        }
                        //Prevent player from going out of bounds
                        mp.Position.X = MathHelper.Clamp(entity.Position.X, GameArt.MainPlayer.Width, GameRoot.GraphicsDevice.Viewport.Width);
                        mp.Position.Y = MathHelper.Clamp(entity.Position.Y, 0, GameRoot.GraphicsDevice.Viewport.Height - GameArt.MainPlayer.Height);
                        break;
                    case StandardEnemy se:
                        if (se.Position.X > 0 && se.Position.X < GameRoot.GraphicsDevice.Viewport.Width && se.Position.Y > 0 && se.Position.Y < GameRoot.GraphicsDevice.Viewport.Height)
                        {
                            se.Onscreen = true;
                        }
                        else
                        {
                            //if onscreen is true, but we are now off screen, time to delete.
                            if (se.Onscreen == true)
                            {
                                se.Die(se);
                            }
                        }
                        break;
                    case GameBoss gb:
                        if (gb.Position.X > 0 && gb.Position.X < GameRoot.GraphicsDevice.Viewport.Width && gb.Position.Y > 0 && gb.Position.Y < GameRoot.GraphicsDevice.Viewport.Height)
                        {
                            gb.Onscreen = true;
                        }
                        else
                        {
                            //if onscreen is true, but we are now off screen, time to delete.
                            if (gb.Onscreen == true)
                            {
                                gb.Die(gb);
                            }
                        }
                        break;
                    case PlayerBullet pb:
                        //check if bullet has gone out of bounds
                        if (pb.Position.X > GameRoot.GraphicsDevice.Viewport.Width || pb.Position.Y < 0)
                        {
                            pb.Die(pb);
                        }
                        break;

                }



                entity.Update(gameTime, Entities);                
            }

            if (EntitiesToRemove != null)
            {
                foreach (var entity in EntitiesToRemove)
                {
                    Entities.Remove(entity);
                }

                EntitiesToRemove.Clear();
            }









            // IMPLEMENT NEXT TIME
            //// 5 seconds of loading - then start with phase one
            //// Phase1 from  3 - 48 seconds (45 seconds)
            //// normal enemies the whole time
            //if (gameTime.TotalGameTime.TotalSeconds == 3)
            //{
            //    Phase1();
            //}
            //else if (gameTime.TotalGameTime.TotalSeconds == 48)
            //{
            //    //for now clear enemies and bring out mid boss
            //    Entities.Clear();

            //    //Mid boss fight (technically, bullets from phase one do not clear)
            //    MidBoss();
            //}
            //else if (gameTime.TotalGameTime.TotalSeconds == 75)
            //{
            //    Entities.Clear();

            //    //regular enemies again
            //    Phase2();
            //}
            //else if (gameTime.TotalGameTime.TotalSeconds == 92)
            //{
            //    Entities.Clear();

            //    //There's a cutscene here before starting the boss battle.
            //}
            //else if (gameTime.TotalGameTime.TotalSeconds == 95)
            //{
            //    //B
            //}
            //if (gameTime.TotalGameTime.TotalSeconds == 15)
            //{
            //    Entities.Clear();
            //}

            IsUpdating = false;

            foreach (var entity in AddedEntities)
                Entities.Add(entity);

            AddedEntities.Clear();

            // remove any expired entities.
           // Entities = Entities.Where(x => x.IsActive()).ToList();
        }

        // IMPLEMENT LATER
        //private static void Phase1()
        //{
        //    //4 enemies from left

        //    //5 enemies from right

        //}

        //private static void Phase2()
        //{

        //}

        //private static void MidBoss()
        //{

        //}


        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var entity in Entities)
            {
                if (entity.IsActive())
                    entity.Draw(spriteBatch);
            }
                
        }

        /// <summary>
        /// This is the observer Pattern update function that alerts the observer (entity manager) that an entity has perished.
        /// </summary>
        /// <param name="entityToRemove"></param>
        public override void Update(Entity<int> entityToRemove)
        {
            EntitiesToRemove.Add(entityToRemove);
        }
    }
}
