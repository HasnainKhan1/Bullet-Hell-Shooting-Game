using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHSTG.Domain.Model.BHSTGAggregate;
using BHSTG.SharedKernel.BaseDomainObjects;
using BHSTG.SharedKernel.Enums;
using BHSTG.SharedKernel.GameDomainObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BHSTG.SharedKernel.GameImages;

namespace BHSTG.Domain.Model.ValueObjects
{
    public class LaserBullet : BossBullet
    {
        double angle = 0;
        // Cast to a GameBoss entity
        //GameBoss gameBoss = ;
        int duration = 300;
        int tick = 0;

        public LaserBullet()
        {
            Active = true;
            Texture = GameArt.LaserBullet;
            Color = Color.LimeGreen * 1f;
            Orientation = 0f;
            Velocity = new Vector2(1f, 1f);
            BulletTexture = Texture;
            referencePoint = Position;
            //Position.X = referencePoint.X;
            //Position.Y = referencePoint.Y - Height;
            BulletBox = new Rectangle((int)referencePoint.X, (int)referencePoint.Y - Texture.Height, Texture.Width, Texture.Height);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //spriteBatch.Draw(Texture, Position, Color.White);
        }

        public override void Update(GameTime gameTime, List<Entity<int>> entities)
        {
            //set the reference points for the laser
          //  referencePoint = gameBoss.Position;
          //  Position.X = referencePoint.X + gameBoss.Texture.Width / 2;
          //  Position.Y = referencePoint.Y - Texture.Height;

            //BulletBox.Location = Position.ToPoint();


            //tick++;
            //if(tick >= duration)
            //{
            //    Active = false;
            //}

           // var move = MoveTowards(Position, new Vector2(400f, 800f));


            Position.X += (Velocity.X - .5f);
            Position.Y += (Velocity.Y + .5f);

        }

        private void MoveBullet()
        {
            
        }

        public override void MoveInPattern(Entity<int> boss)
        {
            // Cast to a GameBoss entity
            GameBoss gameBoss = boss as GameBoss;
            //only the final boss is dhooting lasers for now
            if (gameBoss.BossType == BossTypes.FinalBoss)
            {
                //laser is being shot 8 times from pi/2 angle
                float angleOfLaser = (float)Math.Atan((referencePoint.Y - Position.Y) / (referencePoint.X - Position.X));
                if (referencePoint.X >= Position.X)
                {
                    angleOfLaser += (float)(Math.PI / 2);
                }
            }
        }
    }

    public static class LaserBulletFactory
    {
        public static LaserBullet CreateLaserBullet(Vector2 spawnObjectPosition)
        {
            Vector2 bulletPosition = new Vector2(spawnObjectPosition.X,
                spawnObjectPosition.Y);


            return new LaserBullet()
            {
                Active = true,
                BulletTexture = GameArt.LaserBullet,
                BulletSpeed = 3f,
                Damage = 10,
                Orientation = 0f,
                Position = bulletPosition,
                Velocity = new Vector2((1f / 1000f), (1f / 1000f))
            };
        }
    }
}
