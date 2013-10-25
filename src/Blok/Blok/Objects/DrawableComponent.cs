using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using BEPUphysics.Entities;
using BlokGameObjects;

namespace Blok
{
    public class DrawableComponent : DrawableGameComponent
    {
        #region Fields

        protected Model model;
        protected Vector3 diffuseColor = new Vector3(1.0f, 1.0f, 1.0f);

        protected Matrix localTransform = Matrix.Identity;
        protected Matrix worldTransform = Matrix.Identity;

        //protected Camera camera;

        public Vector3 DiffuseColor { get { return diffuseColor; } set { diffuseColor = value; } }

        public Matrix LocalTransform { get { return localTransform; } set { localTransform = value; } }
        public Matrix WorldTransform { get { return worldTransform; } set { worldTransform = value; } }

        public Camera Camera { get; set; }//{ get { return camera; } set { camera = value; } }

        #endregion

        #region Initializations

        public DrawableComponent(Model model, Camera camera, Game game) : base(game)
        {
            this.model = model;
            this.Camera = camera;
        }

        // Temporary constructor for compatibility with inherited classes. Should not be used
        public DrawableComponent(Game game)
            : base(game)
        {
        }

        #endregion

        #region Loading

        /// <summary>
        /// Load the component content
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        #endregion

        #region Update

        /// <summary>
        /// Update the component
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #endregion

        #region Render

        /// <summary>
        /// Draw the component
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Draw(GameTime gameTime)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = localTransform * worldTransform;
                    effect.View = Camera.View;
                    effect.Projection = Camera.Projection;
                    
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.DiffuseColor = diffuseColor;
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }

        #endregion
    }
}
