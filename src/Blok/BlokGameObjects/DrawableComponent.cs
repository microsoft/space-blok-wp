/**
* Copyright (c) 2011 Digia Plc
* Copyright (c) 2011-2014 Microsoft Mobile and/or its subsidiary(-ies).
* All rights reserved.
*
* For the applicable distribution terms see the license text file included in
* the distribution.
*/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Collections.Generic;
using BEPUphysics.Entities;

namespace BlokGameObjects
{
    /// <summary>
    /// Makes it easier to store and draw 3D objects, by adding model member variable to
    /// DrawableGameComponent. Holds transforms, texture, transparency etc. to save the 
    /// state of the drawable component. Knows how to draw the component with the 
    /// set transform, texure, etc.
    /// Used as base class for 3D objects such as Ball and Platform.
    /// </summary>
    public class DrawableComponent : DrawableGameComponent
    {
        #region Fields

        protected Model model;
        protected Vector3 diffuseColor = new Vector3(1.0f, 1.0f, 1.0f);

        protected Matrix localTransform = Matrix.Identity;
        protected Matrix worldTransform = Matrix.Identity;

        public Vector3 DiffuseColor { get { return diffuseColor; } set { diffuseColor = value; } }

        public Texture2D Texture { get; set; }
        public float Transparency { get; set; }

        public Matrix LocalTransform { get { return localTransform; } set { localTransform = value; } }
        public Matrix WorldTransform { get { return worldTransform; } set { worldTransform = value; } }

        public Camera Camera { get; set; }

        public bool LightingEnable { get; set; }
        public bool FogEnable { get; set; }

        #endregion

        #region Initializations

        public DrawableComponent(Model model, Camera camera, Game game) : base(game)
        {
            this.model = model;
            this.Camera = camera;

            Transparency = 1.0f;
            LightingEnable = false;
            FogEnable = false;
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

                    if (LightingEnable == true)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = false;
                    }

                    effect.DiffuseColor = diffuseColor;
                    effect.Alpha = Transparency;

                    if (Texture != null)
                    {
                        effect.Texture = Texture;
                        effect.TextureEnabled = true;
                    }

                    if (FogEnable == true)
                    {
                        effect.FogColor = new Color(11, 26, 38).ToVector3();
                        effect.FogStart = 25.0f;
                        effect.FogEnd = 45.0f;
                        effect.FogEnabled = true;
                    }
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }

        #endregion
    }
}
