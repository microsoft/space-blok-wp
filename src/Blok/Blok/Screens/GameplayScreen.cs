/**
* Copyright (c) 2011 Digia Plc
* Copyright (c) 2011-2014 Microsoft Mobile and/or its subsidiary(-ies).
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
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.Constraints.SolverGroups;
using BEPUphysics.Entities;
using BEPUphysics.Settings;
using BEPUphysics.CollisionShapes;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Constraints.SingleEntity;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.CollisionTests;
using BEPUphysics.EntityStateManagement;

using Blok.Particles;
using System.Xml;

using BlokGameObjects.GameLevelObjects;
using BlokGameObjects;
using BlokGameObjects.ScreenManagement;
using BlokGameObjects.GamePlatforms;
using BlokGameObjects.GameLevelObjects.ScoresParticles;

namespace Blok
{
    /// <summary>
    /// Represents the game play screen, holding the "game" objects.
    /// </summary>
    class GameplayScreen : GameScreen
    {
        private Vector3[] PLATFORM_POSITIONS = 
        {
            new Vector3(10.6f, 4.0f, 6.0f), 
            new Vector3(-10.6f, 4.0f, 6.0f), 
            new Vector3(-10.6f, 4.0f, -6.0f), 
            new Vector3(10.6f, 4.0f, -6.0f)
        };

        private float[] PLATFORM_ORIENTATIONS = 
        {
            225.0f, 135.0f, 45.0f, 315.0f
        };

        private Vector2[] PLATFORM_SWIPE_CENTERS = 
        {
            new Vector2(770.0f, 450.0f), 
            new Vector2(30.0f, 450.0f),         
            new Vector2(30.0f, 30.0f), 
            new Vector2(770.0f, 30.0f)
        };

        private Vector3[] PLATFORM_BALL_COLORS = 
        {
            new Vector3(1.0f, 0.25f, 0.25f),
            new Vector3(0.25f, 1.0f, 0.25f),
            new Vector3(0.25f, 0.25f, 1.0f),
            new Vector3(1.0f, 0.5f, 0.0f)
        };

        Camera camera;

        Space space;

        SpriteFont font;

        List<Platform> platforms = new List<Platform>();

        CollisionManager collisionManager;

        Texture2D bgTex;
        Texture2D dimTex;

        GameLevelEx level;

        DepthStencilState depthStensilState = new DepthStencilState() { DepthBufferEnable = true };

        #region Initialization

        // Constructor
        public GameplayScreen()
        {
        }

        private void GameEndCheck()
        {
            if (level.Blocks.Count == 0 && level.isVisibleParticles() == false)
            {
                WinningScreen winningScreen = new WinningScreen(platforms);
                screenManager.AddScreen(winningScreen);
            }
        }

        private void CreatePlatforms()
        {
            for (int n = 0; n < PLATFORM_POSITIONS.Count(); ++n)
            {
                Platform platform = new Platform(PLATFORM_POSITIONS[n], PLATFORM_ORIENTATIONS[n],
                    PLATFORM_SWIPE_CENTERS[n], PLATFORM_BALL_COLORS[n], screenManager.Game, camera);

                platform.Initialize();
                platform.scoreAddedEvent += new Platform.ScoreAdded(GameEndCheck);
                platforms.Add(platform);
            }
        }

        public Model model;
        public DrawableComponent c;

        public override void LoadContent()
        {
            // Create physics space and add some gravity to it
            space = new Space();

            // Add space instance to game services
            screenManager.Game.Services.AddService(typeof(ISpace), space);

            // Set up space
            space.ForceUpdater.Gravity = new Vector3(0.0f, -9.81f, 0.0f);
            space.DeactivationManager.VelocityLowerLimit = 0.01f;

            // Init view and projection matrices
            camera = new Camera(screenManager.GraphicsDevice.Viewport);
            camera.Initialize();
            // Place camera to appropriate position
            camera.View = Matrix.CreateLookAt(new Vector3(0.0f, 30.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), Vector3.Forward);

            screenManager.Game.Services.AddService(typeof(Camera), camera);
            
            // Load font
            font = ScreenManager.Game.Content.Load<SpriteFont>(@"Fonts\InfoScreenFont");

            // Load background image
            bgTex = screenManager.Game.Content.Load<Texture2D>(@"Images\bk");

            // Load dimming image
            dimTex = screenManager.Game.Content.Load<Texture2D>(@"Images\dim");

            CreatePlatforms();

            level = new GameLevelEx(screenManager.Game);
            level.LoadLevel(@"Game\Levels\testlvl");

            collisionManager = new CollisionManager(platforms, level);

            model = screenManager.Game.Content.Load<Model>(@"Models\block02");
            c = new DrawableComponent(model, camera, screenManager.Game);
        }

        public override void UnloadContent()
        {
            screenManager.Game.Services.RemoveService(typeof(ISpace));
            screenManager.Game.Services.RemoveService(typeof(Camera));
        }

        #endregion

        #region Update & rendering

        public override void HandleInput(InputState input)
        {
            if (input.GamepadState.Buttons.Back == ButtonState.Pressed)
            {
                screenManager.AddScreen(new PauseScreen());
                //screenManager.AddScreen(new WinningScreen(platforms));
            }

            // Let platforms handle input
            foreach (Platform platform in platforms)
            {
                platform.HandleInput(input);
            }
        }

        private TimeSpan elapsedTime;
        private float frameCounter;
        private float frameRate;

        public override void Update(GameTime gameTime)
        {
            if (screenManager.TopmostScreen != this)
            {
                return;
            }

            // Update physics
            space.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            // Update platforms
            foreach (Platform platform in platforms)
            {
                platform.Update(gameTime);
            }

            // Measure framerate.
            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            elapsedTime += gameTime.ElapsedGameTime;

            level.Update(gameTime);

            base.Update(gameTime);
        }

        private StringBuilder stringBuilder = new StringBuilder();

        public override void Draw(GameTime gameTime)
        {
            screenManager.SpriteBatch.Begin();
            screenManager.SpriteBatch.Draw(bgTex, new Rectangle(0, 0, screenManager.GraphicsDevice.PresentationParameters.BackBufferWidth, screenManager.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            screenManager.SpriteBatch.End();
            
            // Each time setting up depth buffering since sprite batch operations always mess up graphics device state
            ScreenManager.GraphicsDevice.DepthStencilState = depthStensilState;

            foreach (Platform platform in platforms)
            {
                platform.Draw(gameTime);
            }

            level.Draw(gameTime);

            screenManager.SpriteBatch.Begin();
            // Render text
            foreach (Platform platform in platforms)
            {
                stringBuilder.Remove(0, stringBuilder.Length);
                stringBuilder.Append(platform.Score);
                Vector2 scoreStringSize = font.MeasureString(stringBuilder);

                scoreStringSize *= 0.5f;
                scoreStringSize.Y += 60.0f;

                screenManager.SpriteBatch.DrawString(font, stringBuilder, platform.SwipeCenterPos, Color.White,
                    MathHelper.ToRadians(180 - platform.OrientationAngle), scoreStringSize, platform.ScoreScale, SpriteEffects.None, 0.0f);
            }

            /*
            // Render FPS value
            stringBuilder.Remove(0, stringBuilder.Length);
            stringBuilder.AppendFormat("FPS: {0}", frameRate);
            screenManager.SpriteBatch.DrawString(font, stringBuilder, new Vector2(100.0f, 10.0f), Color.White);
            
            // Measure framerate.
            frameCounter++;
            */

            if (screenManager.TopmostScreen != this)
            {
                screenManager.SpriteBatch.Draw(dimTex, new Vector2(0, 0), Color.White);
            }

            screenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
