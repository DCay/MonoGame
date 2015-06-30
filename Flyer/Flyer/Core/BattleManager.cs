using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flyer.Enemies;
using Flyer.Interfaces;
using Flyer.Projectiles;
using Flyer.Projectiles.EnemyProjectiles;
using Microsoft.Xna.Framework;

namespace Flyer.Core
{
    static class BattleManager
    {
        public static int CheckHitStatus(List<IProjectile> projectiles,List<Enemy> enemies)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                for (int j = 0; j < projectiles.Count; j++)
                {
                    if ((projectiles[j].Location.X>=enemies[i].Location.X-enemies[i].Texture.Width/2)
                        && (projectiles[j].Location.X <= enemies[i].Location.X + enemies[i].Texture.Width / 2)
                        && (projectiles[j].Location.Y >= enemies[i].Location.Y - enemies[i].Texture.Height / 2)
                        && (projectiles[j].Location.Y <= enemies[i].Location.Y + enemies[i].Texture.Height / 2)
                        )
                    {
                        projectiles.Remove(projectiles[j]);
                        return i;
                    }
                }
            }
            return -1;
        }

        public static int CheckCollisionStatus(Ship player, List<Enemy> enemies)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if ((player.location.X >= enemies[i].Location.X - enemies[i].Texture.Width/2)
                    && (player.location.X <= enemies[i].Location.X + enemies[i].Texture.Width/2)
                    && (player.location.Y >= enemies[i].Location.Y - enemies[i].Texture.Height/2)
                    && (player.location.Y <= enemies[i].Location.Y + enemies[i].Texture.Height/2)
                    )
                {
                    return i;
                }
            }
            return -1;
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
