using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flyer.Enemies
{
    public class Mine : Enemy
    {
        public readonly Rectangle source;

        public Mine(Texture2D texture)
            : base(0.05, 1, texture)
        {
            this.source = new Rectangle(0, 0, this.Texture.Width, this.Texture.Height);
        }

        public override void Update(Vector2 location)
        {
            this.location.X -= (float)(this.Speed * Math.Cos(this.EnemyAngle));
            this.location.Y -= (float)(this.Speed * Math.Sin(this.EnemyAngle));
            if (this.location.X < -1 || this.location.X > 5001)
            {
                this.EnemyAngle--;
            }
            if (this.location.Y < -1 || this.location.Y > 5001)
            {
                this.EnemyAngle--;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(this.Texture.Width / 2.0f, this.Texture.Height / 2.0f);
            spriteBatch.Draw(Texture, Location, source, Color.White, EnemyAngle, origin, 1.0f, SpriteEffects.None, 1);
        }

    }
}
