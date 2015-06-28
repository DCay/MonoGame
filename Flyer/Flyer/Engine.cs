using System;
using System.Collections.Generic;
using System.Threading;
using Flyer.Enemies;
using Flyer.Enums;
using Flyer.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flyer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Engine : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private FuelEngine fuelEngine;
        
        private Texture2D background;
        
        private Ship newShip;
        
        private List<Ship_Projectile1> ship_projectiles; 
        private Texture2D projectileTexture;
        private int projectile_index=0;
        
        private SpriteFont coordinate_font;
        private bool coordinate_Show = false;
        
        private int universalReload = 0;//some junkdata
        
        private Camera camera;
        private KeyboardState currentState,previousState;

        /// <summary>
        /// EENEMY DATA
        /// </summary>
        private Texture2D droneTexture;
        private List<Dron> newDrones;
        private int droneIndex=200;

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            camera = new Camera(GraphicsDevice.Viewport,new Vector2(graphics.PreferredBackBufferWidth/2,graphics.PreferredBackBufferHeight/2));
            currentState = Keyboard.GetState();
            ship_projectiles = new List<Ship_Projectile1>();
            newDrones = new List<Dron>();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("images/background3");

            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("images/circle"));
            textures.Add(Content.Load<Texture2D>("images/star"));
            textures.Add(Content.Load<Texture2D>("images/diamond"));
            fuelEngine = new FuelEngine(textures, new Vector2(2500 , 2500));
            
            Texture2D shipTexture = Content.Load<Texture2D>("images/ship");
            newShip = new Ship(shipTexture,0);
            
            projectileTexture = Content.Load<Texture2D>("images/projectile");
            
            coordinate_font = Content.Load<SpriteFont>("coordinate_font");    

            //ENEMY DATA
            droneTexture = Content.Load<Texture2D>("images/drone");
            for (int i = 0; i < droneIndex; i++)
            {
                Dron nextDron = new Dron(droneTexture);

                if ((nextDron.Location.X - newShip.location.X) * (nextDron.Location.X - newShip.location.X) +
                    (nextDron.Location.Y - newShip.location.Y) * (nextDron.Location.Y - newShip.location.Y) > 2000000)
                {
                    newDrones.Add(nextDron);
                    //newDrones.Remove(newDrones[i]);
                    //droneIndex--;
                }
            }

            for (int i = 0; i < droneIndex; i++)
            {

            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            camera.Position = newShip.location;
            PlayInput();
            //CAMERA MOVEMENT
            previousState = currentState;
            currentState = Keyboard.GetState();
            camera.Update(gameTime,currentState,previousState);
            //END CAMERA MOVEMENT

            //DRONE FACTORY
            for (int i = 0; i < newDrones.Count; i++)
            {
                newDrones[i].Update();
            }
            
            //END

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,null,null,null,null,camera.Transform);
            spriteBatch.Draw(background, new Rectangle(0,0, 5000, 5000), Color.White);
            fuelEngine.Draw(spriteBatch);
            newShip.Draw(spriteBatch);
            for (int i = 0; i < newDrones.Count; i++)
            {
                newDrones[i].Draw(spriteBatch);   
            }

            for (int i = 0; i < ship_projectiles.Count; i++)
            {
                ship_projectiles[i].Draw(spriteBatch);   
            }
            spriteBatch.End();
            spriteBatch.Begin();
            if (coordinate_Show)
            {
                spriteBatch.DrawString(coordinate_font,"Ship.X - " + newShip.location.X,new Vector2(10, 10),Color.White);
                spriteBatch.DrawString(coordinate_font, "Ship.Y - " + newShip.location.Y, new Vector2(10, 30), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }


        //CUSTOM ENGINE METHODS
        //CHAR MOVEMENT
        public void PlayInput()
        {
            KeyboardState state = Keyboard.GetState();
            // CHAR MOVEMENT
            if (state.IsKeyDown(Keys.W)
                && state.IsKeyUp(Keys.S)
                && state.IsKeyUp(Keys.D)
                && state.IsKeyUp(Keys.A))
            {
                fuelEngine.Build();
                if (newShip.location.Y > 0)
                {
                    newShip.location.Y -= 10;
                }
                newShip.ship_angle = 0;
                newShip.ShipDirection = Direction.Up;
                fuelEngine.EmitterLocation = new Vector2(newShip.location.X, newShip.location.Y + 40);
            }

            if (state.IsKeyDown(Keys.D)
                && state.IsKeyUp(Keys.W)
                && state.IsKeyUp(Keys.S)
                && state.IsKeyUp(Keys.A))
            {
                fuelEngine.Build();
                if (newShip.location.X < 5000)
                {
                    newShip.location.X += 10;
                }
                newShip.ship_angle = 1.575f;
                newShip.ShipDirection = Direction.Right;
                fuelEngine.EmitterLocation = new Vector2(newShip.location.X - 40, newShip.location.Y);
            }

            if (state.IsKeyDown(Keys.A)
                && state.IsKeyUp(Keys.W)
                && state.IsKeyUp(Keys.S)
                && state.IsKeyUp(Keys.D))
            {
                fuelEngine.Build();
                if (newShip.location.X > 0)
                {
                    newShip.location.X -= 10;
                }
                newShip.ship_angle = 4.725f;
                newShip.ShipDirection = Direction.Left;
                fuelEngine.EmitterLocation = new Vector2(newShip.location.X + 40, newShip.location.Y);
            }

            if (state.IsKeyDown(Keys.S)
                && state.IsKeyUp(Keys.W)
                && state.IsKeyUp(Keys.A)
                && state.IsKeyUp(Keys.D))
            {
                fuelEngine.Build();
                if (newShip.location.Y < 5000)
                {
                    newShip.location.Y += 10;
                }
                newShip.ship_angle = 3.150f;
                newShip.ShipDirection = Direction.Down;
                fuelEngine.EmitterLocation = new Vector2(newShip.location.X, newShip.location.Y - 40);
            }
            if (universalReload < 15)
            {
                universalReload += 1;

            }
            //DIAGONAL MOVEMENT
            if (state.IsKeyDown(Keys.W) && state.IsKeyDown(Keys.D) && state.IsKeyUp(Keys.A))
            {
                newShip.ShipDirection = Direction.UpRight;
                newShip.ship_angle = 0.7875f;
                if (newShip.location.Y > 0 && newShip.location.X < 5000)
                {
                    newShip.location.Y -= 7.5f;
                    newShip.location.X += 7.5f;
                }
                fuelEngine.EmitterLocation = new Vector2(newShip.location.X - 30, newShip.location.Y + 30);
                fuelEngine.Build();
            }

            if (state.IsKeyDown(Keys.W) && state.IsKeyDown(Keys.A) && state.IsKeyUp(Keys.D))
            {
                newShip.ShipDirection = Direction.UpLeft;
                newShip.ship_angle = 5.5075f;
                if (newShip.location.Y > 0 && newShip.location.X > 0)
                {
                    newShip.location.Y -= 7.5f;
                    newShip.location.X -= 7.5f;
                }
                fuelEngine.EmitterLocation = new Vector2(newShip.location.X + 30, newShip.location.Y + 30);
                fuelEngine.Build();
            }

            if (state.IsKeyDown(Keys.S) && state.IsKeyDown(Keys.A) && state.IsKeyUp(Keys.D))
            {
                newShip.ShipDirection = Direction.DownLeft;
                newShip.ship_angle = 3.9375f;
                if (newShip.location.Y < 5000 && newShip.location.X > 0)
                {
                    newShip.location.Y += 7.5f;
                    newShip.location.X -= 7.5f;
                }
                fuelEngine.EmitterLocation = new Vector2(newShip.location.X + 30, newShip.location.Y - 30);
                fuelEngine.Build();
            }

            if (state.IsKeyDown(Keys.S) && state.IsKeyDown(Keys.D) && state.IsKeyUp(Keys.A))
            {
                newShip.ShipDirection = Direction.DownRight;
                newShip.ship_angle = 2.3625f;
                if (newShip.location.Y < 5000 && newShip.location.X < 5000)
                {
                    newShip.location.Y += 7.5f;
                    newShip.location.X += 7.5f;
                }
                fuelEngine.EmitterLocation = new Vector2(newShip.location.X - 30, newShip.location.Y - 30);
                fuelEngine.Build();
            }

            if (state.IsKeyDown(Keys.F3))
            {
                if (universalReload >= 15)
                {
                    if (coordinate_Show)
                    {
                        coordinate_Show = false;
                        universalReload = 0;

                    }
                    else
                    {
                        coordinate_Show = true;
                        universalReload = 0;
                    }
                }
            }
            fuelEngine.Destroy();
            //END CHAR MOVEMENT

            //CHAR SHOTTING
            if (universalReload >= 15)
            {
                if (state.IsKeyDown(Keys.Space))
                {
                    newShip.directionWhenShot = newShip.ShipDirection;
                    ship_projectiles.Add(new Ship_Projectile1(projectileTexture, newShip.location, false));
                    ship_projectiles[projectile_index].Projectile_IsShot = true;
                    ship_projectiles[projectile_index].direction = newShip.directionWhenShot;
                    projectile_index++;
                }
                universalReload = 0;
            }
            for (int i = 0; i < ship_projectiles.Count; i++)
            {
                ship_projectiles[i].Update(ship_projectiles[i].direction, newShip.location);
                if (ship_projectiles[i].ToDraw == false)
                {
                    ship_projectiles.Remove(ship_projectiles[i]);
                    projectile_index--;
                }
            }
            //END CHAR SHOTTING
        }
    }
}
