using Flyer.Enums;
using Flyer.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flyer.Projectiles.EnemyProjectiles
{
    public class EnemyProjectile : Projectile
    {
        public bool isAlive = false;
        public EnemyProjectile(Texture2D texture, Vector2 location, Direction direction)
            : base(texture, location, direction)
        {
            this.speed = 1;

        }

        public override void SetPosition(Vector2 target)
        {
            //this.Angle = (float)Math.Atan(Math.Abs((this.Location.Y - target.Y)) / Math.Abs((target.X - this.Location.X)));

            if (target.X > this.location.X && target.Y < this.location.Y)
            {
                this.Angle = (float)Math.Atan((this.Location.Y - target.Y) / (target.X - this.Location.X));
            }
            else if (target.X < this.location.X && target.Y < this.location.Y)
            {
                this.Angle = (float)Math.PI - (float)Math.Atan((this.Location.Y - target.Y) / (-target.X + this.Location.X));
            }
            else if (target.X < this.location.X && target.Y > this.location.Y)
            {
                this.Angle = (float)Math.PI + (float)Math.Atan((target.Y - this.Location.Y) / (-target.X + this.Location.X));
            }
            else
            {
                this.Angle = 3 * (float)Math.PI / 2 + (float)Math.Atan((target.X - this.Location.X) / (target.Y - this.Location.Y));
            }
        }

        public override void Update()
        {

            this.location.X += (float)(this.Speed * Math.Cos(this.Angle));
            this.location.Y -= (float)(this.Speed * Math.Sin(this.Angle));
            if (this.location.X > 5001 || this.location.Y > 5001) this.isAlive = false;
        }

    }
}
