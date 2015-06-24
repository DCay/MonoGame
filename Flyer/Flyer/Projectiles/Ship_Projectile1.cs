using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flyer.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flyer.Projectiles
{
    public class Ship_Projectile1
    {
        public Texture2D Texture { get; set; }
        public Vector2 location;
        public bool Projectile_IsShot=false;
        public Direction direction=Direction.Up;

        public Ship_Projectile1(Texture2D texture, Vector2 location,bool projectile_IsShot)
        {
            this.Texture = texture;
            this.Location = location;
            this.Projectile_IsShot = projectile_IsShot;
        }

        public Vector2 Location { get; set; }

        public void Update()
        {
            if (Projectile_IsShot)
            {
                switch (direction)
                {
                    case Direction.Up:
                        this.location.Y -= 20;
                        break;
                    case Direction.Down:
                        this.location.Y += 20;
                        break;
                    case Direction.Right:
                        this.location.X += 20;
                        break;
                    case Direction.Left:
                        this.location.X -= 20;
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture,this.location,Color.White);
        }
    }
}
