using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flyer.Enemies
{
    public abstract class Enemy
    {
        private int hitPoints;
        private double speed;
        static Random rand = new Random();
        protected Vector2 location = new Vector2(rand.Next(0, 5000), rand.Next(0, 5000));
       
        //private int initalX = rand.Next(0, 200);

        protected Enemy(double speed, int hitPoints, Texture2D texture)
        {
            this.Speed = speed;
            this.HitPoints = hitPoints;
            this.Texture = texture;
            this.EnemyAngle = (float)(rand.NextDouble() * 2 * Math.PI);
            this.Location = location;
        }

        public Enemy()
        {
            // TODO: Complete member initialization
        }

        public double Speed { get; set; }
        public int HitPoints { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Location
        {
            get
            {
                return this.location;
            }
            set
            {
                if (value.X < 0 || value.X > 5000 || value.Y < 0 || value.Y > 5000)
                {
                    throw new ArgumentOutOfRangeException("The enemy location X and Y shoud be between 0 and 5000");
                }
                this.location = value;
            }
        }
        public float EnemyAngle { get; set; }

        public virtual void Update()
        {
            this.location.X -= (float)(this.Speed * Math.Cos(this.EnemyAngle));
            this.location.Y -= (float)(this.Speed * Math.Sin(this.EnemyAngle));
            if (this.location.X < -1 || this.location.X > 5001)
            {
                this.EnemyAngle--;
            }
            if (this.location.Y < -1 || this.location.Y > 5001)
            {
                this.EnemyAngle--;
            }
        }

        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
