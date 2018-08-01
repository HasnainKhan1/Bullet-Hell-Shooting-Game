using System;
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
    public class CircularPattern : BulletPatterns
    {
        int count;
        Double x;
        Double y;
        int speed;

        public CircularPattern(Texture2D newTexture, Vector2 Origin, int Count)
        {
            velocity = new Vector2(0, 1);
            //Texture = newTexture;
            texture = newTexture;
            origin = Origin;
            this.count = Count;
            bullets = new List<Bullet>(this.count);
            isActive = true;
            //Active = true;
            //angle
            double angle = 0;
            double move = 360 / count;
            Double deviation = 1;

            for(int i = 0; i <= count; i++)
            {
                Bullet b = new Bullet(texture, this);
                angle += move;
                b.angle = angle;

                double radiansAngle = (b.angle * Math.PI) / 180;

                x = origin.X + Math.Cos(radiansAngle) * 60;
                y = origin.Y + Math.Sin(radiansAngle) * 60;

                b.Position = new Vector2((float)x, (float)y);
                b.Velocity = new Vector2(0, 1);

                bullets.Add(b);
                b.Observers.Add(this);
                deviation += 3;
            }
            isActive = true;
        }

        public CircularPattern(Texture2D newTexture, Vector2 Origin, int Count, int newSpeed)
        {
            speed = newSpeed;
            velocity = new Vector2(0, speed);
            //Texture = newTexture;
            texture = newTexture;
            origin = Origin;
            this.count = Count;
            bullets = new List<Bullet>(this.count);
            isActive = true;
            //Active = true;
            //angle
            double angle = 0;
            double move = 360 / count;
            Double deviation = 1;

            for (int i = 0; i <= count; i++)
            {
                Bullet b = new Bullet(texture, this);
                angle += move;
                b.angle = angle;

                double radiansAngle = (b.angle * Math.PI) / 180;

                x = origin.X + Math.Cos(radiansAngle) * 60;
                y = origin.Y + Math.Sin(radiansAngle) * 60;

                b.Position = new Vector2((float)x, (float)y);
                b.Velocity = new Vector2(0, speed);

                bullets.Add(b);
                b.Observers.Add(this);
                deviation += 3;
            }
            isActive = true;
        }

        public override void Update(GameTime gameTime, List<Entity<int>> entities)
        {
            base.Update(gameTime, entities);

            foreach(var bullet in bullets)
            {
                bullet.angle += 2;
                Double radians = (bullet.angle * Math.PI) / 180.0;
                Double x = bullet.Position.X + Math.Cos(radians) * 0.5;
                Double y = bullet.Position.Y + Math.Sin(radians) * 0.5;

                bullet.Position = new Vector2((float)x, (float)y);
                if(Vector2.Distance(bullet.Position, spriteLocation) > 1000)
                {
                    bullet.isActive = false;
                }
            }
            foreach(var b in bullets)
            {
                b.Position += (1 * b.Velocity);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            foreach(Bullet b in bullets)
                b.Draw(spriteBatch);
            
            
        }

        public override int collision(Rectangle hitBox)
        {
            //if there is a collision run through the list and remove the bullet at to non active
            for(int i = 0; i < bullets.Count; i++)
            {
                if(bullets[i].hitBox.Intersects(hitBox) && bullets[i].isActive == true)
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
