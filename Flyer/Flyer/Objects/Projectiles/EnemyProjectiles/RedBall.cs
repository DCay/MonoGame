﻿using Flyer.Enums;
using Flyer.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flyer.Projectiles.EnemyProjectiles;

namespace Flyer.Projectiles
{
    public class RedBall : EnemyProjectile
    {
        public RedBall(Texture2D texture, Vector2 location, Direction direction)
            : base(texture, location, direction)
        {
            this.speed = 20;
        }
    }
}
