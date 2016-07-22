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
    /// A static camera is a fixed camera in the game space represented by a point, its field of view, near plane, far plane,
    /// and facing direction.
    /// </summary>
    public class StaticCamera
    {
        /// <summary>
        /// A matrix representing the position, look position and up vector.
        /// </summary>
        public Matrix View;

        /// <summary>
        /// A matrix representing the field of view, aspect ration, near plane and far plane.
        /// </summary>
        public Matrix Projection; // FoV, aspectRatio, nearPlane, farPlane

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="position">Position in game world.</param>
        /// <param name="target">Position to be looking up.</param>
        /// <param name="up">Orientation.</param>
        public StaticCamera(Game1 game, Vector3 position, Vector3 target, Vector3 up)
        {
            View = Matrix.CreateLookAt(position, target, up);

            Projection = Matrix.CreatePerspectiveFieldOfView((float)game.Window.ClientBounds.Width / (float)game.Window.ClientBounds.Height,
                MathHelper.PiOver2, 
                1, 
                1000);
        }
    }
}
