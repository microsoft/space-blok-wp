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

namespace BlokGameObjects.ScreenManagement
{
    /// <summary>
    /// ScreenManagermanages game screens by encapsulating the stack of screens 
    /// and utilities for managing this stack.
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields & properties
        
        private List<GameScreen> screens = new List<GameScreen>();
        private bool initialized = false;

        private InputState inputState = new InputState();

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public SpriteBatch SpriteBatch { get { return spriteBatch; } }

        public GameScreen TopmostScreen { get { return screens.Last(); } }
        
        #endregion

        #region Initialization

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">The game object</param>
        public ScreenManager(Game game) : base(game)
        {
            graphics = new GraphicsDeviceManager(game);
            graphics.IsFullScreen = true;
            graphics.PreferMultiSampling = false;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;

            game.IsMouseVisible = true;
        }

        /// <summary>
        /// From DrawableGameComponent. Initializes screen manager.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            // Create a new SpriteBatch, which is used to render game screens.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            initialized = true;
        }

        /// <summary>
        /// From DrawableGameComponent. Loads content to screen manager and to existing screens
        /// </summary>
        protected override void LoadContent()
        {
            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
            }
        }

        /// <summary>
        /// From DrawableGameComponent. Unloads content from screen manager and from existing screens
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (GameScreen screen in screens)
            {
                screen.UnloadContent();
            }            
        }

        #endregion

        #region Updating & Drawing

        /// <summary>
        /// From DrawableGameComponent. Updates the state of Screen manager, 
        /// updates game screens and passes user input to topmost one.
        /// </summary>
        /// <param name="gameTime">Game time</param>
        public override void Update(GameTime gameTime)
        { 
            // Grab input state and pass it to topmost window for handling
            inputState.Update();
            if (screens.Count() > 0)
            {
                TopmostScreen.HandleInput(inputState);
            }

            // Update all game screens, using "for" instead of "foreach"
            // allows more screens to be inserted via addScreen during the loop
            // in the Update methods. Calling ExitScreen during inside the Update
            // will crash the app.
            for (int i = 0; i < screens.Count; i++)
            {
                screens[i].Update(gameTime);
            }
        }

        /// <summary>
        /// From DrawableGameComponent. Renders game screens from the bottom to
        /// the top in the screen stack.
        /// </summary>
        /// <param name="gameTime">Game time</param>
        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen screen in screens)
            {
                screen.Draw(gameTime);
            }
        }

        #endregion

        #region Screens management

        /// <summary>
        /// Pushes the screen into the top of screen stack.
        /// This screen will be rendered on top of all other screens and will receive user input.
        /// </summary>
        /// <param name="screen">The screen to be pushed to the stack</param>
        public void AddScreen(GameScreen screen)
        {
            foreach (GameScreen screensInList in screens)
            {
                if (screen.GetType() == screensInList.GetType())
                {
                    System.Diagnostics.Debug.WriteLine("Screen is already in list");
                    return;
                }
            }

            screens.Add(screen);
            screen.ScreenManager = this;
            if (initialized)
            {
                screen.LoadContent();
            }
        }

        /// <summary>
        /// Removes topmost screen from the screen stack.
        /// </summary>
        public void ExitScreen()
        {
            if (screens.Count() > 0)
            {
                screens.Last().UnloadContent();
                screens.Remove(screens.Last());
            }
        }

        #endregion 
    }
}
