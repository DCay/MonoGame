using Flyer.Enums;
using Flyer.Interfaces;
using Flyer.Projectiles.EnemyProjectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flyer.Projectiles
{
    class BlackBall : EnemyProjectile
    {
        public BlackBall(Texture2D texture, Vector2 location, Direction direction)
            : base(texture, location, direction)
        {
            this.speed = 30;
        }
    }
}
