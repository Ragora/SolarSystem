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

        RenderTarget2D SceneRender;

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
        public FlyingCamera Camera;

        Texture2D ReticleTexture;

        public List<Projectile> Projectiles;

        Planet Stars;

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

            Projectiles = new List<Projectile>();

            InputManager.Create(this);
            InputManager.UseKeyboard = true;

            SoundManager.Create(this);

            Camera = new FlyingCamera(this, new Vector3(0, 10, -20), Vector3.Zero, new Vector3(0, 1, 0));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            AsteroidManager.Create(this, 600, 200, 300, 0, 50);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D earthTexture = Content.Load<Texture2D>("textures/earthmap");
            Texture2D moon = Content.Load<Texture2D>("textures/meteor1");
            Texture2D sun = Content.Load<Texture2D>("textures/sun");
            Texture2D mercury = Content.Load<Texture2D>("textures/mercury");
            Texture2D venus = Content.Load<Texture2D>("textures/venus");
            Texture2D mars = Content.Load<Texture2D>("textures/mars");
            Texture2D stars = Content.Load<Texture2D>("textures/stars");

            ReticleTexture = Content.Load<Texture2D>("textures/reticle");
            SceneRender = new RenderTarget2D(GraphicsDevice, Window.ClientBounds.Width, Window.ClientBounds.Height);

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

                Origin = new Vector3(13.0f, 0, 0),
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

                Origin = new Vector3(20.0f, 0, 0),
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

                Origin = new Vector3(40.0f, 0, 0),
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

                Origin = new Vector3(100.0f, 0, 0),
            };

            Drawn.Add(planet);
            Updated.Add(planet);

            // Stars
            Stars = new Planet(this)
            {
                Scale = new Vector3(-100.0f, 100, 100),
                Texture = stars,

                Spin = new Vector3(0.01f, 0.02f, 0.03f),
            };

            Updated.Add(Stars);
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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            AsteroidManager.Update(gameTime);

            foreach (Planet planet in Updated)
                planet.Update(gameTime);

            foreach (Projectile projectile in Projectiles)
                projectile.Update(gameTime);


            InputManager.Update(gameTime);
            SoundManager.Update();

            Camera.Update(gameTime);
            Stars.Position = Camera.Position;
            Stars.UpdateTransformation();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            #region Draw 3D
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            GraphicsDevice.SetRenderTarget(SceneRender);
            GraphicsDevice.Clear(Color.Black);

            BasicEffect effect = new BasicEffect(GraphicsDevice);
            effect.View = Camera.View;
            effect.Projection = Camera.Projection;
            effect.World = Matrix.Identity;

            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            Stars.Draw(effect);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            AsteroidManager.Draw(effect);

            foreach (ANode drawn in Drawn)
                drawn.Draw(effect);

            foreach (Projectile projectile in Projectiles)
                projectile.Draw(effect);
            #endregion

            #region Combined Buffers
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteBatch.Begin();
            spriteBatch.Draw(SceneRender, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height), Color.White);

            spriteBatch.Draw(ReticleTexture, new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2), null, Color.White, 0,
                new Vector2(ReticleTexture.Width / 2, ReticleTexture.Height / 2), 1, SpriteEffects.None, 0);
            spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }
    }
}
