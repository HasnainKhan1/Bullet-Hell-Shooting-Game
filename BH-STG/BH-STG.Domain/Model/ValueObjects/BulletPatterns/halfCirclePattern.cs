using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BHSTG.Domain.Model.BHSTGAggregate;
using BHSTG.SharedKernel.BaseDomainObjects;

namespace BHSTG.Domain.Model.ValueObjects.BulletPatterns
{
    public class halfCirclePattern : BulletPatterns
    {
        int speed;
        public halfCirclePattern(Texture2D newTexture, Vector2 newOrigin, int count)
        {
            texture = newTexture;
            origin = newOrigin;
            velocity = new Vector2(0, 1); //abstract variable velocity
            isActive = true;
            count = 5;

            bullets = new List<Bullet>(count);
            Double degreeAngle = 180;
            for (int i = 0; i < count; i++)
            {
                Bullet b = new Bullet(texture, this);
                b.angle = degreeAngle;

                Double angleAsRadians = (b.angle * Math.PI) / 180;

                Double x = origin.X;
                Double y = origin.Y;
                b.Position = new Vector2((float)x, (float)y);
                b.Velocity = new Vector2(0, 1);
                bullets.Add(b);
                b.Observers.Add(this);
                degreeAngle += 50;
            }
            isActive = true;
        }

        public halfCirclePattern(Texture2D newTexture, Vector2 newOrigin, int count, int newSpeed)
        {
            speed = newSpeed;
            texture = newTexture;
            origin = newOrigin;
            velocity = new Vector2(0, speed); //abstract variable velocity
            isActive = true;
            count = 5;

            bullets = new List<Bullet>(count);
            Double degreeAngle = 180;
            for (int i = 0; i < count; i++)
            {
                Bullet b = new Bullet(texture, this);
                b.angle = degreeAngle;

                Double angleAsRadians = (b.angle * Math.PI) / 180.0;

                Double x = origin.X;
                Double y = origin.Y;
                b.Position = new Vector2((float)x, (float)y);
                b.Velocity = new Vector2(0, 1);
                bullets.Add(b);
                b.Observers.Add(this);
                degreeAngle += 28;
            }
            isActive = true;
        }

        public override void Update(GameTime gameTime, List<Entity<int>> entities)
        {
            base.Update(gameTime, entities);
            foreach (var bullet in bullets)
            {
                bullet.angle += 1;
                Double radians = (bullet.angle * Math.PI) / -270;
                Double x = bullet.Position.X + Math.Cos(radians) * 1;
                Double y = bullet.Position.Y + Math.Sin(radians) * 1;

                bullet.Position = new Vector2((float)x, (float)y);
                if (Vector2.Distance(bullet.Position, spriteLocation) > 1000)
                {
                    bullet.isActive = false;
                }
            }
            foreach (var b in bullets)
            {
                b.Position.X += (1 * b.Velocity.X);
                b.Position.Y += (1 * b.Velocity.Y);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in bullets)
            {
                b.Draw(spriteBatch);
            }

        }

        public override int collision(Rectangle hitBox)
        {
            //if there is a collision run through the list and remove the bullet at to non active
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].hitBox.Intersects(hitBox) && bullets[i].isActive == true)
                {
                    bullets[i].isActive = false;
                    bullets.RemoveAt(i);
                    return i;
                }
            }
            return -1;
        }
    }
}
