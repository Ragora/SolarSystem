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
    public class Debris : Projectile
    {
        public float DespawnTime;

        public float CurrentTime;

        public Debris(Game1 game) : base(game)
        {
            Texture = game.Content.Load<Texture2D>("Textures/meteor1");
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            float deltaSeconds = (float)time.ElapsedGameTime.Milliseconds / 1000;
            CurrentTime += deltaSeconds;
        }
    }
}
