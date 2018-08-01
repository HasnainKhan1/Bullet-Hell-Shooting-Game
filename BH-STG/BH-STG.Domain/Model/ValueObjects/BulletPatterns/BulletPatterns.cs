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
using BHSTG.SharedKernel.GameDomainObjects;

namespace BHSTG.Domain.Model.ValueObjects.BulletPatterns
{
    //base class that all other pattern classes inherits
    public abstract class BulletPatterns : Observer
    {
        
        public List<Bullet> bullets;
        public List<Bullet> removeBullets = new List<Bullet>();

        public Texture2D texture;

        public Vector2 position;

        public Vector2 velocity;

        public Vector2 origin;

        public bool isActive;
        public Vector2 playerLocation;
        public Vector2 spriteLocation = new Vector2(0, 0);

        public void setOrigin(Vector2 Origin)
        {
            origin = Origin;
        }
        public void setVelocity(Vector2 Velocity)
        {
            velocity = Velocity;
        }

        //for player
        public virtual void Draw(SpriteBatch spriteBatch, MainPlayer player)
        {}


        public abstract void Draw(SpriteBatch spriteBatch);



        public virtual void Update(GameTime gameTime, List<Entity<int>> entities)
        {
            foreach(var bullet in bullets)
                bullet.Update(gameTime, entities);

            if (removeBullets != null)
            {
                foreach (var bullet in removeBullets)
                {
                    bullets.Remove(bullet);
                }

                removeBullets.Clear();
            }
        }

        public override void Update(Entity<int> entityToRemove)
        {
            //remove bullet from pattern
            removeBullets.Add((Bullet)entityToRemove);
        }

        public virtual int collision(Rectangle hitBox)
        {
            return 0;
        }

        public Rectangle hitBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, 10, 10); }
        }


    }

    public static class BulletPatternFactory
    {
        public enum PatternTypes
        {
            Circular,
            HalfCircle,
            Player,
            Random,
            Single
        };

        public static BulletPatterns CreateCirclePattern(Vector2 position, Texture2D texture)
        {
            return new CircularPattern(texture, position, 12);

        }

        public static BulletPatterns CreateHalfCirclePattern(Vector2 position, Texture2D texture)
        {
            return new halfCirclePattern(texture, position, 6);

        }

        public static BulletPatterns CreatePlayerBulletPattern(Vector2 position, Texture2D texture)
        {
            return new playerBullet(texture, position, 1);

        }
        public static BulletPatterns CreateRandomBulletPattern(Vector2 position, Texture2D texture)
        {
            return new RandomPattern(texture, position, 50);

        }
        public static BulletPatterns CreateSingleBulletPattern(Vector2 position, Texture2D texture)
        {
            return new SingleBulletPattern(texture, position);
        }
    }
}
