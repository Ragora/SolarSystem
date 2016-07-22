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
    /// A planet is essentially a model with a hardcoded sphere reference and will spin about its axiis as
    /// specified by Spin on its own.
    /// </summary>
    public class Planet : Model
    {
        /// <summary>
        /// A list of planets that orbit this given planet, and are thus dubbed satellites.
        /// </summary>
        public List<Planet> Satellites;

        /// <summary>
        /// A vector representing the spin axiis and strength.
        /// </summary>
        public Vector3 Spin;

        /// <summary>
        /// A vector representing the overall orbit of this planet, relative to its orbit planet or the center
        /// of the universe.
        /// </summary>
        public Vector3 Orbit;

        /// <summary>
        /// A constructor accepting an instantiating game.
        /// </summary>
        /// <param name="game">The game that is instantiating this planet.</param>
        public Planet(Game1 game, string model = null) : base(game, model != null ? model : "Models/sphere")
        {
            Satellites = new List<Planet>();
        }

        /// <summary>
        /// Draws the planet to the game world.
        /// </summary>
        /// <param name="effect">
        /// The effect to draw using.
        /// </param>
        public override void Draw(BasicEffect effect)
        {
            base.Draw(effect);

            foreach (Planet satellite in Satellites)
                satellite.Draw(effect);
        }

        /// <summary>
        /// Update method to update this planet's rotation.
        /// </summary>
        /// <param name="time">
        /// A snapshot of the current game time.
        /// </param>
        public override void Update(GameTime time)
        {
            float elapsedSeconds = (float)time.ElapsedGameTime.Milliseconds / 1000.0f;

            // Update the main rotation and relative rotation.
            Rotation += Orbit * elapsedSeconds;
            RelativeRotation += Spin * elapsedSeconds;

            foreach (Planet satellite in Satellites)
            {
                satellite.Position = Transformation.Translation;

                satellite.Update(time);
            }

            UpdateTransformation();
        }
    }
}
