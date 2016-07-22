using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SolarSystem
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        /// <summary>
        /// Drawn nodes.
        /// </summary>
        private List<ANode> Drawn;

        /// <summary>
        /// Planets to update.
        /// </summary>
        private List<Planet> Updated;

        /// <summary>
        /// Main world transformation.
        /// </summary>
        private Matrix World;

        /// <summary>
        /// The camera we use to draw from.
        /// </summary>
        public StaticCamera Camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1024;
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
            // TODO: Add your initialization logic here
            World = Matrix.Identity;

            RasterizerState state = new RasterizerState();
            state.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = state;

            Camera = new StaticCamera(this, new Vector3(0, 10, -20), Vector3.Zero, new Vector3(0, 1, 0));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            AsteroidManager.Create(this, 900, 150000, 200000, -10000, 60000);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D earthTexture = Content.Load<Texture2D>("textures/earthmap");
            Texture2D moon = Content.Load<Texture2D>("textures/meteor1");
            Texture2D sun = Content.Load<Texture2D>("textures/sun");
            Texture2D mercury = Content.Load<Texture2D>("textures/mercury");
            Texture2D venus = Content.Load<Texture2D>("textures/venus");
            Texture2D mars = Content.Load<Texture2D>("textures/mars");
            Texture2D stars = Content.Load<Texture2D>("textures/stars");

            // Sun, Mercury, Venus, Earth + Moon, Mars,  Asteroids

            // Initialize the lists
            Drawn = new List<ANode>();
            Updated = new List<Planet>();

            // Sun
            Planet planet = new Planet(this)
            {
                Scale = new Vector3(1.5f, 1.5f, 1.5f),
                Texture = sun,

                Orbit = new Vector3(0.5f, 0, 0),
                Spin = new Vector3(0.5f, 0, 0),
            };

            Drawn.Add(planet);
            Updated.Add(planet);

            // Mercury
            planet = new Planet(this)
            {
                Scale = new Vector3(0.4f),
                Texture = mercury,

                Orbit = new Vector3(0.7f, 0, 0),
                Spin = new Vector3(0.7f, 0, 0),

                Origin = new Vector3(7.0f, 0, 0),
            };

            Drawn.Add(planet);
            Updated.Add(planet);

            // Venus
            planet = new Planet(this)
            {
                Scale = new Vector3(0.6f),
                Texture = venus,

                Spin = new Vector3(0.4f, 0, 0),
                Orbit = new Vector3(0.4f, 0, 0),

                Origin = new Vector3(8.0f, 0, 0),
            };

            Drawn.Add(planet);
            Updated.Add(planet);

            // Create the Earth and moon
            Planet earth = new Planet(this)
            {
                Scale = new Vector3(0.6f),
                Texture = earthTexture,

                Spin = new Vector3(0.6f, 0, 0),
                Orbit = new Vector3(0.6f, 0, 0),

                Origin = new Vector3(12.0f, 0, 0),
            };

            Drawn.Add(earth);
            Updated.Add(earth);

            planet = new Planet(this)
            {
                Scale = new Vector3(0.2f),
                Texture = moon,
                Orbit = new Vector3(2.0f, 0, 0),

                Origin = new Vector3(10.0f, 0, 0),
            };

            earth.Satellites.Add(planet);

            // Mars
            planet = new Planet(this)
            {
                Scale = new Vector3(0.4f),
                Texture = mars,

                Spin = new Vector3(0.30f, 0, 0),
                Orbit = new Vector3(0.30f, 0, 0),

                Origin = new Vector3(28.0f, 0, 0),
            };

            Drawn.Add(planet);
            Updated.Add(planet);

            // Stars
            planet = new Planet(this)
            {
                Scale = new Vector3(-100.0f, 100, 100),
                Texture = stars,

                Spin = new Vector3(0.01f, 0.02f, 0.03f),
            };

            Drawn.Add(planet);
            Updated.Add(planet);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            AsteroidManager.Update(gameTime);

            foreach (Planet planet in Updated)
                planet.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            foreach (ANode drawn in Drawn)
                drawn.Draw(null);

            AsteroidManager.Draw(null);

            base.Draw(gameTime);
        }
    }
}
