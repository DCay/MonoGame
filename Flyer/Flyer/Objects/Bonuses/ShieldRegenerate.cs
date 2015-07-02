using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flyer.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = Flyer.Interfaces.IDrawable;

namespace Flyer.Objects.Bonuses
{
    class ShieldRegenerate : IBonus
    {
        public Texture2D Texture { get; set; }
        public Vector2 Location { get; set; }

        public ShieldRegenerate(Texture2D texture, Vector2 location)
        {
            this.Texture = texture;
            this.Location = location;
            this.Bonus = 200;
        }

        public void Upgrade(Ship player)
        {
            player.ShieldPoints = this.Bonus;
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
