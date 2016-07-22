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
            Asteroids = new List<Planet>();

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

                Planet asteroid = new Planet(game, "Models/asteroid" + RandomHelper.RandomInt(1, 2))
                {
                    Texture = texture,
                    Scale = new Vector3(0.00008f),

                    Origin = new Vector3((float)Math.Cos(angle) * distanceX, height, (float)Math.Sin(angle) * distanceZ),
                    Spin = new Vector3(RandomHelper.RandomFloat(0, 2.0f * 3.14f),
                    RandomHelper.RandomFloat(0, 2.0f * 3.14f), RandomHelper.RandomFloat(0, 2.0f * 3.14f)),
                    Orbit = new Vector3(1.0f, 0, 0),
                };

                Console.WriteLine(asteroid.Origin);

                Asteroids.Add(asteroid);
            }
        }

        public static void Draw(BasicEffect effect)
        {
            foreach (Planet asteroid in Asteroids)
                asteroid.Draw(effect);
        }

        public static void Update(GameTime time)
        {
            foreach (Planet asteroid in Asteroids)
                asteroid.Update(time);
        }
    }
}
