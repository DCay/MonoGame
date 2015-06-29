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
        public static List<Dron> Build(Texture2D texture, List<Dron> list, Vector2 location)
        {
            for (int i = 0; i < 200; i++)
            {
                Dron nextDron = new Dron(texture);

                if (Math.Sqrt((nextDron.Location.X - location.X) * (nextDron.Location.X - location.X) +
                    (nextDron.Location.Y - location.Y) * (nextDron.Location.Y - location.Y)) > 1400)
                {
                    list.Add(nextDron);
                }
            }
            return list;
        } 
    }
}
