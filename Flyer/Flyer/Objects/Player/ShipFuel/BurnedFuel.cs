using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = Flyer.Interfaces.IDrawable;

namespace Flyer
{
    class BurnedFuel : IDrawable
    {
        public Texture2D Texture { get; set; }
        public Vector2 Location { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public Color Color { get; set; }
        public float Size { get; set; }
        public int Lifespan { get; set; }

        public BurnedFuel(Texture2D texture, Vector2 position, Vector2 velocity, float angle, float angularVelocity, Color color, float size, int lifespan)
        {
            Texture = texture;
            Location = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            Lifespan = lifespan;
        }

        public void Update()
        {
            Lifespan--;
            Location += Velocity;
            Angle += AngularVelocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle source_rect = new Rectangle(0,0,Texture.Width,Texture.Height);
            Vector2 origin = new Vector2(Texture.Width/2,Texture.Height/2);

            spriteBatch.Draw(Texture, Location, source_rect, Color,
        Angle, origin, Size, SpriteEffects.None, 0f);
        }
    }
}
