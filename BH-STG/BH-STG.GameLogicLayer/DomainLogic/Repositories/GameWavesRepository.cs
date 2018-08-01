using System;
using System.Collections.Generic;
using System.IO;
using BHSTG.Domain.Interfaces;
using BHSTG.Domain.Model.BHSTGAggregate;
using BHSTG.Domain.Model.ValueObjects.BulletPatterns;
using BHSTG.Domain.Model.ValueObjects.JsonModels;
using BHSTG.GameLogicLayer.Factory;
using BHSTG.GameLogicLayer.Factory.ConcreteFactories;
using BHSTG.SharedKernel.BaseDomainObjects;
using BHSTG.SharedKernel.Enums.BHSTG.SharedKernel.Enums;
using BHSTG.SharedKernel.GameImages;
using Microsoft.Xna.Framework;

/* 
 *  i. The type of enemies to spawn; when/where/how/how many do they spawn, etc.
 *  ii. The type of movements of the enemies/bosses.
 *  iii. The type of bullets the enemies/bosses “fire”; when/where/how/how many do the
 *       bullets spawn, etc.
 *  iv. The type of movements of the bullets
 * 
 */

namespace BHSTG.GameLogicLayer.DomainLogic.Repositories
{
    public class GameWavesRepository : IParseJson
    {
        private readonly string _fileName;
        private const string FileExtension = ".json";

        public List<Entity<int>> Entities { get; set; }

        public List<GameWaves> Waves;
        public static GameFactory GameFactory { get; set; }

        public GameWavesRepository(Game root, string fileName)
        {
            Waves = new List<GameWaves>();
            GameFactory = new BHSTGFactory(root);
            _fileName = fileName;
            Entities = new List<Entity<int>>();
        }

        public void ReadGameLevels()
        {
            using (StreamReader r = new StreamReader("../../../../../BH-STG.SharedKernel\\GameLevelScripts\\"
                + _fileName + FileExtension))
            {
                var json = r.ReadToEnd();
                Waves.Add(ConvertJsonToGameWave(json));
            }
        }

        public List<float> GetWaveStartTimes()
        {
            List<float> waveTimeFrames = new List<float>();

            Wave wOne = Waves[0].Waves.WaveOne;
            Wave wTwo = Waves[0].Waves.WaveTwo;
            Wave wThree = Waves[0].Waves.WaveThree;

            waveTimeFrames.Add(float.Parse(wOne.Time.Start) / 100);
            waveTimeFrames.Add(float.Parse(wTwo.Time.Start) / 100);
            waveTimeFrames.Add(float.Parse(wThree.Time.Start) / 100);

            return waveTimeFrames;
        }

        public List<float> GetWaveEndTimes()
        {
            List<float> waveTimeFrames = new List<float>();

            Wave wOne = Waves[0].Waves.WaveOne;
            Wave wTwo = Waves[0].Waves.WaveTwo;
            Wave wThree = Waves[0].Waves.WaveThree;

            waveTimeFrames.Add(float.Parse(wOne.Time.End) / 100);
            waveTimeFrames.Add(float.Parse(wTwo.Time.End) / 100);
            waveTimeFrames.Add(float.Parse(wThree.Time.End) / 100);

            return waveTimeFrames;
        }

        // get the time of each wave
        public List<float> GetWaveTimeFrame()
        {
            List<float>waveTimeFrames = new List<float>();

            Wave wOne = Waves[0].Waves.WaveOne;
            Wave wTwo = Waves[0].Waves.WaveTwo;
            Wave wThree = Waves[0].Waves.WaveThree;

            waveTimeFrames.Add(float.Parse(wOne.Time.End) - float.Parse(wOne.Time.Start));
            waveTimeFrames.Add(float.Parse(wTwo.Time.End) - float.Parse(wTwo.Time.Start));
            waveTimeFrames.Add(float.Parse(wThree.Time.End) - float.Parse(wThree.Time.Start));

            return waveTimeFrames;

            //Wave tempWave;

            //switch (_fileName)
            //{
            //    case "LevelOne":
            //        tempWave = Waves[0].Waves.WaveOne;
            //        break;
            //    case "LevelTwo":
            //        tempWave = Waves[0].Waves.WaveTwo;
            //        break;
            //    case "LevelThree":
            //        tempWave = Waves[0].Waves.WaveThree;
            //        break;
            //}

            //return tempWave;
        }

