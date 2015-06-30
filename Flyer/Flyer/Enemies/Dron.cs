using Flyer.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flyer.Enemies
{
    public class Dron : Enemy
    {
        //Texture2D dronTexture;

        public readonly Rectangle source;

        public Dron(Texture2D texture)
            : base(3, 1, texture)
        {
            this.source = new Rectangle(0, 0, this.Texture.Width, this.Texture.Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(this.Texture.Width/2.0f,this.Texture.Height/2.0f);
            spriteBatch.Draw(Texture, Location, source, Color.White, EnemyAngle, origin, 1.0f, SpriteEffects.None, 1);
        }
    }
}
