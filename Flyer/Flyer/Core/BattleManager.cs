using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Flyer.Bonuses;
using Flyer.Enemies;
using Flyer.Interfaces;
using Flyer.Objects;
using Flyer.Projectiles;
using Flyer.Projectiles.EnemyProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flyer.Core
{
    static class BattleManager
    {
        private static Texture2D ExplosionTexture;
        private static Texture2D UpgradeTexture;


        public static void Initialise(Texture2D explosionTexture,Texture2D upgradeTexture)
        {
            ExplosionTexture = explosionTexture;
            UpgradeTexture = upgradeTexture;
        }

        public static void CheckHitStatus(Ship player,List<Enemy> enemies,List<Explosion> explosions,List<WeaponUpgrade> upgrades)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                for (int j = 0; j < player.shipProjectiles.Count; j++)
                {
                    if ((player.shipProjectiles[j].Location.X >= enemies[i].Location.X - enemies[i].Texture.Width/2)
                        && (player.shipProjectiles[j].Location.X <= enemies[i].Location.X + enemies[i].Texture.Width/2)
                        && (player.shipProjectiles[j].Location.Y >= enemies[i].Location.Y - enemies[i].Texture.Height/2)
                        && (player.shipProjectiles[j].Location.Y <= enemies[i].Location.Y + enemies[i].Texture.Height/2)
                        )
                    {
                        player.shipProjectiles.Remove(player.shipProjectiles[j]);
                        Random chanceToDrop = new Random();
                        player.playerScore += 10;
                        explosions.Add(new Explosion(ExplosionTexture,enemies[i].location));
                        //EXPLOSION
                        int explosionIndex = explosions.Count-1;
                        explosions[explosionIndex].Location = new Vector2((enemies[i].Location.X - enemies[i].Texture.Width)
                            , (enemies[i].Location.Y - enemies[i].Texture.Height));

                        //LOOT
                        int isDroped = chanceToDrop.Next(20);
                        switch (isDroped)
                        {
                            case 1:
                                upgrades.Add(new WeaponUpgrade(UpgradeTexture, enemies[i].Location));
                                break;
                            case 8:
                                break;
                            default:
                                break;
                        }
                        //END LOOT
                        enemies.Remove(enemies[i]);
                    }
                }
            }
            return;
        }

        public static void CheckCollisionStatus(Ship player,List<Enemy> enemies,List<Explosion> explosions)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if ((player.location.X >= enemies[i].Location.X - enemies[i].Texture.Width/2)
                    && (player.location.X <= enemies[i].Location.X + enemies[i].Texture.Width/2)
                    && (player.location.Y >= enemies[i].Location.Y - enemies[i].Texture.Height/2)
                    && (player.location.Y <= enemies[i].Location.Y + enemies[i].Texture.Height/2)
                    )
                {
                    //COLLISION
                    player.playerScore += 10;
                    explosions.Add(new Explosion(ExplosionTexture,enemies[i].location));
                    int explosionIndex = explosions.Count - 1;
                    explosions[explosionIndex].Location = new Vector2((enemies[i].Location.X - enemies[i].Texture.Width)
                        , (enemies[i].Location.Y - enemies[i].Texture.Height));
                    enemies.Remove(enemies[i]);
                    if (player.PlayerShields <= 0)
                    {
                        player.PlayerHP -= 50;
                    }
                    else
                    {
                        player.PlayerShields -= 50;
                    }
                }
            }
            return;
        }

        public static void CheckIfShipHit(List<EnemyProjectile> projectiles, Ship player)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                if (projectiles[i].Location.X >= player.sourceRectangle.Left - 20 + player.location.X
                    && projectiles[i].Location.X <= player.sourceRectangle.Right/2 + player.location.X
                    && projectiles[i].Location.Y >= player.sourceRectangle.Top - 20 + player.location.Y
                    && projectiles[i].Location.Y <= player.sourceRectangle.Bottom/2 + player.location.Y)
                {
                    if (player.PlayerShields < 0)

                    {
                        player.PlayerHP -= 10;
                    }
                    else
                    {
                        player.PlayerShields -= 10;
                    }
                    projectiles.Remove(projectiles[i]);
                    return;
                }
            }
        }
    }
}
