using System;
using System.Collections.Generic;
using System.Text;
using BHSTG.Domain.Model.ValueObjects;
using BHSTG.SharedKernel.BaseDomainObjects;
using BHSTG.SharedKernel.Enums.BHSTG.SharedKernel.Enums;
using BHSTG.SharedKernel.GameImages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BHSTG.Domain.Model.ValueObjects.BulletPatterns;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    public class StandardEnemy : Entity<int>
    {
        private static StandardEnemy _instance;
        public static StandardEnemy Instance => _instance ?? (_instance = new StandardEnemy());

        public MovePattern enemyMovement;
        public List<MovePattern> enemyMoves = new List<MovePattern>();
        public int moveCount = 1;
        public int delayTime;

        public int PointValue;

        //public List<BulletSpawn> Bullets { get; set; }
        public List<BulletPatterns> Bullets { get; set; }

        public bool Inactive { get; set; }

        public Vector2 InitialPosition { get; set; }
        public Vector2 TargetPosition { get; set; }


        public StandardEnemy(Vector2 position, Vector2 target, List<MovePattern> moves)
        {

            Texture = GameArt.StandardEnemies;
            Velocity = new Vector2(1f, 1f);
            Position = position;
            InitialPosition = position;
            TargetPosition = target;
            Color = Color.Red;
            enemyMoves = moves;
            enemyMovement = enemyMoves[0];
            moveCount = enemyMoves.Count;
            Health = 50;
            Radius = 20f;
            PointValue = 50;
            Onscreen = false;

            delayTime = 250;

            MovementSpeed = 6f;
            Orientation = 0f;

            HitBox = new Rectangle(Position.ToPoint(), new Point(Texture.Width / 4, Texture.Height / 4));
        }

        public StandardEnemy() : base(new Random().Next(1, 1000))
        {
            Radius = 6;
            Active = true;
            Velocity = new Vector2(1f, 1f);

            Health = 50;
            PointValue = 50;

            Damage = 50;

            delayTime = 250;
            InitialPosition = Position;
            TargetPosition = new Vector2(180.5f, 180.5f);

            moveCount = enemyMoves.Count;
            Active = true;
            MovementSpeed = 6f;
            Onscreen = false;

            Orientation = 0f;

            Color = Color.Aquamarine * 1f;
            Texture = GameArt.StandardEnemies;

            // Set the hitbox size
            HitBox = new Rectangle(Position.ToPoint(), new Point(Texture.Width / 4, Texture.Height / 4));
        }



        private int i = 0;

        public void UpdateStandardEnemy()
        {
            float x, y;
            if (i < enemyMoves.Count && (enemyMoves[i] == MovePattern.Delay) && delayTime == 0)
            {
                i++;
                moveCount--;
            }
            else if (i < enemyMoves.Count && (enemyMoves[i] == MovePattern.Delay) && delayTime > 0)
            {
                delayTime--;
            }

            if (i == 2)
            {
                if (moveCount == 1)
                {
                    TargetPosition = InitialPosition;
                    InitialPosition = Position;
                }
                moveCount--;
            }

            int currentX = (int)Math.Round(Position.X);
            int futureX = (int)Math.Round(TargetPosition.X);

            int currentY = (int)Math.Round(Position.Y);
            int futureY = (int)Math.Round(TargetPosition.Y);



            if (i < enemyMoves.Count && (currentX != futureX || currentY != futureY))
            {
                if (enemyMoves[i] == MovePattern.Straight)
                {
                    float deltaX = Math.Abs(InitialPosition.X - TargetPosition.X);
                    float deltaY = Math.Abs(InitialPosition.Y - TargetPosition.Y);
                    double theta = Math.Atan(deltaY / deltaX);

                    x = (float)Math.Cos(theta);
                    y = (float)Math.Sin(theta);

                    if (InitialPosition.X > TargetPosition.X)
                    {
                        x = -x;
                    }
                    if (InitialPosition.Y > TargetPosition.Y)
                    {
                        y = -y;
                    }
                    Position.X += x;
                    Position.Y += y;


                }
                else if (enemyMoves[i] == MovePattern.StraightDown)
                {
                    Position.Y += Velocity.Y;
                }
                else if (enemyMoves[i] == MovePattern.QuarterTurnRight)
                {
                    if (currentY <= futureY)
                    {
                        Position.Y += Velocity.Y;
                    }
                    else
                    {
                        Position.X += Velocity.X;
                    }

                }
                else if (enemyMoves[i] == MovePattern.QuarterTurnLeft)
                {
                    if (currentY <= futureY)
                    {
                        Position.Y += Velocity.Y;
                    }
                    else
                    {
                        Position.X -= Velocity.X;
                    }
                }
                else if (enemyMoves[i] == MovePattern.Butterfly)
                {
                    TargetPosition = new Vector2(Position.X, 200f);
                    if (Math.Abs(Position.Y - TargetPosition.Y) >= 1f)
                    {
                        Position.Y += Velocity.Y;
                    }
                    //else
                    //{
                    if (InitialPosition.X <= 450)
                    {
                        Position.X += Velocity.X;
                    }
                    else
                    {
                        Position.X -= Velocity.X;
                    }
                    //}
                }
            }
            else if (i < enemyMoves.Count && (enemyMoves[i] != MovePattern.Delay)) { i++; moveCount--; }

            HitBox.Location = new Point((int)Position.X - Height / 2, (int)Position.Y - Width / 2);

        }


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
                                mp.TakeDamage();

                                // TO DO: Check 
                            }
                            break;

                        // add main player bullet logic here
                        case PlayerBullet pb:
                            if (IsCollision(pb))
                            {
                                TakeDamage(pb.Damage);
                            }
                            break;
                    }
                UpdateStandardEnemy();
                //foreach (var bullet in Bullets)
                //{
                //    bullet.Update(gameTime);
                //}

                //foreach (var bulletSpawn in Bullets)
                //    bulletSpawn.Update(gameTime, entities);
                foreach (var bulletPattern in Bullets)
                    bulletPattern.Update(gameTime, entities);
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            //foreach(var bulletspawn in Bullets)
            //    bulletspawn.Draw(spriteBatch);

            foreach (var pattern in Bullets)
                pattern.Draw(spriteBatch);

            //spriteBatch.Draw(Texture, InitialPosition, null, Color.Red, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }


        public bool IsCollision(Entity<int> gameEntity)
        {
            return HitBox.Intersects(gameEntity.HitBox);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;

            if (IsDead())
            {
                Die(this);
            }
        }

        public bool IsDead()
        {
            return Health <= 0;
        }

    }
    public class Butterfly : StandardEnemy
    {

        public Butterfly()
        {
            Texture = GameArt.ButterflyEnemies;
            // bullets
            TargetPosition = new Vector2(Position.X, 200f);
        }

        public void UpdateButterflyEnemy()
        {
            if (Math.Abs(Position.Y - TargetPosition.Y) >= 1f)
            {
                Position.Y += Velocity.Y;
            }
            else
            {
                if (InitialPosition.X <= 450)
                {
                    Position.X += Velocity.X;
                }
                else
                {
                    Position.X -= Velocity.X;
                }
            }
            HitBox.Location = new Point((int)Position.X, (int)Position.Y);
        }

        public override void Update(GameTime gameTime, List<Entity<int>> entities)
        {
            if (!IsDead())
            {
                // for each of the entities in the game, see if they are the main player
                // or if they are the main player bullets. nothing else can damage the boss.
                foreach (var entity in entities)
                {
                    switch (entity)
                    {
                        case MainPlayer mp:
                            if (IsCollision(mp))
                            {
                                TakeDamage(mp.Damage);

                                // TO DO: Check 
                            }

                            break;

                        case PlayerBullet pb:
                            if (IsCollision(pb))
                            {
                                TakeDamage(pb.Damage);
                            }
                            break;
                    }
                }
                UpdateButterflyEnemy();


                foreach (var bulletPattern in Bullets)
                    bulletPattern.Update(gameTime, entities);
            }

        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);


            foreach (var bulletSpawn in Bullets)
                bulletSpawn.Draw(spriteBatch);
        }
    }

    public static class StandardEnemyFactory
    {

        public static StandardEnemy Create(Texture2D texture, Vector2 velocity, Vector2 position, Vector2 target,
           List<MovePattern> moves)//, List<BulletPatterns> bullets)
        {
            return new StandardEnemy()
            {
                Texture = texture,
                Velocity = velocity,
                Position = position,
                InitialPosition = position,
                TargetPosition = target,
                Color = Color.White * 1f,
                enemyMoves = moves,
                moveCount = moves.Count,
                delayTime = 250,
                Health = 50,
                Radius = 20f,
                Orientation = 0f,
                //Bullets = bullets,
                Bullets = new List<BulletPatterns>() { },
                HitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width / 4, texture.Height / 4)
            };
        }

        public static Butterfly CreateButterfly(Texture2D texture, Vector2 velocity, Vector2 position)//, List<BulletPatterns> bullets)
        {
            return new Butterfly()
            {
                Texture = texture,
                Velocity = velocity,
                Position = position,
                InitialPosition = position,
                Color = Color.White * 1f,
                delayTime = 250,
                Health = 80,
                Radius = 20f,
                Orientation = 0f,
                //Bullets = bullets,
                Bullets = new List<BulletPatterns>() { },
                HitBox = new Rectangle((int)position.X, (int)position.Y, texture.Width / 4, texture.Height / 4)
            };
        }
    }
}

