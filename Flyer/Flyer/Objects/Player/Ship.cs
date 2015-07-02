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
using IDrawable = Flyer.Interfaces.IDrawable;


namespace Flyer
{
    public class Ship : IAttack,IDestroyable,IDrawable
    {
        public Texture2D Texture { get; set; }
        public Vector2 location = new Vector2(2500,2500);
        public float ship_angle { get; set; }
        public Direction ShipDirection = Direction.Up;
        public Direction directionWhenShot;
        public int ShieldPoints { get; set; }
        private FuelEngine fuelEngine;
        public int playerScore = 0;
        public string WayToDie { get; set; }

        //PROJECTILE DATA
        public List<IProjectile> projectiles= new List<IProjectile>();
        public Texture2D ProjectileTexture;
        public string ProjectileType="bullet";

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
            this.ship_angle = shipAngle;
            this.HitPoints = 100;
            this.ShieldPoints = 200;
            this.fuelEngine = new FuelEngine(fuelEngineTexture,new Vector2(2500, 2500),Color.OrangeRed);
            this.reload = 15;
            this.reloadCounter = 0;
            this.Damage = 1;
        }

        public void Update(KeyboardState state)
        {
            if (!isDead)
            {
                if (this.HitPoints <= 0)
                {
                    this.isDead = true;
                    Engine.fuN = true;
                }
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
                reloadCounter++;

                if (reloadCounter >= reload)
                {
                    if (state.IsKeyDown(Keys.Space))
                    {
                        this.directionWhenShot = this.ShipDirection;
                        switch (this.ProjectileType)
                        {
                            case "bullet":
                                projectiles.Add(new Bullet(ProjectileTexture, this.location,
                                    this.directionWhenShot));
                                break;
                            case "laser":
                                projectiles.Add(new Laser(ProjectileTexture, this.location,
                                    this.directionWhenShot));
                                break;
                            case "plasma":
                                projectiles.Add(new PlasmaProjectile(ProjectileTexture, this.location,
                                    this.directionWhenShot));
                                break;
                        }
                        reloadCounter = 0;
                    }
                }
                for (int i = 0; i < projectiles.Count; i++)
                {
                    projectiles[i].Update();
                    if (projectiles[i].ToDraw == false)
                    {
                        projectiles.Remove(projectiles[i]);
                    }
                }
                //END CHAR SHOTTING
            }
        }

        public Rectangle sourceRectangle;

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isDead)
            {
                sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
                Vector2 origin = new Vector2(Texture.Width/2, Texture.Height/2);
                spriteBatch.Draw(Texture, location, sourceRectangle, Color.White, ship_angle, origin, 1.0f,
                    SpriteEffects.None, 1);
                fuelEngine.Draw(spriteBatch);
                for (int i = 0; i < projectiles.Count; i++)
                {
                    projectiles[i].Draw(spriteBatch);
                }
            }
        }

        public Vector2 Location
        {
            get { return this.location; }
            set { this.location = value; }
        }
        public int Damage { get; set; }
        public int reload { get; set; }
        public int reloadCounter { get; set; }
        public int HitPoints { get; set; }
        public bool isDead { get; set; }
    }
}
