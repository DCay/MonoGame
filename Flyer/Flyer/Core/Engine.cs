using System;
using System.Collections.Generic;
using System.Threading;
using Flyer.Core;
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
        //UNIVERSAL DATA
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Texture2D background;
        private SpriteFont universalFont;


        //PLAYER DATA
        private Ship newShip;
        private FuelEngine fuelEngine;
        private int playerScore=0;
        private SpriteFont score_font;    
        private SpriteFont coordinate_font;
        private bool coordinate_Show = false;
        
        //PROJECTILE DATA
        private List<Ship_Projectile1> ship_projectiles; 
        private Texture2D projectileTexture;
        private int projectile_index=0;

        //CAMERA DATA
        private Camera camera;
        private KeyboardState currentState,previousState;

        /// <summary>
        /// EENEMY DATA
        /// </summary>
        private Texture2D droneTexture;
        private List<Dron> newDrones;
        private int droneIndex=200;

        //JUNK DATA
        private int universalReload = 0;
        private Texture2D explosionTexture;
        private bool explosionDraw = false;
        private Vector2 explosionLocation;
        private int explosionTimer = 0;

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
            camera.cameraY = 2500;
            camera.cameraX = 2500;
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
            
            //SHIP DATA
            Texture2D shipTexture = Content.Load<Texture2D>("images/ship");
            newShip = new Ship(shipTexture,0);
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("images/circle"));
            textures.Add(Content.Load<Texture2D>("images/star"));
            textures.Add(Content.Load<Texture2D>("images/diamond"));
            fuelEngine = new FuelEngine(textures, new Vector2(2500, 2500));
            
            //PROJECTILE DATA
            projectileTexture = Content.Load<Texture2D>("images/projectile");

            explosionTexture = Content.Load<Texture2D>("images/explosion");

            //FONT DATA
            universalFont = Content.Load<SpriteFont>("UniversalFont");
            coordinate_font = universalFont;
            score_font = universalFont;
            
            //ENEMY DATA
            droneTexture = Content.Load<Texture2D>("images/drone");
            for (int i = 0; i < droneIndex; i++)
            {
                Dron nextDron = new Dron(droneTexture);

                if (Math.Sqrt((nextDron.Location.X - newShip.location.X) * (nextDron.Location.X - newShip.location.X) +
                    (nextDron.Location.Y - newShip.location.Y) * (nextDron.Location.Y - newShip.location.Y)) > 1400)
                {
                    newDrones.Add(nextDron);
                }
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
            CameraCheck();
            PlayInput();
            //CAMERA MOVEMENT
            previousState = currentState;
            currentState = Keyboard.GetState();
            camera.Update(gameTime,currentState,previousState);
            //END CAMERA MOVEMENT

            //BATTLE STATUS CHECK
            CheckBattleStats();
            //END

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
            ExplosionDraw(spriteBatch);
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
                spriteBatch.DrawString(coordinate_font, "Ship.Y - " + newShip.location.Y, new Vector2(10, 50), Color.White);
                spriteBatch.DrawString(coordinate_font, "Enemies - " + newDrones.Count, new Vector2(10, 90), Color.White);
            }
            spriteBatch.DrawString(score_font,"Score : " + playerScore,new Vector2(graphics.PreferredBackBufferWidth-150,20),Color.White);
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
                    camera.cameraY -= 10;
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
                    camera.cameraX += 10;
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
                    camera.cameraX -= 10;
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
                    camera.cameraY += 10;
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
                    camera.cameraY -= 7.5f;
                    camera.cameraX += 7.5f;
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
                    camera.cameraY -= 7.5f;
                    camera.cameraX -= 7.5f;
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
                    camera.cameraY += 7.5f;
                    camera.cameraX -= 7.5f;
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
                    camera.cameraY += 7.5f;
                    camera.cameraX += 7.5f;
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
        //BATTLE STATUS
        public void CheckBattleStats()
        {
            int index = BattleManager.CheckHitStatus(ship_projectiles, newDrones);
            if (index != -1)
            {
                playerScore += 10;
                explosionDraw = true;
                explosionTimer = 0;
                explosionLocation = new Vector2((newDrones[index].Location.X - newDrones[index].Texture.Width)
                    , (newDrones[index].Location.Y - newDrones[index].Texture.Height));
                newDrones.Remove(newDrones[index]);
            }
        }
        //EXPLOSIONS
        public void ExplosionDraw(SpriteBatch spriteBatch)
        {

            if (explosionDraw && explosionTimer < 10)
            {
                explosionTimer++;
                spriteBatch.Draw(explosionTexture, explosionLocation
                    , new Rectangle(0, 0, explosionTexture.Width, explosionTexture.Height), Color.White);
            }
            else explosionDraw = false;
        }
        //CAMERA CHECK
        public void CameraCheck()
        {
            if (newShip.location.X > (5000 - graphics.PreferredBackBufferWidth / 2))
            {
                camera.cameraX = (5000 - graphics.PreferredBackBufferWidth / 2);
            }

            if (newShip.location.Y > (5000 - graphics.PreferredBackBufferHeight / 2))
            {
                camera.cameraY = (5000 - graphics.PreferredBackBufferHeight / 2);
            }

            if (newShip.location.X < graphics.PreferredBackBufferWidth / 2)
            {
                camera.cameraX = (graphics.PreferredBackBufferWidth / 2);
            }

            if (newShip.location.Y < graphics.PreferredBackBufferHeight / 2)
            {
                camera.cameraY = (graphics.PreferredBackBufferHeight / 2);
            }
            camera.Position = new Vector2(camera.cameraX, camera.cameraY);
        }
    }
}
