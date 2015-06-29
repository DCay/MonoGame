using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
        public bool ToDraw = false;

        public Ship_Projectile1(Texture2D texture, Vector2 location,bool projectile_IsShot)
        {
            this.Texture = texture;
            this.Location = location;
            this.Projectile_IsShot = projectile_IsShot;
        }

        public Vector2 Location { get; set; }

        public void SetPosition(Direction direction, Vector2 location)
        {
            switch (direction)
            {
                case Direction.Up:
                    this.direction = Direction.Up;
                    this.location = location;
                    this.location.X -= 14;
                    break;
                case Direction.Down:
                    this.direction = Direction.Down;
                    this.location = location;
                    this.location.X -= 14;
                    break;
                case Direction.Right:
                    this.direction = Direction.Right;
                    this.location = location;
                    this.location.Y -= 14;
                    break;
                case Direction.Left:
                    this.direction = Direction.Left;
                    this.location = location;
                    this.location.Y -= 14;
                    break;
                //DIAGONALS
                case Direction.UpLeft:
                    this.direction = Direction.UpLeft;
                    this.location = location;
                    this.location.Y -= 12;
                    this.location.X -= 12;
                    break;
                case Direction.UpRight:
                    this.direction = Direction.UpRight;
                    this.location = location;
                    this.location.Y -= 12;
                    this.location.X += 12;
                    break;
                case Direction.DownLeft:
                    this.direction = Direction.DownLeft;
                    this.location = location;
                    this.location.Y += 12;
                    this.location.X -= 12;
                    break;
                case Direction.DownRight:
                    this.direction = Direction.DownRight;
                    this.location = location;
                    this.location.Y -= 12;
                    this.location.X += 12;
                    break;
            }
            Projectile_IsShot = false;
        }

        public void Update(Direction direction,Vector2 location)
        {
            if (Projectile_IsShot)
            {
                SetPosition(direction,location);
                this.ToDraw = true;
            }
            switch (direction)
            {
                case Direction.Up:
                    if (this.location.Y > 0)
                    {
                        this.location.Y -= 20;
                    }
                    else this.ToDraw = false;
                    break;
                case Direction.Down:
                    if (this.location.Y < 5000)
                    {
                        this.location.Y += 20;
                    }
                    else this.ToDraw = false;
                    break;
                case Direction.Right:
                    if (this.location.X < 5000)
                    {
                        this.location.X += 20;
                    }
                    else this.ToDraw = false;
                    break;
                case Direction.Left:
                    if (this.location.X > 0)
                    {
                        this.location.X -= 20;
                    }
                    else this.ToDraw = false;
                    break;
                case Direction.UpLeft:
                    if (this.location.X > 0 && this.location.Y > 0)
                    {
                        this.location.X -= 20;
                        this.location.Y -= 20;
                    }
                    else this.ToDraw = false;
                    break;
                case Direction.UpRight:
                    if (this.location.X < 5000 && this.location.Y>0)
                    {
                        this.location.X += 20;
                        this.location.Y -= 20;
                    }
                    else this.ToDraw = false;
                    break;
                case Direction.DownLeft:
                    if (this.location.X > 0 && this.location.Y < 5000)
                    {
                        this.location.X -= 20;
                        this.location.Y += 20;
                    }
                    else this.ToDraw = false;
                    break;
                case Direction.DownRight:
                    if (this.location.X < 5000 && this.location.Y < 5000)
                    {
                        this.location.X += 20;
                        this.location.Y += 20;
                    }
                    else
                    {
                        this.ToDraw = false;
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (ToDraw)
            {
                spriteBatch.Draw(this.Texture, this.location, Color.White);
            }
        }
    }
}
