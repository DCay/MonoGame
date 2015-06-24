using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flyer.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Flyer
{
    class Ship
    {
        public Texture2D Texture { get; set; }
        public Vector2 location = new Vector2(2500,2500);
        public float ship_angle { get; set; }
        public Direction ShipDirection = Direction.Up;

        public Ship(Texture2D texture, float shipAngle)
        {
            this.Texture = texture;
            this.location = location;
            ship_angle = shipAngle;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0,0,Texture.Width,Texture.Height);
            Vector2 origin = new Vector2(Texture.Width/2,Texture.Height/2);
            spriteBatch.Draw(Texture,location,sourceRectangle,Color.White,ship_angle,origin,1.0f,SpriteEffects.None,1);
        }
    }
}
