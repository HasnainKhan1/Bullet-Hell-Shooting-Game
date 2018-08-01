using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHSTG.Domain.Model.ValueObjects.BulletPatterns;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BHSTG.SharedKernel.BaseDomainObjects;
using BHSTG.SharedKernel.GameDomainObjects;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    public class Bullet : Entity<int>
    {
        public Vector2 towards;
        public Vector2 nextPosition;
        public Vector2 origin;
        public Vector2 direction;

        public double angle;
        //for circle
        public int counter = 0;
        //for random bullet attack
        public float rand;
        public bool isActive;
        public bool onScreen;
        public float distance;

        public Bullet(Texture2D newTexture, object T)
        {
            switch (T)
            {
                case CircularPattern cP:
                    Color = Color.RoyalBlue;
                    break;
                case playerBullet pB:
                    Color = Color.Green;
                    break;
                case halfCirclePattern hCp:
                    Color = Color.Yellow;
                    break;
                case SingleBulletPattern sBp:
                    Color = Color.Magenta;
                    break;
            }

            Texture = newTexture;
            //abstract member variable
            Velocity = new Vector2(0f, 1f);
            isActive = true;
           
            Orientation = 0f;
            Active = true;
            Damage = 10;
            HitBox = new Rectangle((int)Position.X, (int)Position.Y, newTexture.Width, newTexture.Height);
        }

        public Bullet(Texture2D newTexture, float newRand)

        {
            Texture = newTexture;
            rand = newRand;
            Position = origin;
            Velocity = new Vector2(0f, 1f);
            isActive = true;
            Color = Color.Green;
            Orientation = 0f;
            Active = true;
            Damage = 10;
            HitBox = new Rectangle((int)Position.X, (int)Position.Y, newTexture.Width, newTexture.Height);
        }

        public void Update(GameTime gametime, Vector2 location)
        {
            //move in a straigt line
            Position.X += Velocity.X;
            //when bullet goes out of bound. need to figure out exact boundary
            if(Vector2.Distance(Position, location) > 1000)
            {
                isActive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            //spriteBatch.Draw(Texture, Position, null, Color.White * 1f, 0f, origin, 1f, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime, List<Entity<int>> entities)
        {
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
            HitBox.Location = new Point((int)Position.X, (int)Position.Y);
            Update(gameTime, Position);
        }

        public Rectangle hitBox => HitBox;


        public bool CheckCollison(List<Entity<int>> entities)
        {
            MainPlayer player;

            for (int i = 1; i < entities.Count; i++)
            {
                if (IsCollision(this, entities[i]))
                {
                    if (entities[i] is MainPlayer)
                    {
                        player = (MainPlayer)entities[i];
                        Die(this);
                    }
                    else
                    {
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
