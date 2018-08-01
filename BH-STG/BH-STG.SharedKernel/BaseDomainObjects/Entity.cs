using System;
using System.Collections.Generic;
using System.Text;
using BHSTG.SharedKernel.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BHSTG.SharedKernel.BaseDomainObjects
{
    public abstract class Entity<TId> : Subject, IEquatable<Entity<TId>>
    {
        public TId Id { get; protected set; }

        // Entity Hit Box
        public Rectangle HitBox;

        // Animation representing the entity
        public Texture2D Texture;

        // The tint of the image. allows for transparency 
        public Color Color = Color.White * 1f;

        // Position of the entity relative to the upper left side of the screen
        public Vector2 Position, Velocity;

        // Used for circular collision detection
        public float Radius = 20;

        // Orientation of the entity
        public float Orientation;

        // State of the entity
        public bool Active;

        // Onscreen state of entity
        public bool Onscreen;

        // The amount of damage the entity inflicts
        public int Damage;

        // The speed at which the entity moves
        public float MovementSpeed;

        // Get the width of the entity
        public int Width => Texture.Width;

        // Get the height of the entity 
        public int Height => Texture.Height;

        // entity health
        public int Health { get; set; }

        // Size of the entity
        public Vector2 Size => Texture == null ? Vector2.Zero : new Vector2(Texture.Width, Texture.Height);

        // method to return whether the entity is active
        public bool IsActive() => Active;

        public abstract void Update(GameTime gameTime, List<Entity<int>> entities);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color * 1f, Orientation,
                Size, 1f, SpriteEffects.None, 0);
        }

        public void Die(Entity<int> entityToRemove)
        {
            Active = false;
            Notify(entityToRemove);
        }

        protected Entity(TId id)
        {
            if (object.Equals(id, default(TId)))
            {
                throw new ArgumentException("The ID cannot be the type's default value.", "id");
            }

            this.Id = id;
        }

        // Required empty constructor
        protected Entity()
        {
        }

        public override bool Equals(object otherObject)
        {
            var entity = otherObject as Entity<TId>;
            if (entity != null)
            {
                return this.Equals(entity);
            }
            return base.Equals(otherObject);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public bool Equals(Entity<TId> other)
        {
            if (other == null)
            {
                return false;
            }
            return this.Id.Equals(other.Id);
        }

        public virtual void TakeDamage(Entity<int> entityGivingDamage)
        {
            Health -= entityGivingDamage.Damage;
        }

        // returns true if two entity's hit boxes intersect.
        public virtual bool IsCollision(Entity<int> objectOne, Entity<int> objectTwo)
        {
            return objectOne.HitBox.Intersects(objectTwo.HitBox);
        }
    }
}