        public List<float> GetIntervals()
        {
            List<float> intervals = new List<float>();

            Wave wOne = Waves[0].Waves.WaveOne;
            Wave wTwo = Waves[0].Waves.WaveTwo;
            Wave wThree = Waves[0].Waves.WaveThree;

            intervals.Add(float.Parse((float.Parse(wOne.Time.Start) / 1000) + wOne.Interval) / 100);
            intervals.Add(float.Parse((float.Parse(wTwo.Time.Start) / 1000) + wTwo.Interval) / 100);
            intervals.Add(float.Parse((float.Parse(wThree.Time.Start) / 1000) + wThree.Interval) / 100);

            return intervals;
        }


        public void SetGameLevels()
        {
            var mainPlayer = GameFactory.CreateMainPlayer();
            Entities.Add(mainPlayer);


            // TO DO: Parse game stage info and set
            //        Parse game time info and set
            //        Parse interval amount and set


            //set the enemy / boss properties from JSON
            foreach (var waves in Waves)
            {
                List<Wave> tempWaveList = new List<Wave>
                {
                    waves.Waves.WaveOne,
                    waves.Waves.WaveTwo,
                    waves.Waves.WaveThree
                };

                foreach (var wave in tempWaveList)
                {
                    if (wave.EnemyType == "StandardEnemy")
                    {
                        var bulletType = wave.EnemyBulletType;
                        var movementType = wave.EnemyMovement;
                        var sE = GameFactory.CreateStandardEnemy(movementType.Pattern, bulletType.Pattern, Int32.Parse(wave.EnemyAmount));

                        foreach (var se in sE)
                        {
                            Entities.Add(se);
                        }

                   
                    }
                    else if (wave.EnemyType == "Butterfly")
                    {
                        var bulletType = wave.EnemyBulletType;
                        var butterfly =
                            GameFactory.CreateButterflyEnemy(bulletType.Pattern,
                                Int32.Parse(wave.EnemyBulletType.Amount));

                        Entities.Add(butterfly[0]);
                    }
                    else if (wave.EnemyType == "GameBoss")
                    {
                        var gBoss = GameFactory.CreateGameBoss();
                        Entities.Add(gBoss);
                    }

                    //for (int i = 0; i < Int32.Parse(wave.EnemyAmount); i++)
                    //{
                    //    var bulletType = wave.EnemyBulletType;
                    //    var movementType = wave.EnemyMovement;
                    //    //var waveDuration = wave.Time;

                    //    switch (wave.EnemyType)
                    //    {
                    //        /*case "StandardEnemy":
                    //            var sE = StandardEnemyFactory.Create(
                    //                GameArt.StandardEnemies,
                    //                new Vector2(wave.Veloctiy, wave.Veloctiy),
                    //                new Vector2(wave.EnemyStartingPosition.X, wave.EnemyStartingPosition.Y),
                    //                new Vector2(150f + (55f * i), 200f),
                    //                new List<MovePattern>
                    //                {
                    //                    (MovePattern) Enum.Parse(typeof(MovePattern), movementType.Pattern)
                    //                });

                    //            var bulletPattern = GameFactory.CreateBulletPattern(
                    //                Enum.Parse(typeof(BulletPatternFactory.PatternTypes), bulletType.Pattern),
                    //                new Vector2(wave.EnemyStartingPosition.X, wave.EnemyStartingPosition.Y));

                    //            sE.Bullets = new List<BulletPatterns>() {bulletPattern};

                    //            Entities.Add(sE);
                    //            break;*/
                    //        case "GameBoss":

 




                    //            break;
                    //    }
                    //}


                }
            }
        }

        public GameWaves ConvertJsonToGameWave(string json)
        {
            return GameWaves.FromJson(json);
        }
    }
}
