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
using Flyer.Objects.Bonuses;
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
        private static Texture2D UpgradeTexture2;

        public static void Initialise(Texture2D explosionTexture, Texture2D upgradeTexture, Texture2D upgradeTexture2)
        {
            ExplosionTexture = explosionTexture;
            UpgradeTexture = upgradeTexture;
            UpgradeTexture2 = upgradeTexture2;
        }

        public static void CheckHitStatus(Ship player,List<Enemy> enemies,List<Explosion> explosions,List<IBonus> upgrades)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                for (int j = 0; j < player.projectiles.Count; j++)
                {
                    if ((player.projectiles[j].Location.X >= enemies[i].Location.X - enemies[i].Texture.Width/2)
                        && (player.projectiles[j].Location.X <= enemies[i].Location.X + enemies[i].Texture.Width/2)
                        && (player.projectiles[j].Location.Y >= enemies[i].Location.Y - enemies[i].Texture.Height/2)
                        && (player.projectiles[j].Location.Y <= enemies[i].Location.Y + enemies[i].Texture.Height/2)
                        )
                    {
                        if (player.projectiles[j].GetType().Name != "PlasmaProjectile")
                        {
                            player.projectiles.Remove(player.projectiles[j]);
                        }
                        Random chanceToDrop = new Random();
                        enemies[i].HitPoints -= player.Damage;
                        if (enemies[i].HitPoints <= 0)
                        {
                            enemies[i].isDead = true;
                            player.playerScore += 10;
                            explosions.Add(new Explosion(ExplosionTexture, enemies[i].location));
                            //EXPLOSION
                            int explosionIndex = explosions.Count - 1;
                            if (enemies[i].GetType().Name != "Invader")
                            {
                                explosions[explosionIndex].Location =
                                new Vector2((enemies[i].Location.X - enemies[i].Texture.Width)
                                    , (enemies[i].Location.Y - enemies[i].Texture.Height));
    
                            }
                            else
                            {
                                explosions[explosionIndex].Location =
                                new Vector2((enemies[i].Location.X)
                                    , (enemies[i].Location.Y));

                            }                            
                            //LOOT
                            int isDroped = chanceToDrop.Next(40);
                            switch (isDroped)
                            {
                                case 1:
                                    upgrades.Add(new WeaponUpgrade(UpgradeTexture, enemies[i].Location));
                                    break;
                                case 15:
                                    upgrades.Add(new WeaponUpgrade(UpgradeTexture, enemies[i].Location));
                                    break;
                                case 35:
                                    upgrades.Add(new WeaponUpgrade(UpgradeTexture, enemies[i].Location));
                                    break;
                                case 20:
                                    upgrades.Add(new ShieldRegenerate(UpgradeTexture2,enemies[i].Location));
                                    break;
                            }
                            //END LOOT
                            enemies.Remove(enemies[i]);
                            break;
                        }
                    }
                }
            }
            return;
        }

        public static bool CheckCollisionStatus(Ship player,List<Enemy> enemies,List<Explosion> explosions)
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

                    if (enemies[i].GetType().Name != "Invader")
                    {
                        explosions[explosionIndex].Location =
                        new Vector2((enemies[i].Location.X - enemies[i].Texture.Width)
                            , (enemies[i].Location.Y - enemies[i].Texture.Height));

                    }
                    else
                    {
                        explosions[explosionIndex].Location =
                        new Vector2((enemies[i].Location.X)
                            , (enemies[i].Location.Y));

                    }     

                    enemies.Remove(enemies[i]);
                    if (player.ShieldPoints <= 0)
                    {
                        player.HitPoints -= enemies[i].Damage+20;
                    }
                    else
                    {
                        player.ShieldPoints -= enemies[i].Damage+20;
                    }
                    player.WayToDie = "Allahu Akbar ! ! !";
                    return true;
                }
            }
            return false;
        }

        public static void CheckIfShipHit(List<Enemy> enemies, Ship player)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                for (int j = 0; j < enemies[i].projectiles.Count; j++)
                {
                    if (enemies[i].projectiles[j].Location.X >= player.sourceRectangle.Left - 20 + player.location.X
                        && enemies[i].projectiles[j].Location.X <= player.sourceRectangle.Right/2 + player.location.X
                        && enemies[i].projectiles[j].Location.Y >= player.sourceRectangle.Top - 20 + player.location.Y
                        && enemies[i].projectiles[j].Location.Y <= player.sourceRectangle.Bottom/2 + player.location.Y)
                    {
                        if (player.ShieldPoints < 0)

                        {
                            player.HitPoints -= enemies[i].Damage;
                        }
                        else
                        {
                            player.ShieldPoints -= enemies[i].Damage;
                        }
                        enemies[i].projectiles.Remove(enemies[i].projectiles[j]);
                        player.WayToDie = "Rest In Pieces...";
                        return;
                    }
                }
            }
        }
    }
}
