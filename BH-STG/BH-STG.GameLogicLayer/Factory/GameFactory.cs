using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHSTG.Domain.Model.BHSTGAggregate;
using BHSTG.Domain.Model.ValueObjects;
using BHSTG.Domain.Model.ValueObjects.BulletPatterns;
using BHSTG.SharedKernel.Enums.BHSTG.SharedKernel.Enums;
using Microsoft.Xna.Framework;

namespace BHSTG.GameLogicLayer.Factory
{
    public abstract class GameFactory
    {
        // Domain Entities
        public abstract BulletSpawn CreateBulletSpawn();

        public abstract GameBoss CreateGameBoss();
        public abstract GameStage CreateGameStage();
        public abstract MainPlayer CreateMainPlayer();
        /* public abstract StandardEnemy CreateStandardEnemy();
         public abstract StandardEnemy CreateStandardEnemy2(Vector2 init, Vector2 final, List<MovePattern> mps);*/
        public abstract List<StandardEnemy> CreateStandardEnemyHorizontal(string bulletType, int num);
        public abstract List<StandardEnemy> CreateStandardEnemyHorizontalShoot(string bulletType, int num);
        public abstract List<StandardEnemy> CreateStandardEnemyQuater(float x, int num, MovePattern mp);
        public abstract List<StandardEnemy> CreateButterflyEnemy(string bulletType, int num);
        public abstract List<StandardEnemy> CreateStandardEnemyQuaterRight(string bulletType, int num);
        public abstract List<StandardEnemy> CreateStandardEnemyQuaterLeft(string bulletType, int num);
        public abstract List<StandardEnemy> CreateStandardEnemy(string moveType, string bulletType, int num);

        public abstract PlayerBullet CreatePlayerBullet(Vector2 playerPos);
        // Domain Value Objects

        public abstract BulletPatterns CreateBulletPattern(object type, Vector2 currentPosition);
        public abstract BlueBullet CreateBlueBullet();

        public abstract EnemyBullet CreateEnemyBullet();
        public abstract LaserBullet CreateLaserBullet();
        public abstract RedBullet CreateRedBullet();
    }
}
