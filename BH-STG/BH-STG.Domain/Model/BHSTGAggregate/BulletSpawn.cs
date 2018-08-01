using System;
using System.Collections.Generic;
using BHSTG.SharedKernel.BaseDomainObjects;
using BHSTG.SharedKernel.GameDomainObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    public class BulletSpawn : Entity<int>
    {
        public List<BossBullet> Bullets { get; set; }
        public Vector2 CurrentPosition { get; set; }


        private double lastTime;
        private int ticker = 1;
        public BulletSpawn()
        {
            CurrentPosition = Position;
            MovementSpeed = 6f;
            Orientation = 0f;
            
        }


        private void MoveBullets(GameTime time, List<Entity<int>> entities)
        {
            foreach(var bullet in Bullets)
            {
                bullet.Update(time, entities);
            }
        }


        public override void Update(GameTime gameTime, List<Entity<int>> entities)
        {
            MoveBullets(gameTime, entities);
            Orientation += 0.01f;
            //Position = new Vector2(Position.X - (Size.X / 2), Position.Y);
            if (ticker % 200 == 0)
            {
                this.Color = Color.White * 0f;
            }

            ticker++;

            foreach(var bullet in Bullets)
                bullet.Update(gameTime, entities);


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);

            foreach(var bullet in Bullets)
                bullet.Draw(spriteBatch);
        }
    }

    public static class BulletSpawnFactory
    {
        public static BulletSpawn Create(Vector2 position, Color color, float radius, 
            List<BossBullet> bullets, Vector2 velocity, Texture2D texture)
        {
            return new BulletSpawn()
            {
                Color = color * 1f,
                Position = position,
                Texture = texture,
                Radius = radius,
                Velocity = velocity,
                Bullets = bullets,
                Active = true
            };
        }
    }
}
