using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Flyer.Interfaces;
using Flyer.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = Flyer.Interfaces.IDrawable;

namespace Flyer.Bonuses
{
    public class WeaponUpgrade : IBonus
    {
        public Texture2D Texture { get; set; }
        public Vector2 Location { get; set; }

        public WeaponUpgrade(Texture2D texture, Vector2 location)
        {
            this.Texture = texture;
            this.Location = location;
            this.Bonus = 5;
        }

        public void Upgrade(Ship player)
        {
            switch (player.ProjectileType)
            {
                case "bullet":
                    player.Damage += this.Bonus;
                    player.reload -= this.Bonus;
                    player.ProjectileType = "laser";
                    break;
                case "laser":
                    player.Damage += this.Bonus;
                    player.reload -= this.Bonus;
                    player.ProjectileType = "plasma";
                    break;
                case "plasma":
                    player.ProjectileType = "plasma";
                    break;
            }
            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this != null)
            {
                spriteBatch.Draw(this.Texture, this.Location, Color.White);
            }
        }

        public int Bonus { get; set; }
    }
}
