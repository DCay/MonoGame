using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flyer.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flyer.Interfaces
{
    public abstract class Projectile : IProjectile
    {
        private Vector2 location;
        private float angle;
        protected int speed;

        protected Projectile(Texture2D texture, Vector2 location, Direction direction, float angle)
        {
            this.Texture = texture;
            this.location = location;
            this.Direction = direction;
            this.ToDraw = true;
            this.angle = angle;
        }

        public bool ToDraw { get; set; }
        public Texture2D Texture { get; private set; }
        public Direction Direction { get; private set; }
        public Vector2 Location
        { 
            get { return this.location; }
        }

        public float Angle
        {
            get { return this.angle; }
            private set { this.angle = value; }
        }

        public virtual void Update()
        {
            switch (this.Direction)
            {
                case Direction.Up:
                    this.location.Y -= this.speed;
                    break;
                case Direction.Down:
                    this.location.Y += this.speed;
                    break;
                case Direction.Right:
                    this.location.X += this.speed;
                    break;
                case Direction.Left:
                    this.location.X -= this.speed;
                    break;
                //DIAGONALS
                case Direction.UpLeft:
                    this.location.Y -= this.speed / 2;
                    this.location.X -= this.speed / 2;
                    break;
                case Direction.UpRight:
                    this.location.Y -= this.speed / 2;
                    this.location.X += this.speed / 2;
                    break;
                case Direction.DownLeft:
                    this.location.Y += this.speed / 2;
                    this.location.X -= this.speed / 2;
                    break;
                case Direction.DownRight:
                    this.location.Y += this.speed / 2;
                    this.location.X += this.speed / 2;
                    break;
            }
            if (this.Location.X < 0 || this.Location.X > 5000 || this.Location.Y < 0 || this.Location.Y > 5000)
            {
                this.ToDraw = false;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (this.ToDraw)
            {
                Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
                Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
                spriteBatch.Draw(this.Texture, this.Location, sourceRectangle, Color.White, this.Angle, origin, 1.0f, SpriteEffects.None, 1);
            }
        }
    }
}
