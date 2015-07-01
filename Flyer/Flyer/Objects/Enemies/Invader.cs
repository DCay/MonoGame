using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flyer.Enemies
{
    public class Invader : Enemy
    {
        public readonly Rectangle source;

        public Invader(Texture2D texture, Texture2D projectileTexture)
            : base(5, 2, texture)
        {
            this.source = new Rectangle(0, 0, this.Texture.Width, this.Texture.Height);
            this.ProjectileTexture = projectileTexture;
            this.HitPoints = 15;
            this.Damage = 30;
            this.reload = 45;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(this.Texture.Width/2.0f,this.Texture.Height/2.0f);
            spriteBatch.Draw(Texture, Location, source, Color.White, EnemyAngle, origin, 1.0f, SpriteEffects.None, 1);
            for (int i = 0; i < this.projectiles.Count; i++)
            {
                projectiles[i].Draw(spriteBatch);
            }
        }
    }
}
