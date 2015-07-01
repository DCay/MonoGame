using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Flyer.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flyer.Bonuses
{
    public class WeaponUpgrade
    {
        public Texture2D Texture;
        public Vector2 Location;

        public WeaponUpgrade(Texture2D texture, Vector2 location)
        {
            this.Texture = texture;
            this.Location = location;
        }

        public string Upgrade(string projectileType)
        {
            switch (projectileType)
            {
                case "bullet":
                    return "laser";
                case "laser":
                    return "plasma";
                case "plasma":
                    return "plasma";
            }
            return null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this != null)
            {
                spriteBatch.Draw(this.Texture, this.Location, Color.White);
            }
        }
    }
}
