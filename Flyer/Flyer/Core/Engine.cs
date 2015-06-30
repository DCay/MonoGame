using System;
using System.Collections.Generic;
using System.Threading;
using Flyer.Bonuses;
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
        private List<Dron> newDrones;
        private int droneIndex=200;
        private List<Enemy> newMines;

        //BONUS DATA
        private Texture2D upgradeTexture;
        private List<WeaponUpgrade> upgrades; 

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
            newMines = new List<Enemy>();
            upgrades=new List<WeaponUpgrade>();
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

            explosionTexture = Content.Load<Texture2D>("images/explosion");

            //BONUS DATA
            upgradeTexture = Content.Load<Texture2D>("images/weaponupgrade");

            //FONT DATA
            universalFont = Content.Load<SpriteFont>("UniversalFont");
            coordinate_font = universalFont;
            score_font = universalFont;
            
            //ENEMY DATA
            droneTexture = Content.Load<Texture2D>("images/drone");
            //EnemyFactory.Build(droneTexture, newDrones, newShip.location);
            newShip.ProjectileTexture = bulletTexture;
            var mineTexture = Content.Load<Texture2D>("images/mine");
            EnemyFactory.Build(mineTexture, newMines, newShip.location);
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
            UpgradeCheck();
            //END

            //DRONE FACTORY
            for (int i = 0; i < newMines.Count; i++)
            {
                newMines[i].Update();
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
            
            //LIST DRAWING
            //UPGRADES
            for (int i = 0; i < upgrades.Count; i++)
            {
                upgrades[i].Draw(spriteBatch);
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
            //SHOT
            int index = BattleManager.CheckHitStatus(newShip.shipProjectiles, newMines);
            Random chanceToDrop = new Random();
            if (index != -1)
            {
                playerScore += 10;
                
                //EXPLOSION
                explosionDraw = true;
                explosionTimer = 0;
                explosionLocation = new Vector2((newMines[index].Location.X - newMines[index].Texture.Width)
                    , (newMines[index].Location.Y - newMines[index].Texture.Height));
                
                //LOOT
                int isDroped = chanceToDrop.Next(20);
                switch (isDroped)
                {
                    case 1:
                        upgrades.Add(new WeaponUpgrade(upgradeTexture,newMines[index].Location));
                        break;
                    case 8:
                        break;
                    default:
                        break;
                }

                //END LOOT

                newMines.Remove(newMines[index]);
                index = -1;
            }
            //COLLISION
            int index2 = BattleManager.CheckCollisionStatus(newShip, newMines);
            if(index2!=-1)
            {
                playerScore += 10;
                explosionDraw = true;
                explosionTimer = 0;
                explosionLocation = new Vector2((newMines[index2].Location.X - newMines[index2].Texture.Width)
                    , (newMines[index2].Location.Y - newMines[index2].Texture.Height));
                newMines.Remove(newMines[index2]);
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
        //UPGRADE CHECK
        public void UpgradeCheck()
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                if ((newShip.location.X >= upgrades[i].Location.X - upgrades[i].Texture.Width / 2)
                    && (newShip.location.X <= upgrades[i].Location.X + upgrades[i].Texture.Width / 2)
                    && (newShip.location.Y >= upgrades[i].Location.Y - upgrades[i].Texture.Height / 2)
                    && (newShip.location.Y <= upgrades[i].Location.Y + upgrades[i].Texture.Height / 2)
                    )
                {
                    newShip.ProjectileType = upgrades[i].Upgrade(newShip.ProjectileType);
                    upgrades.Remove(upgrades[i]);
                }
                if (newShip.ProjectileType == "laser") newShip.ProjectileTexture = laserTexture;
                if (newShip.ProjectileType == "plasma") newShip.ProjectileTexture = plasmaTexture;
            }
        }
    }
}
