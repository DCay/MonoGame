using System.Collections.Generic;
using System.Threading;
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
        private Ship_Projectile1 ship_projectile;
        private SpriteFont coordinate_font;
        private bool coordinate_Show = false;
        private int sometimer = 0;//some junkdata
        private Camera camera;
        private KeyboardState currentState,previousState;

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
            background = Content.Load<Texture2D>("images/background2");

            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("images/circle"));
            textures.Add(Content.Load<Texture2D>("images/star"));
            textures.Add(Content.Load<Texture2D>("images/diamond"));
            fuelEngine = new FuelEngine(textures, new Vector2(2500 , 2500));
            
            Texture2D shipTexture = Content.Load<Texture2D>("images/ship");
            newShip = new Ship(shipTexture,0);
            
            Texture2D texture = Content.Load<Texture2D>("images/projectile");
            Vector2 projectile_location = newShip.location;
            
            coordinate_font = Content.Load<SpriteFont>("coordinate_font");
            ship_projectile=new Ship_Projectile1(texture,projectile_location,false);
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
            // CHAR MOVEMENT
            KeyboardState state = Keyboard.GetState();
            camera.Position = newShip.location;
            if (state.IsKeyDown(Keys.Up))
            {
                fuelEngine.Build();
                newShip.location.Y-=10;
                newShip.ship_angle = 0;
                newShip.ShipDirection=Direction.Up;
                fuelEngine.EmitterLocation = new Vector2(newShip.location.X, newShip.location.Y + 50);
                camera.Position = newShip.location;
            }

            if (state.IsKeyDown(Keys.Right))
            {
                fuelEngine.Build();
                newShip.location.X += 10;
                newShip.ship_angle = 1.58f;
                newShip.ShipDirection=Direction.Right;
                fuelEngine.EmitterLocation = new Vector2(newShip.location.X-50, newShip.location.Y);
                camera.Position = newShip.location;
            }

            if (state.IsKeyDown(Keys.Left))
            {
                fuelEngine.Build();
                newShip.location.X -= 10;
                newShip.ship_angle = 4.70f;
                newShip.ShipDirection = Direction.Left;
                fuelEngine.EmitterLocation = new Vector2(newShip.location.X+50, newShip.location.Y);
                camera.Position = newShip.location;
            }

            if (state.IsKeyDown(Keys.Down))
            {
                fuelEngine.Build();
                newShip.location.Y += 10;
                newShip.ship_angle = 3.15f;
                newShip.ShipDirection = Direction.Down;
                fuelEngine.EmitterLocation = new Vector2(newShip.location.X, newShip.location.Y - 50);
                camera.Position = newShip.location;
            }
            if (sometimer < 15)
            {
                sometimer+=1;
                
            }
            if (state.IsKeyDown(Keys.F3))
            {
                if (sometimer >= 15)
                {
                    if (coordinate_Show)
                    {
                        coordinate_Show = false;
                        sometimer = 0;

                    }
                    else
                    {
                        coordinate_Show = true;
                        sometimer = 0;
                    }
                }
            }
            fuelEngine.Destroy();
            //END CHAR MOVEMENT
            //CHAR SHOTTING
            if (state.IsKeyDown(Keys.Space))
            {
                switch (newShip.ShipDirection)
                {
                    case Direction.Up:
                        ship_projectile.direction = Direction.Up;
                        ship_projectile.location = newShip.location;
                        ship_projectile.location.X -= 12;
                        ship_projectile.Projectile_IsShot = true;
                        break;
                    case Direction.Down:
                        ship_projectile.direction = Direction.Down;
                        ship_projectile.location = newShip.location;
                        ship_projectile.location.X -= 13;
                        ship_projectile.Projectile_IsShot = true;
                        break;
                    case Direction.Right:
                        ship_projectile.direction = Direction.Right;
                        ship_projectile.location = newShip.location;
                        ship_projectile.location.Y -= 14;
                        ship_projectile.Projectile_IsShot = true;
                        break;
                    case Direction.Left:
                        ship_projectile.direction = Direction.Left;
                        ship_projectile.location = newShip.location;
                        ship_projectile.location.Y -= 12;
                        ship_projectile.Projectile_IsShot = true;
                        break;
                }
            }
            ship_projectile.Update();
            //END CHAR SHOTTING
            //CAMERA MOVEMENT
            previousState = currentState;
            currentState = Keyboard.GetState();
            camera.Update(gameTime,currentState,previousState);

            //END CAMERA MOVEMENT
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
            ship_projectile.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
            if (coordinate_Show)
            {
                spriteBatch.DrawString(coordinate_font,"Ship.X - " + newShip.location.X,new Vector2(10,10),Color.White);
                spriteBatch.DrawString(coordinate_font, "Ship.Y - " + newShip.location.Y, new Vector2(10, 30), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
