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
    /// ANode is an abstract class representing a drawable node in the 3D game space.
    /// </summary>
    public abstract class ANode
    {
        /// <summary>
        /// The graphics device in use by the node.
        /// </summary>
        protected GraphicsDevice GraphicsDevice;

        /// <summary>
        /// The texture to be drawn on the node.
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// The current transformation of the node.
        /// </summary>
        public Matrix Transformation;

        /// <summary>
        /// The game that has instantiated this node.
        /// </summary>
        protected Game1 Game;

        /// <summary>
        /// An abstract method to draw the node on screen.
        /// </summary>
        /// <param name="effect">
        /// The effect to draw with. Some objects may ignore this.
        /// </param>
        public abstract void Draw(BasicEffect effect);

        /// <summary>
        /// A constructor accepting the instantiating game.
        /// </summary>
        /// <param name="game">The game that is instantiating this node.</param>
        public ANode(Microsoft.Xna.Framework.Game game)
        {
            this.Game = (Game1)game;
            this.GraphicsDevice = game.GraphicsDevice;

            this.Scale = new Vector3(1, 1, 1);
        }

        /// <summary>
        /// The internally stored position.
        /// </summary>
        private Vector3 InternalPosition;

        /// <summary>
        /// A position property with set-get modifiers to properly update the node transformation.
        /// </summary>
        public Vector3 Position
        {
            set
            {
                InternalPosition = value;
                this.UpdateTransformation();
            }
            get
            {
                return InternalPosition;
            }
        }

        /// <summary>
        /// The internally stored rotation.
        /// </summary>
        private Vector3 InternalRotation;

        /// <summary>
        /// A rotation property with set-get modifiers to properly update the node transformation.
        /// </summary>
        public Vector3 Rotation
        {
            set
            {
                InternalRotation = value;
                this.UpdateTransformation();
            }
            get
            {
                return InternalRotation;
            }
        }

        /// <summary>
        /// The internally stored scale.
        /// </summary>
        private Vector3 InternalScale;

        /// <summary>
        /// A scale property with set-get modifiers to properly update the node transformation.
        /// </summary>
        public Vector3 Scale
        {
            set
            {
                InternalScale = value;
                this.UpdateTransformation();
            }
            get
            {
                return InternalPosition;
            }
        }

        /// <summary>
        /// The internally stored origin.
        /// </summary>
        private Vector3 InternalOrigin;

        /// <summary>
        /// A property with set-get modifiers to update the node transformation.
        /// </summary>
        public Vector3 Origin
        {
            set
            {
                InternalOrigin = value;
                this.UpdateTransformation();
            }
            get
            {
                return InternalOrigin;
            }
        }

        /// <summary>
        /// The internally stored relative rotation.
        /// </summary>
        private Vector3 InternalRelativeRotation;

        /// <summary>
        /// A relative rotation property with set-get modifiers to update the node transformation.
        /// </summary>
        public Vector3 RelativeRotation
        {
            set
            {
                InternalRelativeRotation = value;
                this.UpdateTransformation();
            }
            get
            {
                return InternalRelativeRotation;
            }
        }

        /// <summary>
        /// Method that is called internally to update the node transformation matrix.
        /// </summary>
        private void UpdateTransformation()
        {
            Transformation = Matrix.CreateFromYawPitchRoll(this.InternalRelativeRotation.X, this.InternalRelativeRotation.Y, this.InternalRelativeRotation.Z);
            Transformation *= Matrix.CreateTranslation(this.InternalOrigin);
            Transformation *= Matrix.CreateFromYawPitchRoll(this.InternalRotation.X, this.InternalRotation.Y, this.InternalRotation.Z);
            Transformation *= Matrix.CreateScale(this.InternalScale);
            Transformation *= Matrix.CreateTranslation(this.InternalPosition);
        }
    }
}
