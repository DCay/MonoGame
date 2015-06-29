using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flyer.Enemies;
using Flyer.Interfaces;
using Flyer.Projectiles;
using Microsoft.Xna.Framework;

namespace Flyer.Core
{
    static class BattleManager
    {
        public static int CheckHitStatus(List<IProjectile> projectiles,List<Dron> drones)
        {
            for (int i = 0; i < drones.Count; i++)
            {
                for (int j = 0; j < projectiles.Count; j++)
                {
                    if ((projectiles[j].Location.X>=drones[i].Location.X-drones[i].Texture.Width/2)
                        && (projectiles[j].Location.X <= drones[i].Location.X + drones[i].Texture.Width / 2)
                        && (projectiles[j].Location.Y >= drones[i].Location.Y - drones[i].Texture.Height / 2)
                        && (projectiles[j].Location.Y <= drones[i].Location.Y + drones[i].Texture.Height / 2)
                        )
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public static int CheckCollisionStatus(Ship player, List<Dron> drones)
        {
            for (int i = 0; i < drones.Count; i++)
            {
                if ((player.location.X >= drones[i].Location.X - drones[i].Texture.Width/2)
                    && (player.location.X <= drones[i].Location.X + drones[i].Texture.Width/2)
                    && (player.location.Y >= drones[i].Location.Y - drones[i].Texture.Height/2)
                    && (player.location.Y <= drones[i].Location.Y + drones[i].Texture.Height/2)
                    )
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
