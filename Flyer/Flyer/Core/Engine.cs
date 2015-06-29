using System;
using System.Collections.Generic;
using System.Threading;
using Flyer.Core;
using Flyer.Enemies;
using Flyer.Enums;
using Flyer.Factories;
using Flyer.Interfaces;
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
        private int playerScore=0;
        private SpriteFont score_font;    
        private SpriteFont coordinate_font;
        private Texture2D playerHPbar;
        private Texture2D playerShieldbar;
        private Texture2D bulletTexture;
        private Texture2D laserTexture;
        private Texture2D plasmaTexture;
        
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
        private Texture2D redbar;
        private Texture2D bluebar;
        private double redBarIndex=195;
        private double blueBarIndex = 190;


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
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("images/circle"));
            textures.Add(Content.Load<Texture2D>("images/star"));
            textures.Add(Content.Load<Texture2D>("images/diamond"));
            playerHPbar = Content.Load<Texture2D>("images/statbar");
            playerShieldbar = playerHPbar;
            redbar = Content.Load<Texture2D>("images/redline");
            bluebar = Content.Load<Texture2D>("images/blueline");
            newShip = new Ship(shipTexture, 0,textures);

            //PROJECTILE DATA
            bulletTexture = Content.Load<Texture2D>("images/projectile");
            plasmaTexture = Content.Load<Texture2D>("images/plasma");
            laserTexture = Content.Load<Texture2D>("images/laser");

            explosionTexture = Content.Load<Texture2D>("images/explosion");

            //FONT DATA
            universalFont = Content.Load<SpriteFont>("UniversalFont");
            coordinate_font = universalFont;
            score_font = universalFont;
            
            //ENEMY DATA
            droneTexture = Content.Load<Texture2D>("images/drone");
            EnemyFactory.Build(droneTexture, newDrones, newShip.location);
            newShip.ProjectileTexture = bulletTexture;
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
            KeyboardState state = Keyboard.GetState();
            CameraCheck();
            newShip.Update(state);
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
            Outsiders.Update(state);
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
            newShip.Draw(spriteBatch);
            ExplosionDraw(spriteBatch);
            for (int i = 0; i < newDrones.Count; i++)
            {
                newDrones[i].Draw(spriteBatch);   
            }
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.DrawString(score_font,"Score : " + playerScore,new Vector2(graphics.PreferredBackBufferWidth-150,20),Color.White);
            //THE BULLSHIT METHOD !!!
            Outsiders.Draw(spriteBatch,universalFont,newShip.location,playerHPbar,redbar,bluebar,redBarIndex,blueBarIndex);
            //END OF BULLSHIT !!!
            spriteBatch.End();

            base.Draw(gameTime);
        }


        //CUSTOM ENGINE METHODS
        //BATTLE STATUS
        public void CheckBattleStats()
        {
            int index = BattleManager.CheckHitStatus(newShip.shipProjectiles, newDrones);
            if (index != -1)
            {
                playerScore += 10;
                explosionDraw = true;
                explosionTimer = 0;
                explosionLocation = new Vector2((newDrones[index].Location.X - newDrones[index].Texture.Width)
                    , (newDrones[index].Location.Y - newDrones[index].Texture.Height));
                newDrones.Remove(newDrones[index]);
                index = -1;
            }
            int index2 = BattleManager.CheckCollisionStatus(newShip, newDrones);
            if(index2!=-1)
            {
                playerScore += 10;
                explosionDraw = true;
                explosionTimer = 0;
                explosionLocation = new Vector2((newDrones[index2].Location.X - newDrones[index2].Texture.Width)
                    , (newDrones[index2].Location.Y - newDrones[index2].Texture.Height));
                newDrones.Remove(newDrones[index2]);
                newShip.PlayerHP -= 20;
                index2 = -1;
            }
            redBarIndex = (195 * newShip.PlayerHP) / 100;
            blueBarIndex = (190 * newShip.PlayerShields) / 200;
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
