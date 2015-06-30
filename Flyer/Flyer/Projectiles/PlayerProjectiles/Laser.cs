using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flyer.Enums;
using Flyer.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flyer.Projectiles
{
    public class Laser : Projectile
    {
        public Laser(Texture2D texture, Vector2 location, Direction direction) 
            : base(texture, location, direction)
        {
            this.speed = 50;
        }
    }
}
