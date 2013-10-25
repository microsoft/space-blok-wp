/**
* Copyright (c) 2011 Digia Plc
* Copyright (c) 2011 Nokia Corporation and/or its subsidiary(-ies).
* All rights reserved.
*
* For the applicable distribution terms see the license text file included in
* the distribution.
*/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using BEPUphysics.Entities;
using BlokGameObjects;
using BlokGameObjects.GameComponents;


namespace BlokGameObjects.GamePlatforms
{
    /// <summary>
    /// Represents the visible ball object, holds also the collision object.
    /// </summary>
    public class Ball : GameObject
    {
        #region Fields/Properties

        public bool Active { get; set; }

        public DrawableComponent Drawable { get; private set; }
        public Entity Entity { get; private set; }

        public Platform Platform { get; private set; }

        public int HitPoints { get; set; }

        #endregion

        #region Initializtions

        public Ball(DrawableComponent drawable, Entity entity, Platform platform) 
        {
            Active = false;

            Drawable = drawable;
            Entity = entity;
            Platform = platform;

            HitPoints = 1;
        }

        #endregion Initializtions

        public override void Update(GameTime gameTime)
        {
            Drawable.WorldTransform = Entity.WorldTransform;
        }
    }
}
