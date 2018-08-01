using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BHSTG.SharedKernel.BaseDomainObjects;

namespace BHSTG.SharedKernel.GameDomainObjects
{
    public abstract class BossBullet : Entity<BossBullet>
    {
        // Texture to represent the bullets
        public Texture2D BulletTexture;

        // Speed in which the bullet travels - default is 30f
        public float BulletSpeed = 30f;



        public Vector2 referencePoint;

        // Range of the bullet
        public int Range;

        // The width of the bullet
        //public int Width => BulletTexture.Width;

        // The height of the bullet
        //public int Height => BulletTexture.Height;
        // box for the bullet
        public Rectangle BulletBox;

        // number of bullets per wave
        public int bulletsPerWave = 20;

        public virtual void MoveInPattern(Entity<int> boss)
        {
            
        }

        public virtual Vector2 MoveTowards(Vector2 position, Vector2 target)
        {
            double direction = (float)(Math.Atan2(target.Y - position.Y, target.X - position.X) * 180 / Math.PI);

            Vector2 move = new Vector2(0, 0)
            {
                X = (float)Math.Cos(direction * Math.PI / 180) * Velocity.X,
                Y = (float)Math.Sin(direction * Math.PI / 180) * Velocity.Y
            };

            return move;
        }
    }
}
