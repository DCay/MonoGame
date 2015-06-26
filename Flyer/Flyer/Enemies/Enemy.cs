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
        private int speed;
        static Random rand = new Random();
        protected Vector2 location = new Vector2(rand.Next(0, 5000), rand.Next(0, 5000));
       
        //private int initalX = rand.Next(0, 200);

        protected Enemy(int speed, int hitPoints, Texture2D texture)
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

        public int Speed { get; set; }
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

        public abstract void Update();

        public abstract void Draw(SpriteBatch spriteBatch);

    }
}
