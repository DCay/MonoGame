using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flyer
{
    public class Camera
    {
        /// <summary>
        /// PROPERTIES & FIELDS
        /// </summary>
        public Viewport Viewpoint { get; private set; }

        public Vector2 Position { get; set; }
        
        public Vector2 SavedPosition { get; private set; }

        public Vector2 Center { get; private set; }

        public float Zoom { get; private set; }

        public float Rotation { get; private set; }

        public float SavedRotation { get; private set; }

        public float PositionShakeAmount { get; private set; }

        public float RotationShakeAmount { get; private set; }

        public float MaxShakeTime { get; private set; }

        public Matrix Transform { get; private set; }

        TimeSpan shakeTimer;
        Random random;
        
        /// <summary>
        /// CONSTRUCTORS
        /// </summary>
        /// <param name="view"></param>
        /// <param name="position"></param>
        public Camera(Viewport view, Vector2 position)
        {
            this.Viewpoint = view;
            this.Position = position;
            this.Zoom = 1.0f;
            this.Rotation = 0;
            this.random = new Random();
        }

        public Camera(Viewport view, Vector2 position,float zoom,float rotation)
        {
            this.Viewpoint = view;
            this.Position = position;
            this.Zoom = zoom;
            this.Rotation = rotation;
            this.random = new Random();
        }

        /// <summary>
        /// Update Method
        /// </summary>
        public void Update(GameTime gametime, KeyboardState current, KeyboardState previous)
        {
            if (shakeTimer.TotalSeconds > 0)
            {
                this.Position = SavedPosition;
                this.Rotation = SavedRotation;
                shakeTimer = shakeTimer.Subtract(gametime.ElapsedGameTime);
                if (shakeTimer.TotalSeconds > 0)
                {
                    this.Position += new Vector2((float) ((random.NextDouble()*2) - 1)*PositionShakeAmount,
                        (float) ((random.NextDouble()*2) - 1)*PositionShakeAmount);
                    this.Rotation += (float) ((random.NextDouble()*2) - 1)*RotationShakeAmount;
                }
            }
            else
            {
                if (current.IsKeyDown(Keys.Z))
                    this.Zoom += 0.002f;
                if (current.IsKeyDown(Keys.X))
                    this.Zoom -= 0.002f;


                Vector2 movement = Vector2.Zero;
                if (current.IsKeyDown(Keys.Up))
                    movement.Y -= 10.0f/Zoom;
                if (current.IsKeyDown(Keys.Down))
                    movement.Y += 10.0f / Zoom;
                if (current.IsKeyDown(Keys.Left))
                    movement.X -= 10.0f / Zoom;
                if (current.IsKeyDown(Keys.Right))
                    movement.X += 10.0f / Zoom;

                Position = Vector2.Add(Position, movement);
            }

            Center = new Vector2(Position.X - Viewpoint.Width / 2, Position.Y - Viewpoint.Height / 2);

            Transform = Matrix.CreateScale(new Vector3((float) Math.Pow(Zoom, 10), (float) Math.Pow(Zoom, 10), 0))*
                        Matrix.CreateRotationZ(Rotation)*
                        Matrix.CreateTranslation(new Vector3(-Center.X, -Center.Y, 0));
        }

        /// <summary>
        /// SHAKY SHAKY !
        /// </summary>
        public void Shake(float shakeTime, float positionAmount, float rotationAmount)
        {
            if (shakeTimer.TotalSeconds <= 0)
            {
                MaxShakeTime = shakeTime;
                shakeTimer = TimeSpan.FromSeconds(MaxShakeTime);
                PositionShakeAmount = positionAmount;
                RotationShakeAmount = rotationAmount;

                SavedPosition = Position;
                SavedRotation = Rotation;
            }
        }


    }
}
