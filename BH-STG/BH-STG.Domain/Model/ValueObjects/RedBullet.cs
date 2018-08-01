using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHSTG.Domain.Model.BHSTGAggregate;
using BHSTG.SharedKernel.BaseDomainObjects;
using BHSTG.SharedKernel.Enums;
using BHSTG.SharedKernel.GameImages;
using BHSTG.SharedKernel.GameDomainObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BHSTG.Domain.Model.ValueObjects
{
    public class RedBullet : BossBullet
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public RedBullet()
        {
            Texture = GameArt.RedBullet;
            Color = Color.Red * 1f;
            Active = true;
            Orientation = 0f;
            Velocity = new Vector2(1f, 1f);
            BulletTexture = GameArt.RedBullet;
            referencePoint = Position;
            Active = true;
            BulletBox = new Rectangle((int)referencePoint.X, (int)referencePoint.Y - Texture.Height, Texture.Width, Texture.Height);
        }
    
        public override void Update(GameTime gameTime, List<Entity<int>> entities)
        {
            //Position.X = Position.Y - BulletSpeed;
            //Position.Y = Position.X - BulletSpeed;
            ////ToDo: check if the bullet is still on the stage if not then make it invisible
            //BulletBox.X = (int)Position.X;
            //BulletBox.Y = (int)Position.Y;

            Position.X += Velocity.X + .5f;
            Position.Y += Velocity.Y - .5f;
        }

        public override void MoveInPattern(Entity<int> boss)
        {
            // Cast to a GameBoss entity
            GameBoss gameBoss = boss as GameBoss;

            if (gameBoss.BossType == BossTypes.MidBoss)
            {
                double angle = Math.PI;
                for (double theta = 0; theta < Math.PI * 2; theta += angle)
                {
                    double speedX = Math.Sin(theta);
                    double speedY = Math.Cos(theta);
                }
            }
            else if (gameBoss.BossType == BossTypes.FinalBoss)
            {
                // To Do: Final boss red bullet movement pattern logic
                // To Do: Final boss red bullet movement pattern logic
                //Circular motion
                double angle = (2 * Math.PI) / bulletsPerWave;
                for (double theta = 0; theta < (Math.PI * 2); theta += angle)
                {
                    double speedX = Math.Sin(theta);
                    double speedY = Math.Cos(theta);
                }
            }
        }
    }

    public static class RedBulletFactory
    {
        public static RedBullet CreateRedBullet(Vector2 spawnObjectPosition)
        {
            Vector2 bulletPosition = new Vector2(spawnObjectPosition.X,
                spawnObjectPosition.Y);


            return new RedBullet()
            {
                Active = true,
                BulletTexture = GameArt.RedBullet,
                BulletSpeed = 3f,
                Damage = 10,
                Orientation = 0f,
                Position = bulletPosition,
                Velocity = new Vector2(1f, 1f)
            };
        }
    }
}
