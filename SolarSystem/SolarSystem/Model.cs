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
    /// A model class represents a drawable model in the game world whose geometry and other data
    /// is sourced from a .x or a .fbx file.
    /// </summary>
    public class Model : ANode
    {
        /// <summary>
        /// The model to be drawn.
        /// </summary>
        private Microsoft.Xna.Framework.Graphics.Model Rendered;

        /// <summary>
        /// A constructor accepting the game and fileName as input. These parameters are an
        /// absolute necessicity when instantiating a model at the very least, therefore they
        /// are not initializer list parameters.
        /// </summary>
        /// <param name="game">The game instantiating this model.</param>
        /// <param name="shapeName">The model file to use as geometry source.</param>
        public Model(Microsoft.Xna.Framework.Game game, string shapeName) : base(game)
        {
            Rendered = game.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(shapeName);
        }

        /// <summary>
        /// An empty update method to be overwritten on child classes.
        /// </summary>
        /// <param name="time">
        /// A snapshot of the current timing values.
        /// </param>
        public virtual void Update(GameTime time)
        {

        }

        /// <summary>
        /// Draws the model to the game world.
        /// </summary>
        /// <param name="effect">
        /// The basic effect to draw with. This parameter is ignored for models, its used by other
        /// drawn objects though.
        /// </param>
        public override void Draw(BasicEffect effect)
        {
            Matrix[] transforms = new Matrix[Rendered.Bones.Count];
            Rendered.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in Rendered.Meshes)
            {
                foreach (BasicEffect meshEffect in mesh.Effects)
                {
                    meshEffect.EnableDefaultLighting();
                    meshEffect.Texture = Texture;

                    meshEffect.View = Game.Camera.View;
                    meshEffect.Projection = Game.Camera.Projection;
                    meshEffect.World = transforms[mesh.ParentBone.Index] * Transformation;
                }
                mesh.Draw();
            }
        }
    }
}
