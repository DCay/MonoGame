using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = Flyer.Interfaces.IDrawable;

namespace Flyer.Objects
{
    class Explosion : IDrawable
    {
        public Texture2D texture;
        public bool toDraw = false;
        public Vector2 location;
        private int explosionTimer = 0;

        public Explosion(Texture2D texture, Vector2 location)
        {
            this.Texture = texture;
            this.Location = location;
            this.toDraw = true;
        }

        public void Update()
        {
            explosionTimer++;
            if (explosionTimer >= 10)
            {
                this.toDraw = false;
                explosionTimer = 0;
            }

        }

        public Texture2D Texture { get; set; }
        public Vector2 Location { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (toDraw)
            {
                spriteBatch.Draw(this.Texture, Location, Color.White);
            }
        }
    }
}
