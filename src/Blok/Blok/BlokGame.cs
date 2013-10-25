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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Constraints.SolverGroups;
using System.Threading;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.CollisionTests;
using BEPUphysics.Collidables.MobileCollidables;
using BlokGameObjects.ScreenManagement;

namespace Blok
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BlokGame : Microsoft.Xna.Framework.Game
    {
        private ScreenManager screenManager;
 
        public BlokGame()
        {
            this.IsMouseVisible = true;
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            // Show main menu.
            screenManager.AddScreen(new MainMenuScreen());
        }
    }
}
