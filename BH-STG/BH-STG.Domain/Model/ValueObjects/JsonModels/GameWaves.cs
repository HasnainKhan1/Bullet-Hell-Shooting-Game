using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

/* 
 *  i. The type of enemies to spawn; when/where/how/how many do they spawn, etc.
 *  ii. The type of movements of the enemies/bosses.
 *  iii. The type of bullets the enemies/bosses “fire”; when/where/how/how many do the
 *       bullets spawn, etc.
 *  iv. The type of movements of the bullets
 * 
 */

namespace BHSTG.Domain.Model.ValueObjects.JsonModels
{
    public partial class GameWaves
    {
        [JsonProperty("waves")]
        public Waves Waves { get; set; }
    }

    public class Waves
    {
        [JsonProperty("waveOne")]
        public Wave WaveOne { get; set; }

        [JsonProperty("waveThree")]
        public Wave WaveThree { get; set; }

        [JsonProperty("waveTwo")]
        public Wave WaveTwo { get; set; }

        //public List<Wave> Wave { get; set; }
    }

    public class Wave
    {
        [JsonProperty("enemyAmount")]
        public string EnemyAmount { get; set; }

        [JsonProperty("enemyBulletType")]
        public EnemyBulletType EnemyBulletType { get; set; }

        [JsonProperty("enemyMovement")]
        public EnemyMovement EnemyMovement { get; set; }

        [JsonProperty("enemyStartingPosition")]
        public EnemyStartingPosition EnemyStartingPosition { get; set; }

        [JsonProperty("enemyType")]
        public string EnemyType { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("interval")]
        public string Interval { get; set; }

        [JsonProperty("time")]
        public Time Time { get; set; }

        [JsonProperty("velocity")]
        public int Veloctiy { get; set; }
    }

    public partial class Time
    {
        [JsonProperty("end")]
        public string End { get; set; }

        [JsonProperty("start")]
        public string Start { get; set; }
    }

    public partial class EnemyStartingPosition
    {
        [JsonProperty("X")]
        public long X { get; set; }

        [JsonProperty("Y")]
        public long Y { get; set; }
    }

    public partial class EnemyMovement
    {
        [JsonProperty("pattern")]
        public string Pattern { get; set; }
    }

    public partial class EnemyBulletType
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("pattern")]
        public string Pattern { get; set; }

        [JsonProperty("speed")]
        public string Speed { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class GameWaves
    {
        public static GameWaves FromJson(string json) => JsonConvert.DeserializeObject<GameWaves>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GameWaves self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}
