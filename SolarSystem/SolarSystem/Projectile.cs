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
    public class Projectile : Model
    {
        public float Speed;
        public Vector3 Direction;

        public float CurrentTime;
        public float DespawnTime;

        public Projectile(Game1 game) : base(game, "Models/sphere")
        {
            Texture = game.Content.Load<Texture2D>("Textures/earthmap");
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            float deltaSeconds = (float)time.ElapsedGameTime.Milliseconds / 1000;
            CurrentTime += deltaSeconds;

            Position += (Direction * Speed) * deltaSeconds;
            Rotation += new Vector3(12.0f / 60.0f, 0, 26.0f / 60.0f);
            UpdateTransformation();
        }
    }
}
