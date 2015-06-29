using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Flyer.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flyer.Interfaces
{
    public class ShipProjectile : Projectile
    {

        public ShipProjectile(Texture2D texture, Vector2 location, Direction direction, float angle)
            : base(texture, location, direction, angle)
        {
            this.speed = 50;
        }
    }
}
