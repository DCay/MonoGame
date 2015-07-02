using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flyer.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flyer.Factories
{
    public static class EnemyFactory
    {
        public static GameTime gameTime { get; set; }

        private static int spawnTimer { get; set; }

        private static int listLimit { get; set; }

        public static List<Enemy> Build(Texture2D texture, List<Enemy> list, Vector2 location)
        {
            for (int i = 0; i < 200; i++)
            {
                var nextMine = new Mine(texture);

                if (Math.Sqrt((nextMine.Location.X - location.X) * (nextMine.Location.X - location.X) +
                    (nextMine.Location.Y - location.Y) * (nextMine.Location.Y - location.Y)) > 1400)
                {
                    list.Add(nextMine);
                }
            }
            return list;
        }

        public static List<Enemy> ProcedureBuild(Texture2D texture, List<Enemy> list, Vector2 location,
            Texture2D projectileTexture, GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMinutes == 1) listLimit = 0;
            spawnTimer++;
            if (gameTime.TotalGameTime.TotalMinutes < 1)
            {
                if (spawnTimer > 10)
                {
                    listLimit = list.Count();
                    if (listLimit < 150)
                    {
                        var nextDrone = new Dron(texture, projectileTexture);

                        if (Math.Sqrt((nextDrone.Location.X - location.X)*(nextDrone.Location.X - location.X) +
                                      (nextDrone.Location.Y - location.Y)*(nextDrone.Location.Y - location.Y)) > 1400)
                        {
                            list.Add(nextDrone);
                            listLimit++;
                        }
                    }
                    spawnTimer = 0;
                }
            }
            else
            {
                if (spawnTimer > 10)
                {
                    listLimit=list.Count();
                    if (listLimit < 50)
                    {
                        var nextInvader = new Invader(texture, projectileTexture);

                        if (Math.Sqrt((nextInvader.Location.X - location.X)*(nextInvader.Location.X - location.X) +
                                      (nextInvader.Location.Y - location.Y)*(nextInvader.Location.Y - location.Y)) >
                            1400)
                        {
                            list.Add(nextInvader);
                            listLimit++;
                        }
                    }
                    spawnTimer = 0;
                }
            }
            return list;
        }
    }
}
