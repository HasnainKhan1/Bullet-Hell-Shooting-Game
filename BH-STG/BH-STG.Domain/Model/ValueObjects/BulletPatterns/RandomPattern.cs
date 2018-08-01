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
    public class RandomPattern : BulletPatterns
    {

        double distance = 14; ///How Wide Circle is
        double x;
        double y;
        int speed;
        public RandomPattern(Texture2D newTexture, Vector2 newVector, int count)
        {
            velocity = new Vector2(0, 1);
            texture = newTexture;
            origin = newVector;

            bullets = new List<Bullet>(count);

            double angle = 0;
            double change = 1000 / count;
            double deviation = 1;
            Random rand = new Random();
            for (int i = 0; i < count / 2; i++)
            {
                float randomNumber = rand.Next(-20, 20) / 5;

                Bullet b = new Bullet(texture, randomNumber);
                angle += change;

                b.angle = angle;

                double radians = (b.angle * Math.PI) / 180.0;

                x = origin.X + (Math.Cos(radians) * distance * deviation / 2) + randomNumber; //dividing by 2 makes it skinnier vertically
                y = origin.Y + (distance * deviation);

                b.Position = new Vector2((float)x, (float)y);
                b.Velocity = velocity;
                bullets.Add(b);
                b.Observers.Add(this);

                deviation += 1;
            }
        }

        public RandomPattern(Texture2D newTexture, Vector2 newVector, int count, int newSpeed)
        {
            speed = newSpeed;
            velocity = new Vector2(0, speed);
            texture = newTexture;
            origin = newVector;

            bullets = new List<Bullet>(count);

            double angle = 0;
            double change = 1000 / count;
            double deviation = 1;
            Random rand = new Random();
            for (int i = 0; i < count / 2; i++)
            {
                float randomNumber = rand.Next(-20, 20) / 5;

                Bullet b = new Bullet(texture, randomNumber);
                angle += change;

                b.angle = angle;

                double radians = (b.angle * Math.PI) / 180.0;

                x = origin.X + (Math.Cos(radians) * distance * deviation / 2) + randomNumber; //dividing by 2 makes it skinnier vertically
                y = origin.Y + (distance * deviation);

                b.Position = new Vector2((float)x, (float)y);
                b.Velocity = velocity;
                bullets.Add(b);
                b.Observers.Add(this);

                deviation += 1;
            }
        }

        public override void Update(GameTime gameTime, List<Entity<int>> entities)
        {
            base.Update(gameTime, entities);
            Random rand = new Random(); //for moving the bullets at random

            foreach (var bullet in bullets)
            {
                bullet.angle += 1;

                double radians = (bullet.angle * Math.PI) / 180.0;
                double x = bullet.Position.X += bullet.rand / 15 * ((float)(Math.Cos(radians)));
                double y = bullet.Position.Y + 1;

                bullet.Position = new Vector2((float)(x), (float)(y));


                if (Vector2.Distance(bullet.Position, spriteLocation) > 1000)
                {
                    bullet.isActive = false;
                }
            }

            foreach (var bullet in bullets)
            {
                bullet.Position += (bullet.Velocity / 2); 
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in bullets)
                b.Draw(spriteBatch);
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