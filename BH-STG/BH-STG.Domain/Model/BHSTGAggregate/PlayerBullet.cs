using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BHSTG.SharedKernel.BaseDomainObjects;
using BHSTG.SharedKernel.GameImages;

namespace BHSTG.Domain.Model.BHSTGAggregate
{
    public class PlayerBullet : Entity<int>
    {
        private static PlayerBullet _instance;
        public static PlayerBullet Instance => _instance ?? (_instance = new PlayerBullet(new Vector2()));

        public PlayerBullet(int id) : base(id) {

        }

        public PlayerBullet(Vector2 playerPosition) : base(new Random().Next(1,1000))
        {
            Radius = 10;
            Active = true;

            Damage = 50;
            MovementSpeed = 4;

            HitBox = new Rectangle(Position.ToPoint(), Size.ToPoint());
        }

        public override void Update(GameTime gameTime, List<Entity<int>> entities)
        {
            //check for a collision
            CheckCollision(entities);

            //move the bullet
            Position.Y -= MovementSpeed;
            HitBox.Location = new Point((int)Position.X, (int)Position.Y);

            //check for a collision
            //CheckCollision(entities);
        }


        public bool CheckCollision(List<Entity<int>> entities)
        {
            int index = entities.IndexOf(this);
            GameBoss boss;
            StandardEnemy enemy;

            for (int i = 1; i < entities.Count; i++)
            {
                if (i != index)
                {
                    if (IsCollision(this, entities[i]))
                    {
                        // give points to the player
                        if (entities[0] is MainPlayer)
                        {
                            var player = (MainPlayer)entities[0];
                            if (entities[i] is GameBoss)
                            {
                                boss = (GameBoss)entities[i];
                                player.Score += boss.PointValue;
                                Die(this);
                                boss.TakeDamage(Damage);
                            }
                            else if (entities[i] is StandardEnemy)
                            {
                                enemy = (StandardEnemy)entities[i];
                                player.Score += enemy.PointValue;
                                Die(this);
                                enemy.TakeDamage(Damage);
                            }
                            else
                            {
                                return false;
                            }
                        }

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
                                if (IsCollision(this, bulletpattern.bullets[i]))
                                {
                                    Die(this);
                                    bulletpattern.bullets[i].Die(bulletpattern.bullets[i]);
                                    return true;
                                }
                            }
                        }
                        break;

                    case StandardEnemy se:
                        foreach (var bulletpattern in se.Bullets)
                        {
                            for (int i = 0; i < bulletpattern.bullets.Count; i++)
                            {
                                if (IsCollision(this, bulletpattern.bullets[i]))
                                {
                                    Die(this);
                                    bulletpattern.bullets[i].Die(bulletpattern.bullets[i]);
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

    public static class PlayerBulletFactory
    {
        public static PlayerBullet CreatePlayerBullet(Vector2 playerPos)
        {
            Vector2 hitbox = new Vector2(playerPos.X, playerPos.Y);
            playerPos.Y -= 30;
            playerPos.X -= 15;

            return new PlayerBullet(playerPos)
            {
                Active = true,
                Color = Color.White * 1f,
                Damage = 50,
                Radius = 10,
                MovementSpeed = 4f,
                Orientation = (float)-(Math.PI / 2),
                Texture = GameArt.BlueBullet,
                Position = playerPos,
                HitBox = new Rectangle((int)playerPos.X, (int)playerPos.Y, GameArt.BlueBullet.Width, GameArt.BlueBullet.Height)
             };
        }
    }
}
