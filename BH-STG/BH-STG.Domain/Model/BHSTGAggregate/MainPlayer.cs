using System;
using System.Collections.Generic;
using System.Text;
using BHSTG.Domain.Model.ValueObjects;
using BHSTG.SharedKernel.BaseDomainObjects;
using BHSTG.SharedKernel.GameImages;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BHSTG.Domain.Model.ValueObjects.BulletPatterns;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    public class MainPlayer : Entity<int>
    {
        private static MainPlayer _instance;

        public static MainPlayer Instance => _instance ?? (_instance = new MainPlayer());

        // Main Players game score
        public int Score { get; set; }

        // Main Players number of lives
        public int Lives { get; set; }

        // Main Players powerups
        public double PowerUps { get; set; }    

        // Determines whether player can be take damage or not
        public bool Invulnerable { get; set; }

        public int BombsRemaining { get; set; }
        
        public MainPlayer(int id) : base(id)
        {
            Score = 0;
        }


        public MainPlayer() : base(new Random().Next(1,1000))
        {
            Radius = 10;
            Active = true;

            Lives = 10;
            Damage = 50;
            MovementSpeed = 2;
            Invulnerable = false;
            BombsRemaining = 2;

            // Set the hitbox size
            HitBox = new Rectangle(Position.ToPoint(), Size.ToPoint());
        }

        public override void Update(GameTime gameTime, List<Entity<int>> entities)
        {
            CheckCollision(entities);

            /********************************************player movement************************************************/
            // ISSUE: doesn't handle (SHIFT + Up + Left) or (SHIFT + Up + Right) !! Should work now with Z

            MovementSpeed = 2;
            KeyboardState keyState = Keyboard.GetState(); // for holding the pressed key

            if (keyState.IsKeyDown(Keys.Z)) { MovementSpeed = 5; } // for faster movement

            if (keyState.IsKeyDown(Keys.C)) { Invulnerable = true; } // Cheat mode

            // Handles all 8 movements of the sprite at both slow and fast speeds
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
            {
                Position.X -= MovementSpeed;
                HitBox.Location = new Point((int)Position.X, (int)Position.Y);
            }
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
            {
                Position.X += MovementSpeed;
                HitBox.Location = new Point((int)Position.X, (int)Position.Y);
            }
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
            {
                Position.Y -= MovementSpeed;
                HitBox.Location = new Point((int)Position.X, (int)Position.Y);
            }
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
            {
                Position.Y += MovementSpeed;
                HitBox.Location = new Point((int)Position.X, (int)Position.Y);
            }

            //Player bomb
            if (keyState.IsKeyDown(Keys.B))
            {
                if (BombsRemaining > 0)
                {
                    Rectangle bomb = new Rectangle((int)Position.X - Texture.Width, (int)Position.Y - Texture.Height, Texture.Width * 2, Texture.Height * 2);
                    foreach (var entity in entities)
                    {
                        switch (entity)
                        {
                            case GameBoss gb:
                                foreach (var bulletpattern in gb.BulletPatterns)
                                {
                                    for (int i = 0; i < bulletpattern.bullets.Count; i++)
                                    {
                                        if (IsCollision(this, bulletpattern.bullets[i]))
                                        {
                                            bulletpattern.bullets[i].Die(bulletpattern.bullets[i]);
                                        }
                                    }
                                }

                                if (bomb.Intersects(gb.HitBox))
                                {
                                    gb.TakeDamage(100);
                                }
                                break;

                            case StandardEnemy se:
                                foreach (var bulletpattern in se.Bullets)
                                {
                                    for (int i = 0; i < bulletpattern.bullets.Count; i++)
                                    {
                                        if (IsCollision(this, bulletpattern.bullets[i]))
                                        {
                                            bulletpattern.bullets[i].Die(bulletpattern.bullets[i]);
                                        }
                                    }
                                }

                                if (bomb.Intersects(se.HitBox))
                                {
                                    se.TakeDamage(100);
                                }
                                break;
                        }
                    }
                    //decrement bomb
                    BombsRemaining -= 1;
                }
            }
        }

        public void TakeDamage()
        {
            if (Invulnerable == true)
            {
                return;
            }
            //take damage
            Lives -= 1;

            //check if still alive
            if (IsDead())
            {
                Die(this);
            }
            else
            {
                //reset position to start and be invulnerable
                Invulnerable = true;

                Position = new Vector2(400f, 400f);
                HitBox = new Rectangle(400 + Width/2, 400 + Height/2, GameArt.MainPlayer.Width/8, GameArt.MainPlayer.Height/8);

            }
        }

        public bool IsDead()
        {
            return Lives <= 0;
        }

        public bool CheckCollision(List<Entity<int>> entities)
        {
            int index = entities.IndexOf(this);

            for (int i = 0; i < entities.Count; i++)
            {
                if (i != index && Invulnerable == false)
                {
                    if (entities[i] is PlayerBullet)
                    {
                        return false;
                    }

                    if (IsCollision(this, entities[i]))
                    {
                        TakeDamage(this);
                        return true;
                    }
                }
            }

            foreach (var entity in entities)
            {
                switch (entity)
                {
                    case GameBoss gb:
                        foreach (var bulletpattern in gb.BulletPatterns)
                        {
                            for (int i = 0; i < bulletpattern.bullets.Count; i++)
                            {
                                if(IsCollision(this, bulletpattern.bullets[i]))
                                {
                                    TakeDamage();
                                    return true;
                                }
                            }                          
                        }
                        break;
                    case StandardEnemy se:
                        foreach (var bullet in se.Bullets)
                        {
                            for (int i = 0; i < bullet.bullets.Count; i++)
                            {
                                if (IsCollision(this, bullet.bullets[i]))
                                {
                                    TakeDamage();
                                    bullet.bullets[i].Notify(bullet.bullets[i]);
                                    return true;
                                }
                            }
                        }
                        break;
                }
            }
            
                return false;
        }
    }

    public static class MainPlayerFactory
    {
        public static MainPlayer CreateMainPlayer()
        {

            return new MainPlayer()
            {
                Active = true,
                Color = Color.White,
                Health = 200,
                Damage = 50,
                Radius = 10,
                Lives = 10,
                MovementSpeed = 3f,
                Orientation = (float)-(Math.PI / 2),
                Texture = GameArt.MainPlayer,
                Velocity = new Vector2(3f, 3f),
                Score = 0,
                PowerUps = 0.0,
                BombsRemaining = 2,
                Position = new Vector2(400f, 400f),
                HitBox = new Rectangle(400 + GameArt.MainPlayer.Width/2, 400 + GameArt.MainPlayer.Height/2, GameArt.MainPlayer.Width/8, GameArt.MainPlayer.Height/8),
            };
        }
    }
}
