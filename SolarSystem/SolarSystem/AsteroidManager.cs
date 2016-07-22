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
    /// A singleton class that manages the occurrences of asteroids in the game world.
    /// </summary>
    public class AsteroidManager
    {
        /// <summary>
        /// Private constructor to prevent construction.
        /// </summary>
        private AsteroidManager() { }

        /// <summary>
        /// A list of planets representing asteroids.
        /// </summary>
        private static List<Planet> Asteroids;

        private static List<Planet> RogueAsteroids;

        private static List<Debris> DebrisList;

        private static List<SpriteExplosion> Explosions;

        private static float RogueAsteroidSpeed = 10.0f;

        static int MaxRogueAsteroids = 10;
        static float RogueAsteroidProbability = 0.15f;

        private static Game1 Game;

        /// <summary>
        /// Create the asteroid manager using the following parameters.
        /// </summary>
        /// <param name="game">
        /// The instantiating game.
        /// </param>
        /// <param name="number">
        /// The total number of asteroids to spawn.
        /// </param>
        /// <param name="minDist">
        /// The minimum distance from the center of the universe for our generation.
        /// </param>
        /// <param name="maxDist">
        /// The maximum distance from the center of the universe for our generation.
        /// </param>
        /// <param name="minHeight">
        /// The minimum possible height to use for our asteroids.
        /// </param>
        /// <param name="maxHeight">
        /// The maximum possible height to use for our asteroids.
        /// </param>
        public static void Create(Game1 game, int number, float minDist, float maxDist, float minHeight, float maxHeight)
        {
            Game = game;

            Asteroids = new List<Planet>();
            RogueAsteroids = new List<Planet>();
            DebrisList = new List<Debris>();
            Explosions = new List<SpriteExplosion>();

            Texture2D texture = game.Content.Load<Texture2D>("Textures/meteor1");

            for (int iteration = 0; iteration < number; iteration++)
            {
                // First, determine a random angle between 0 and 2pi and a random height
                float height = RandomHelper.RandomFloat(minHeight, maxHeight);

                float distanceX = RandomHelper.RandomFloat(minDist, maxDist);
                float distanceZ = RandomHelper.RandomFloat(minDist, maxDist);

                // Determine a base angle from iteration then add a variance
                float variance = RandomHelper.RandomFloat(0, 100) / 100.0f; 
                float angle = (float)(iteration % 20);
                angle += variance;

                Planet asteroid = new Planet(game, "Models/sphere")
                {
                    Texture = texture,

                    Origin = new Vector3((float)Math.Cos(angle) * distanceX, height, (float)Math.Sin(angle) * distanceZ),
                    Spin = new Vector3(RandomHelper.RandomFloat(0, 2.0f * 3.14f),
                    RandomHelper.RandomFloat(0, 2.0f * 3.14f), RandomHelper.RandomFloat(0, 2.0f * 3.14f)),
                    Orbit = new Vector3(1.0f, 0, 0),
                    Scale = new Vector3(0.8f),
                };

                Asteroids.Add(asteroid);
            }
        }

        public static void Draw(BasicEffect effect)
        {

            Game.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            foreach (SpriteExplosion explosion in Explosions)
                explosion.Draw(effect);
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (Planet asteroid in Asteroids)
                asteroid.Draw(effect);

            foreach (Planet rogueAsteroid in RogueAsteroids)
                rogueAsteroid.Draw(effect);

            foreach (Debris debris in DebrisList)
                debris.Draw(effect);
        }

        public static void Update(GameTime time)
        {
            List<SpriteExplosion> temporaryExplosions = new List<SpriteExplosion>(Explosions);
            foreach (SpriteExplosion explosion in temporaryExplosions)
            {
                if (explosion.SpriteFlipper.Updated)
                    explosion.Update(time);
                else
                    Explosions.Remove(explosion);
            }

            foreach (Planet asteroid in Asteroids)
                asteroid.Update(time);

            List<Debris> temporaryDebris = new List<Debris>(DebrisList);
            foreach (Debris debris in temporaryDebris)
            {
                debris.Update(time);

                if (debris.CurrentTime >= debris.DespawnTime)
                    DebrisList.Remove(debris);
            }


            float deltaSeconds = (float)time.ElapsedGameTime.Milliseconds / 1000;

            // Every update we pick a percent

            if (Asteroids.Count != 0)
            {
                for (int iteration = 0; iteration < MaxRogueAsteroids; iteration++)
                {
                    if (Asteroids.Count == 0 || RogueAsteroids.Count >= MaxRogueAsteroids)
                        break;

                    float random = (float)RandomHelper.RandomInt(0, 100) / 100;
                    if (random <= RogueAsteroidProbability)
                    {
                        Planet rogueAsteroid = Asteroids[RandomHelper.RandomInt(0, Asteroids.Count - 1)];
                        Asteroids.Remove(rogueAsteroid);
                        RogueAsteroids.Add(rogueAsteroid);

                        rogueAsteroid.Position = rogueAsteroid.Transformation.Translation;
                        rogueAsteroid.Origin = Vector3.Zero;
                        rogueAsteroid.Spin = Vector3.Zero;
                        rogueAsteroid.Orbit = Vector3.Zero;
                    }
                }
            }
       
            List<Planet> temporaryAsteroids = new List<Planet>(RogueAsteroids);
            foreach (Planet asteroid in temporaryAsteroids)
            {
                // Move each one towards center
                Vector3 delta = Vector3.Zero - asteroid.Position;
                delta.Normalize();

                asteroid.Position += (delta * RogueAsteroidSpeed) * deltaSeconds;
                asteroid.UpdateTransformation();

                // What is our asteroid's distance?
                float distance = Vector3.Distance(Vector3.Zero, asteroid.Position);

                if (distance <= 2)
                    RogueAsteroids.Remove(asteroid);
            }

            // Check collisions with projectiles
            temporaryAsteroids = new List<Planet>(RogueAsteroids);
            List<Projectile> temporaryProjectiles = new List<Projectile>(Game.Projectiles);

            // Dead projectiles?
            // Is the projectile dead?
            foreach (Projectile projectile in temporaryProjectiles)
            {
                if (projectile.CurrentTime >= projectile.DespawnTime)
                {
                    Game.Projectiles.Remove(projectile);

                    SpriteExplosion explosion = new SpriteExplosion(Game)
                    {
                        Position = projectile.Position,
                        Scale = new Vector3(0.1f),
                    };

                    Explosions.Add(explosion);
                }
            }

            foreach (Planet asteroid in temporaryAsteroids)
            {
                foreach (Projectile projectile in temporaryProjectiles)
                {
                    if (Vector3.Distance(projectile.Position, asteroid.Position) < 2)
                    {
                        RogueAsteroids.Remove(asteroid);
                        Game.Projectiles.Remove(projectile);

                        // 0.2f
                        int debrisCount = RandomHelper.RandomInt(4, 6);

                        for (int iteration = 0; iteration < debrisCount; iteration++)
                        {
                            Vector3 randomDirection = new Vector3(RandomHelper.RandomFloat(0, 100), RandomHelper.RandomFloat(0, 100), RandomHelper.RandomFloat(0, 100));
                            randomDirection.Normalize();

                            Debris newDebris = new Debris(Game)
                            {
                                Position = asteroid.Position,
                                DespawnTime = 10,
                                Direction = randomDirection,
                                Scale = new Vector3(0.2f),
                                Speed = RandomHelper.RandomFloat(8, 11),
                            };

                            SpriteExplosion explosion = new SpriteExplosion(Game)
                            {
                                Position = asteroid.Position,
                                Scale = new Vector3(0.1f),
                            };

                            Explosions.Add(explosion);
                            DebrisList.Add(newDebris);
                        }
                    }
                }
            }
        }
    }
}
