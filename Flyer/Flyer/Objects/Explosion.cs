using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flyer.Objects
{
    class Explosion
    {
        public Texture2D Texture;
        public bool toDraw = false;
        public Vector2 Location;
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

        public void Draw(SpriteBatch spriteBatch)
        {
            if (toDraw)
            {
                spriteBatch.Draw(this.Texture,Location,Color.White);
            }
        }
    }
}
