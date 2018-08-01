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
    public class playerBullet : BulletPatterns
    {
        int speed;
        public playerBullet(Texture2D newTexture, Vector2 newPosition, int count)
        {
            texture = newTexture;
            position = newPosition;
            bullets = new List<Bullet>();
            isActive = true;
            //player shoot
            for (int i = 0; i < count; i++)
            {
                Bullet b = new Bullet(texture, this);
                double x = newPosition.X;
                double y = newPosition.Y;

                b.Position = new Vector2((float)x, (float)y);
                //player shooting so -1
                b.Velocity = new Vector2(0, -1);

                bullets.Add(b);
                b.Observers.Add(this);
            }
        }

        public playerBullet(Texture2D newTexture, Vector2 newPosition, int count, int newSpeed)
        {
            speed = (-1)*newSpeed;
            texture = newTexture;
            position = newPosition;
            bullets = new List<Bullet>();
            isActive = true;
            //player shoot
            for (int i = 0; i < count; i++)
            {
                Bullet b = new Bullet(texture, this);
                double x = newPosition.X;
                double y = newPosition.Y;

                b.Position = new Vector2((float)x, (float)y);
                //player shooting so -1
                b.Velocity = new Vector2(0, speed);

                bullets.Add(b);
                b.Observers.Add(this);
            }
        }

        public override void Update(GameTime gameTime, List<Entity<int>> entities)
        {
            base.Update(gameTime, entities);
            foreach (var bullet in bullets)
            {
                bullet.Position += (3 * bullet.Velocity); //move bullet further in a straight line

                if (Vector2.Distance(bullet.Position, spriteLocation) > 1000)
                {
                    bullet.isActive = false; //deallocate object if it goes off screen
                }

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
