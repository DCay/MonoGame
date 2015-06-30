using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flyer.Enums;
using Flyer.Interfaces;
using Flyer.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Flyer
{
    class Ship
    {
        public Texture2D Texture { get; set; }
        public Vector2 location = new Vector2(2500,2500);
        public float ship_angle { get; set; }
        public Direction ShipDirection = Direction.Up;
        public Direction directionWhenShot;
        public int PlayerHP { get; set; }
        public int PlayerShields { get; set; }
        private FuelEngine fuelEngine;

        //PROJECTILE DATA
        public List<IProjectile> shipProjectiles= new List<IProjectile>();
        public Texture2D ProjectileTexture;
        public string ProjectileType="bullet";

        //JUNK DATA
        private int detailsReload = 0;

        public Vector2 Origin
        {
            get
            {
                return new Vector2(Texture.Width/2.0f,Texture.Height/2.0f);
            }
        }

        public Ship(Texture2D texture, float shipAngle,List<Texture2D> fuelEngineTexture)
        {
            this.Texture = texture;
            this.location = location;
            ship_angle = shipAngle;
            this.PlayerHP = 100;
            this.PlayerShields = 200;
            fuelEngine = new FuelEngine(fuelEngineTexture,new Vector2(2500, 2500),Color.OrangeRed);
        }

        public void Update(KeyboardState state)
        {
            if(this.PlayerHP<=0)Environment.Exit(0);
            //MOVEMENT
            if (state.IsKeyDown(Keys.W)
                && state.IsKeyUp(Keys.S)
                && state.IsKeyUp(Keys.D)
                && state.IsKeyUp(Keys.A))
            {
                fuelEngine.Build();
                if (this.location.Y > 0)
                {
                    this.location.Y -= 10;
                }
                this.ship_angle = 0;
                this.ShipDirection = Direction.Up;
                fuelEngine.EmitterLocation = new Vector2(this.location.X, this.location.Y + 40);
            }

            if (state.IsKeyDown(Keys.D)
                && state.IsKeyUp(Keys.W)
                && state.IsKeyUp(Keys.S)
                && state.IsKeyUp(Keys.A))
            {
                fuelEngine.Build();
                if (this.location.X < 5000)
                {
                    this.location.X += 10;
                }
                this.ship_angle = 1.575f;
                this.ShipDirection = Direction.Right;
                fuelEngine.EmitterLocation = new Vector2(this.location.X - 40, this.location.Y);
            }

            if (state.IsKeyDown(Keys.A)
                && state.IsKeyUp(Keys.W)
                && state.IsKeyUp(Keys.S)
                && state.IsKeyUp(Keys.D))
            {
                fuelEngine.Build();
                if (this.location.X > 0)
                {
                    this.location.X -= 10;
                }
                this.ship_angle = 4.725f;
                this.ShipDirection = Direction.Left;
                fuelEngine.EmitterLocation = new Vector2(this.location.X + 40, this.location.Y);
            }

            if (state.IsKeyDown(Keys.S)
                && state.IsKeyUp(Keys.W)
                && state.IsKeyUp(Keys.A)
                && state.IsKeyUp(Keys.D))
            {
                fuelEngine.Build();
                if (this.location.Y < 5000)
                {
                    this.location.Y += 10;
                }
                this.ship_angle = 3.150f;
                this.ShipDirection = Direction.Down;
                fuelEngine.EmitterLocation = new Vector2(this.location.X, this.location.Y - 40);
            }
            //DIAGONAL MOVEMENT
            if (state.IsKeyDown(Keys.W) && state.IsKeyDown(Keys.D) && state.IsKeyUp(Keys.A))
            {
                this.ShipDirection = Direction.UpRight;
                this.ship_angle = 0.7875f;
                if (this.location.Y > 0 && this.location.X < 5000)
                {
                    this.location.Y -= 7.5f;
                    this.location.X += 7.5f;
                }
                fuelEngine.EmitterLocation = new Vector2(this.location.X - 30, this.location.Y + 30);
                fuelEngine.Build();
            }

            if (state.IsKeyDown(Keys.W) && state.IsKeyDown(Keys.A) && state.IsKeyUp(Keys.D))
            {
                this.ShipDirection = Direction.UpLeft;
                this.ship_angle = 5.5075f;
                if (this.location.Y > 0 && this.location.X > 0)
                {
                    this.location.Y -= 7.5f;
                    this.location.X -= 7.5f;
                }
                fuelEngine.EmitterLocation = new Vector2(this.location.X + 30, this.location.Y + 30);
                fuelEngine.Build();
            }

            if (state.IsKeyDown(Keys.S) && state.IsKeyDown(Keys.A) && state.IsKeyUp(Keys.D))
            {
                this.ShipDirection = Direction.DownLeft;
                this.ship_angle = 3.9375f;
                if (this.location.Y < 5000 && this.location.X > 0)
                {
                    this.location.Y += 7.5f;
                    this.location.X -= 7.5f;
                }
                fuelEngine.EmitterLocation = new Vector2(this.location.X + 30, this.location.Y - 30);
                fuelEngine.Build();
            }

            if (state.IsKeyDown(Keys.S) && state.IsKeyDown(Keys.D) && state.IsKeyUp(Keys.A))
            {
                this.ShipDirection = Direction.DownRight;
                this.ship_angle = 2.3625f;
                if (this.location.Y < 5000 && this.location.X < 5000)
                {
                    this.location.Y += 7.5f;
                    this.location.X += 7.5f;
                }
                fuelEngine.EmitterLocation = new Vector2(this.location.X - 30, this.location.Y - 30);
                fuelEngine.Build();
            }
            fuelEngine.Destroy();

            //CHAR SHOTTING
            detailsReload++;
            if (detailsReload >= 15)
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    this.directionWhenShot = this.ShipDirection;
                    switch (this.ProjectileType)
                    {
                        case "bullet":
                            shipProjectiles.Add(new Bullet(ProjectileTexture, this.location, 
                                this.directionWhenShot));
                            break;
                        case "laser":
                            shipProjectiles.Add(new Laser(ProjectileTexture, this.location,
                                this.directionWhenShot));
                            break;
                        case "plasma":
                            shipProjectiles.Add(new PlasmaProjectile(ProjectileTexture, this.location,
                                this.directionWhenShot));
                            break;
                    }
                }
                detailsReload = 0;
            }
            for (int i = 0; i < shipProjectiles.Count; i++)
            {
                shipProjectiles[i].Update();
                if (shipProjectiles[i].ToDraw == false)
                {
                    shipProjectiles.Remove(shipProjectiles[i]);
                }
            }
            //END CHAR SHOTTING
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0,0,Texture.Width,Texture.Height);
            Vector2 origin = new Vector2(Texture.Width/2,Texture.Height/2);
            spriteBatch.Draw(Texture,location,sourceRectangle,Color.White,ship_angle,origin,1.0f,SpriteEffects.None,1);
            fuelEngine.Draw(spriteBatch);
            for (int i = 0; i < shipProjectiles.Count; i++)
            {
                shipProjectiles[i].Draw(spriteBatch);
            }
        }
    }
}
