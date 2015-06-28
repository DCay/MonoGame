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

        public Dron(Texture2D texture)
            : base(3, 1, texture)
        {
        }

        public override void Update()
        {
            this.location.X -= (float)(this.Speed * Math.Cos(this.EnemyAngle));
            this.location.Y -= (float)(this.Speed * Math.Sin(this.EnemyAngle));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle source = new Rectangle(0,0,this.Texture.Width,this.Texture.Height);
            Vector2 origin = new Vector2(this.Texture.Width/2.0f,this.Texture.Height/2.0f);
            spriteBatch.Draw(Texture, Location, source, Color.White, EnemyAngle, origin, 1.0f, SpriteEffects.None, 1);
        }
    }
}
