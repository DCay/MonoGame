using System;
using System.Collections.Generic;
using System.Threading;
using Flyer.Bonuses;
using Flyer.Core;
using Flyer.Enemies;
using Flyer.Enums;
using Flyer.Factories;
using Flyer.Interfaces;
using Flyer.Objects;
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
        private SpriteFont score_font;    
        private SpriteFont coordinate_font;
        private Texture2D playerHPbar;
        private Texture2D playerShieldbar;
        public Texture2D bulletTexture;
        public Texture2D laserTexture;
        public Texture2D plasmaTexture;
        
        //CAMERA DATA
        private Camera camera;
        private KeyboardState currentState,previousState;

        /// <summary>
        /// EENEMY DATA
        /// </summary>
        private Texture2D droneTexture;
        private Texture2D invaderTexture;
        private List<Enemy> newDrones;
        private List<Enemy> newInvaders; 
        private int droneIndex=200;
        private List<Enemy> newMines;
        private Texture2D redBallTexture;

        //BONUS DATA
        private Texture2D upgradeTexture;
        private List<WeaponUpgrade> newUpgrades; 

        //JUNK DATA
        private int universalReload = 0;
        private List<Explosion> newExplosions; 
        private Texture2D explosionTexture;
        private int explosionIndex;
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
            newMines = new List<Enemy>();
            newDrones = new List<Enemy>();
            newInvaders = new List<Enemy>();
            newUpgrades=new List<WeaponUpgrade>();
            newExplosions = new List<Explosion>();
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
            bulletTexture = Content.Load<Texture2D>("images/bullet");
            plasmaTexture = Content.Load<Texture2D>("images/plasma");
            laserTexture = Content.Load<Texture2D>("images/laser");
            newShip.ProjectileTexture = bulletTexture;
            explosionTexture = Content.Load<Texture2D>("images/explosion");

            //BONUS DATA
            upgradeTexture = Content.Load<Texture2D>("images/weaponupgrade");

            //FONT DATA
            universalFont = Content.Load<SpriteFont>("UniversalFont");
            coordinate_font = universalFont;
            score_font = universalFont;
            
            //ENEMY DATA
            droneTexture = Content.Load<Texture2D>("images/drone");
            invaderTexture = Content.Load<Texture2D>("images/invader");
            //EnemyFactory.Build(droneTexture, newDrones, newShip.location);
            redBallTexture = Content.Load<Texture2D>("images/redBall");
            var mineTexture = Content.Load<Texture2D>("images/mine");
            EnemyFactory.Build(mineTexture, newMines, newShip.location);
            BattleManager.Initialise(explosionTexture,upgradeTexture);
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
            EnemyFactory.gameTime = gameTime;
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
            UpgradeCheck();
            //END

            //DRONE FACTORY
            EnemyFactory.ProcedureBuild(droneTexture, newDrones, newShip.location, redBallTexture, gameTime);
            
            //END
            
            //ENEMY UPDATE
            for (int i = 0; i < newMines.Count; i++)
            {
                newMines[i].Update(newShip.location);
            }
            for (int i = 0; i < newDrones.Count; i++)
            {
                newDrones[i].Update(newShip.location);
            }
            //END
            for (int i = 0; i < newExplosions.Count; i++)
            {
                newExplosions[i].Update();
            }
            redBarIndex = (195 * newShip.PlayerHP) / 100;
            blueBarIndex = (190 * newShip.PlayerShields) / 200;
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
            
            //LIST DRAWING
            //UPGRADES
            for (int i = 0; i < newUpgrades.Count; i++)
            {
                newUpgrades[i].Draw(spriteBatch);
            }
            //MINES
            for (int i = 0; i < newMines.Count; i++)
            {
                newMines[i].Draw(spriteBatch);
            }
            //DRONES
            for (int i = 0; i < newDrones.Count; i++)
            {
                newDrones[i].Draw(spriteBatch);   
            }
            //EXPLOSIONS
            for (int i = 0; i < newExplosions.Count; i++)
            {
                newExplosions[i].Draw(spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.DrawString(score_font,"Score : " + newShip.playerScore,new Vector2(graphics.PreferredBackBufferWidth-150,20),Color.White);
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
            BattleManager.CheckHitStatus(newShip, newDrones, newExplosions, newUpgrades);
            BattleManager.CheckCollisionStatus(newShip,newMines,newExplosions);
            for (int i = 0; i < newDrones.Count; i++)
            {
                BattleManager.CheckIfShipHit(newDrones[i].projectiles, newShip);
            }
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
        //UPGRADE CHECK
        public void UpgradeCheck()
        {
            for (int i = 0; i < newUpgrades.Count; i++)
            {
                if ((newShip.location.X >= newUpgrades[i].Location.X - newUpgrades[i].Texture.Width / 2)
                    && (newShip.location.X <= newUpgrades[i].Location.X + newUpgrades[i].Texture.Width / 2)
                    && (newShip.location.Y >= newUpgrades[i].Location.Y - newUpgrades[i].Texture.Height / 2)
                    && (newShip.location.Y <= newUpgrades[i].Location.Y + newUpgrades[i].Texture.Height / 2)
                    )
                {
                    newShip.ProjectileType = newUpgrades[i].Upgrade(newShip.ProjectileType);
                    newUpgrades.Remove(newUpgrades[i]);
                }
                if (newShip.ProjectileType == "laser") newShip.ProjectileTexture = laserTexture;
                if (newShip.ProjectileType == "plasma") newShip.ProjectileTexture = plasmaTexture;
            }
        }
    }
}
