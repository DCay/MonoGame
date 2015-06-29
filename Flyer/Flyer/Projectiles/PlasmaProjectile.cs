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
    public class PlasmaProjectile : Projectile
    {
        public PlasmaProjectile(Texture2D texture, Vector2 location, Direction direction, float angle)
            : base(texture, location, direction, angle)
        {
            this.speed = 30;
        }


    }
}
