using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using The_E_Project.Entities;
using The_E_Project.Levels;

namespace Harder
{
    /// <summary>
    ///     This is the main type for your game.
    /// </summary>
    public class Run : Game
    {
        public static List<Bullet> BulletList; //later use Pool<BulletLogic>

        //change this later to local or smth cuz ill have like 50 levels in the future
        public static BaseLevel currentLevel;

        private static readonly string Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private static readonly string Filename = System.IO.Path.Combine(Path, "LEGENDARYgameData.txt");
        private readonly GraphicsDeviceManager _graphics; //assign to local if possible once game is done
        private MainCharacter _character;
        private SpriteBatch _spriteBatch;

        public Run()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
            //_graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            _graphics.SupportedOrientations = DisplayOrientation.LandscapeRight;
        }

        public static int currentLevelNumber { get; set; }

        /// <summary>
        ///     Allows the game to perform any initialization it needs to before starting to run.
        ///     This is where it can query for any required services and load any non-graphic
        ///     related content.  Calling base.Initialize will enumerate through any components
        ///     and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _character = new MainCharacter(GraphicsDevice);

            //add parameter here for limit size
            BulletList = new List<Bullet>();

            currentLevel = new ExampleLevel();

            //load gameData
            try
            {
                if (!File.Exists(Filename))
                {
                    CreateNewFile();
                }
                else
                {
                    var text = File.ReadAllText(Filename);
                    currentLevelNumber = int.Parse(text.Split('-')[1]);
                }
            }
            catch (Exception)
            {
                //File is corrupted
                //create message on scren saying so? maybe not
                CreateNewFile();
            }

            Content = currentLevel.LoadContent(Content);
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //order matters, bullet is dependant on character
            _character.Update(gameTime);

            foreach (var startingBulletEntity in BulletList)
                startingBulletEntity.Update(gameTime);

            currentLevel.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            _spriteBatch.Begin();

            //add drawing logic here

            _character.Draw(_spriteBatch);

            foreach (var bulletLogic in BulletList)
            {
                bulletLogic.Draw(_spriteBatch);
            }

            currentLevel.Draw(_spriteBatch);


            //until here
            _spriteBatch.End();

            base.Draw(gameTime);
        }


        private void CreateNewFile()
        {
            var textToFile = @"currentlevel-0";
            File.WriteAllText(Filename, textToFile);
            currentLevelNumber = 0;
        }
    }
}