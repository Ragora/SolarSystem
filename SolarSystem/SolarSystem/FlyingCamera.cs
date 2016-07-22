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
    public class FlyingCamera : StaticCamera
    {
        private Point ScreenCenter;

        public float YawSpeed = 0.5f;
        public float PitchSpeed = 0.5f;

        public float MoveSpeed = 10;

        public bool MovingForward;
        public bool MovingBackward;
        public bool MovingLeft;
        public bool MovingRight;

        public FlyingCamera(Game1 game, Vector3 position, Vector3 target, Vector3 up) : base(game, position, target, up)
        {
            ScreenCenter = new Point(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);

            InputManager.SetKeyResponder(Keys.W, MoveForwardResponder);
            InputManager.SetKeyResponder(Keys.S, MoveBackwardResponder);
            InputManager.SetKeyResponder(Keys.A, MoveLeftResponder);
            InputManager.SetKeyResponder(Keys.D, MoveRightResponder);

            InputManager.SetKeyResponder(Keys.Space, FireResponder);
        }

        private void MoveForwardResponder(bool pressed)
        {
            MovingForward = pressed;
        }

        private void MoveBackwardResponder(bool pressed)
        {
            MovingBackward = pressed;
        }

        private void MoveLeftResponder(bool pressed)
        {
            MovingLeft = pressed;
        }

        private void MoveRightResponder(bool pressed)
        {
            MovingRight = pressed;
        }

        private void FireResponder(bool pressed)
        {
            if (pressed)
            {
                Projectile newProjectile = new Projectile(Game)
                {
                    Position = this.Position,
                    Direction = this.Direction,
                    DespawnTime = 4,
                    Speed = 15,
                };

                Game.Projectiles.Add(newProjectile);
            }
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            float deltaSeconds = (float)time.ElapsedGameTime.Milliseconds / 1000;

            // What is our mouse delta from center?
            MouseState mouseState = Mouse.GetState();
            Point mouseDelta = new Point(mouseState.X - ScreenCenter.X, mouseState.Y - ScreenCenter.Y);

            Console.WriteLine(Direction.Y);

            SetYaw(deltaSeconds * -mouseDelta.X * YawSpeed);

            if (Direction.Y < 0.97f && mouseDelta.Y < 0)
                SetPitch(deltaSeconds * mouseDelta.Y * PitchSpeed);
            else if (Direction.Y > -0.97f && mouseDelta.Y > 0)
                SetPitch(deltaSeconds * mouseDelta.Y * PitchSpeed);

            Console.WriteLine(Direction.Y);
            // Do our directions
            if (MovingForward)
                Position += Direction * MoveSpeed * deltaSeconds;
            else if (MovingBackward)
                Position -= Direction * MoveSpeed * deltaSeconds;

            Vector3 leftVector = Vector3.Cross(Vector3.Up, Direction);
            if (MovingRight)
                Position -= leftVector * MoveSpeed * deltaSeconds;
            else if (MovingLeft)
                Position += leftVector * MoveSpeed * deltaSeconds;

            UpdateTransformation();

            Position = Position.Y > 20 ? new Vector3(Position.X, 20, Position.Z) : Position;
            Position = Position.Y < -20 ? new Vector3(Position.X, -20, Position.Z) : Position;

            // Slap the mouse back at centre
            Mouse.SetPosition(ScreenCenter.X, ScreenCenter.Y);
        }
    }
}
