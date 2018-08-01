using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
    public class BlueBullet : BossBullet
    {
        

        public BlueBullet()
        {
            Texture = GameArt.BlueBullet;
            Color = Color.White * 1f;
            Orientation = 0f;
            Active = true;
            Velocity = new Vector2(1f, 1f);
            BulletTexture = GameArt.BlueBullet;
            referencePoint = Position;
            Active = true;
            BulletBox = new Rectangle((int)referencePoint.X, (int)referencePoint.Y - Texture.Height, Texture.Width, Texture.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Rectangle source = new Rectangle(frameIndex * frameWidth, 0, frameWidth, frameHeight);
            //Vector2 center = new Vector2(frameWidth / 2, frameHeight);
            //spriteBatch.Draw(BulletTexture, Position, source, Color.Blue, 0, center, 1, SpriteEffects.None, 0);

            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime, List<Entity<int>> entities)
        {
            //Position.Y = Position.Y - BulletSpeed;
            //BulletBox.Location = new Vector2(Position.X, Position.Y).ToPoint();

            //BulletBox.X += (int)Position.X;
            //BulletBox.Y += (int)Position.Y;

            //time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            //while (time > frameTime)
            //{
            //    frameIndex++;
            //    time = 0;
            //}
            //if (frameIndex > totalFrames)
            //{
            //    frameIndex = 1;
            //}

            Position.X += Velocity.X;
            Position.Y += Velocity.Y;


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

                    Position.X += (int) speedX;
                    Position.Y += (int) speedY;
                }
                
            }
            else if (gameBoss.BossType == BossTypes.FinalBoss)
            {
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

    public static class BlueBulletFactory
    {
        public static BlueBullet CreateBlueBullet(Vector2 spawnObjectPosition)
        {
            Vector2 bulletPosition = new Vector2(spawnObjectPosition.X, 
                spawnObjectPosition.Y);
           

            return new BlueBullet()
            {
                Active = true,
                BulletTexture = GameArt.BlueBullet,
                BulletSpeed = 3f,
                Damage = 10,
                Orientation = 0f,
                Position = bulletPosition,
                Velocity = new Vector2(1f, 1f)
            };
        }
    }
}
