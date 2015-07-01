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
            Texture2D projectileTexture,GameTime gameTime)
        {
            spawnTimer++;
            if (spawnTimer > 10)
            {
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
            return list;   
        } 
    
    }
}
