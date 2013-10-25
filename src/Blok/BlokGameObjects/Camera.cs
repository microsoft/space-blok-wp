/**
* Copyright (c) 2011 Digia Plc
* Copyright (c) 2011 Nokia Corporation and/or its subsidiary(-ies).
* All rights reserved.
*
* For the applicable distribution terms see the license text file included in
* the distribution.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlokGameObjects
{
    /// <summary>
    /// Camera class. Encapsulates view and projection transforms.
    /// </summary>
    public class Camera : IGameComponent
    {
        private Matrix view;
        private Matrix projection;

        private Viewport viewport;

        public Matrix View { get; set; }
        public Matrix Projection { get{ return projection; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="viewport">viewport to obtain information needed for constructing projection transform</param>
        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
        }

        /// <summary>
        /// From IGameComponent. Creates default view and projection transforms
        /// </summary>
        public void Initialize()
        {
            // Create default view transform
            view = Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up);
            // Create projection transform
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(30), viewport.AspectRatio, 1.0f, 100.0f);
        }
    }
}
