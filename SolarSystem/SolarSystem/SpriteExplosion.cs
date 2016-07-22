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
    public class SpriteExplosion : Model
    {
        /// <summary>
        /// The animated sprite we used to animate our explosions.
        /// </summary>
        public AnimatedSprite SpriteFlipper;

        public SpriteExplosion(Game1 game) : base(game, "Models/Cube10uR")
        {
            Texture = game.Content.Load<Texture2D>("Textures/pluto");

            SpriteFlipper = new AnimatedSprite(game, "Textures/explosion", new Point(107, 104), new Point(5, 3));
            SpriteFlipper.Initialize();
            SpriteFlipper.MillisecondsPerFrame = 100;
            SpriteFlipper.Updated = true;
            SpriteFlipper.Drawn = true;
            SpriteFlipper.Repeat = false;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            SpriteFlipper.Update(time);
            Texture = SpriteFlipper.CurrentFrameTexture;

            Vector3 forward = Position - Game.Camera.Position;
            forward.Normalize();

            Transformation = Matrix.CreateScale(Scale);
            Transformation *= Matrix.CreateWorld(Position, forward, Vector3.Up);
        }
    }
}
