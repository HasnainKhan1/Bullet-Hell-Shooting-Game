using System;
using System.Collections.Generic;
using BHSTG.Domain.Model.ValueObjects.BulletPatterns;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BHSTG.SharedKernel.BaseDomainObjects;
using BHSTG.SharedKernel.Enums;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    public class GameBoss : Entity<int>
    {
        private static GameBoss _instance;

        public static GameBoss Instance => _instance ?? (_instance = new GameBoss());

        public List<BulletSpawn> Bullets { get; set; }

        public List<BulletPatterns> BulletPatterns { get; set; }

        public BossTypes BossType { get; set; }

        public int PointValue;

        public Vector2 CurrentPosition { get; set; }
        public Vector2 TargetPosition1 { get; set; }
        public Vector2 TargetPosition2 { get; set; }
        public Vector2 Start { get; set; }

        private bool _position1Reached;
        private bool _position2Reached;
        private bool _startPositionReached;

        public GameBoss(int id, Vector2 position, Texture2D texture) : base (id)
        {
            Texture = texture;
            _position2Reached = false;
            // TO DO: set position of the final boss relative to the screen
            Position = position;
            CurrentPosition = position;
            Radius = 10;
            Onscreen = false;

            // Set the hitbox size
            HitBox = new Rectangle(Position.ToPoint(), Size.ToPoint());
        }

        public GameBoss() : base(new Random().Next(1, 1000))
        { 
            // TO DO: set position of the final boss relative to the screen
            Radius = 10;
            _position2Reached = false;
            Active = true;
            Onscreen = false;

            // Set the boss's health
            Health = 500;

            // Set the boss's damage
            Damage = 100;

            CurrentPosition = Start = Position;

            // Set the player point value for the boss
            PointValue = 1000;

            // Set how fast the boss moves
            MovementSpeed = 6f;

            Orientation = 0f;

            Color = Color.Red;

            TargetPosition1 = new Vector2(230.4056f, 230.4056f);
            TargetPosition2 = new Vector2(300.5f, 180.5f);

            // Set the hitbox size
            HitBox = new Rectangle(Position.ToPoint(), Size.ToPoint());
        }

        // Creates a Vector2 to use when moving object from position to a target, with a given speed
        public Vector2 MoveTowards(Vector2 position, Vector2 target)
        {
            double direction = (float)(Math.Atan2(target.Y - position.Y, target.X - position.X) * 180 / Math.PI);

            Vector2 move = new Vector2(0, 0)
            {
                X = (float)Math.Cos(direction * Math.PI / 180) * Velocity.X,
                Y = (float)Math.Sin(direction * Math.PI / 180) * Velocity.Y
            };

            return move;
        }

        private void MoveBoss()
        {
            if (_startPositionReached && !_position1Reached)
            {
                var move = MoveTowards(Position, TargetPosition1);

                // Move Down and right
                Position.Y += move.Y;
                Position.X += move.X;

                HitBox.Location = new Point((int) Position.X-Width/2, (int) Position.Y-Height/2);
            }
            else if (_position1Reached)
            {
                // Move Right and Up
                var move = MoveTowards(Position, TargetPosition2);

                // Move Down and right
                Position.Y += move.Y;
                Position.X += move.X;

                HitBox.Location = new Point((int)Position.X-Width/2, (int)Position.Y-Height/2);
            }
            else if (_position2Reached && !_position1Reached)
            {
                // Move left only
                var move = MoveTowards(Position, Start);

                // Move Down and right
                Position.Y += move.Y;
                Position.X += move.X;

                HitBox.Location = new Point((int)Position.X-Width/2, (int)Position.Y-Height/2);
            }

        }

        public void UpdateBossLocation()
        {
     
            if (Math.Abs(Position.X - TargetPosition1.X) < 2 && Math.Abs(Position.Y - TargetPosition1.Y) < 2)
            {
                _position1Reached = true;
                _position2Reached = false;
                _startPositionReached = false;
            }
                
            else if (Math.Abs(Position.X - TargetPosition2.X) < 2 && Math.Abs(Position.Y - TargetPosition2.Y) < 2)
            {
                _position1Reached = false;
                _position2Reached = true;
                _startPositionReached = false;
            }
                
            else if (Math.Abs(Position.X - Start.X) < 2 && Math.Abs(Position.Y - Start.Y) < 2)
            {
                _position1Reached = false;
                _position2Reached = false;
                _startPositionReached = true;
            }
                
            MoveBoss();
        }

        /*
         * Update() is in charge of:
         * 1. checking if the boss is dead.
         * 2. checking to see if anything has hit the boss.
         *      a. checking to see if the main player has hit it.
         *      b. checking to see any of the main player's bullets hit it.
         * 3. updating the bosses location.
         * 4. calling the game boss's bullet's Update method.
         * 
         */
        public override void Update(GameTime gameTime, List<Entity<int>> entities)
        {
            if (!IsDead())
            {
                // for each of the entities in the game, see if they are the main player
                // or if they are the main player bullets. nothing else can damage the boss.
                foreach (var entity in entities)
                    switch (entity)
                    {
                        case MainPlayer mp:
                            if (IsCollision(mp) && mp.Invulnerable == false)
                            {
                                TakeDamage(mp.Damage);

                                // TO DO: Check 
                            }

                            break;

                        // TO DO: add main player bullet logic here
                        case PlayerBullet pb:
                            if (IsCollision(pb))
                            {
                                TakeDamage(pb.Damage);
                            }
                            break;
                    }

                UpdateBossLocation();

                foreach (var bulletPattern in BulletPatterns)
                    bulletPattern.Update(gameTime, entities);
            }
            else
            {
                Active = false;
                Die(this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);     

            foreach(var bullet in BulletPatterns)
                    bullet.Draw(spriteBatch);
        }

        public bool IsCollision(Entity<int> gameEntity)
        {
            return HitBox.Intersects(gameEntity.HitBox);
        }

        public void TakeDamage(int damage)
        {

            // TO DO: game manager needs to check if objects are active or not in the entities list
            if (IsDead())
            {
                Die(this);
            }
            else
                // Take damage
                Health -= damage;
        }

        public bool IsDead()
        {
            return Health <= 0;
        }
        
        public bool CheckCollision(List<Entity<int>> entities)
        {
            int index = entities.IndexOf(this);

            for (int i = index; i < entities.Count; i++)
            {
                if (IsCollision(this, entities[i]))
                {
                    TakeDamage(entities[i]);
                    return true;
                }
                    
            }
           
            return false;
        }
    }

    public static class GameBossFactory
    {
        public static GameBoss Create(Texture2D texture, Vector2 velocity, Vector2 position, 
            BossTypes bossType, List<BulletPatterns> bulletPatterns)
        {
            return new GameBoss()
            {
                Texture = texture,
                Velocity = velocity,
                Position = position,
                Color = Color.Red,
                BossType = bossType,
                Health = 500,
                Radius = 30f,
                Orientation = 0f,
                HitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width/2, texture.Height/2),
                BulletPatterns = bulletPatterns
            };
        }
    }

}
