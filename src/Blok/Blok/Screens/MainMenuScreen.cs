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
using Microsoft.Xna.Framework.Input;
using BlokGameObjects.ScreenManagement;
using BlokGameObjects;

using BlokGameObjects.Instancing;

namespace Blok
{
    /// <summary>
    /// Represents the main menu screen of the application.
    /// </summary>
    class MainMenuScreen : GameScreen
    {
        private Texture2D logoTex;
        private Texture2D bgTex;
        private Texture2D menuTex;

        private List<Button> buttons = new List<Button>();

        private Model model;
        DrawableComponent cubeDrawable;
        private Camera camera;

        public MainMenuScreen()
        {
        }

        #region Initialization

        public override void LoadContent() 
        {
            // Load background
            bgTex = screenManager.Game.Content.Load<Texture2D>(@"Images\gui\main_menu_bg");
            
            // Load menu graphics
            menuTex = screenManager.Game.Content.Load<Texture2D>(@"Images\gui\menu_bg");

            // Load logo
            logoTex = screenManager.Game.Content.Load<Texture2D>(@"Images\gui\blok_title");

            // Load font
            SpriteFont font = screenManager.Game.Content.Load<SpriteFont>(@"Fonts\MenuFont");

            // Create New game button
            Texture2D btnDefTex = screenManager.Game.Content.Load<Texture2D>(@"Images\gui\button_def");
            Texture2D btnPressedTex = screenManager.Game.Content.Load<Texture2D>(@"Images\gui\button_pressed");

            Button newGameBtn = new Button(new Vector2(90, 215), new Vector2(200, 60), "New game", font);
            buttons.Add(newGameBtn);
            newGameBtn.buttonPressedEvent += new Button.ButtonPressed(NewGameButtonPressed);

            // Create info button
            Button infoBtn = new Button(new Vector2(90, 275), new Vector2(200, 60), "Info", font);
            buttons.Add(infoBtn);
            infoBtn.buttonPressedEvent += new Button.ButtonPressed(InfoButtonPressed);

            // Create camera
            camera = new Camera(screenManager.GraphicsDevice.Viewport);
            camera.Initialize();
            camera.View = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, -5.0f));

            // Load some simple model to show on the right side of the screen
            model = screenManager.Game.Content.Load<Model>(@"Models\block01");

            Texture2D blockTexture = screenManager.Game.Content.Load<Texture2D>(@"Images\SimpleBlock");

            // Create drawable component to display the model
            cubeDrawable = new DrawableComponent(model, camera, screenManager.Game);
            cubeDrawable.LocalTransform *= Matrix.CreateRotationZ(MathHelper.ToRadians(45.0f));
            cubeDrawable.LocalTransform *= Matrix.CreateRotationX(MathHelper.ToRadians(45.0f));
            cubeDrawable.LightingEnable = true;
            cubeDrawable.Texture = blockTexture;
            //cubeDrawable.FogEnable = true;
            cubeDrawable.DiffuseColor = new Vector3(0.0f, 0.7f, 0.7f);
        }

        private void NewGameButtonPressed()
        {
            screenManager.ExitScreen();
            screenManager.AddScreen(new GameplayScreen());
        }

        private void InfoButtonPressed()
        {
            screenManager.ExitScreen();
            screenManager.AddScreen(new InfoScreen());
        }

        public override void UnloadContent() 
        { 
        }

        #endregion

        #region Update & rendering

        public override void HandleInput(InputState input)
        {
            foreach (Button button in buttons)
            {
                if (button.HandleInput(input) == true)
                {
                    return;
                }
            }

            if (input.GamepadState.Buttons.Back == ButtonState.Pressed)
            {
                screenManager.Game.Exit();
            }
        }


        public override void Update(GameTime gameTime) 
        {
        }


        public override void Draw(GameTime gameTime) 
        {
            screenManager.SpriteBatch.Begin();

            screenManager.SpriteBatch.Draw(bgTex, new Vector2(0, 0), Color.White);

            cubeDrawable.WorldTransform = Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalSeconds);
            cubeDrawable.WorldTransform *= Matrix.CreateTranslation(new Vector3(1.0f, 0.0f, 0.0f));

            screenManager.SpriteBatch.Draw(logoTex, new Vector2(80, 15), Color.White);
            screenManager.SpriteBatch.Draw(menuTex, new Vector2(40, 190), Color.White);
            
            foreach (Button button in buttons)
            {
                button.Draw(screenManager.SpriteBatch);
            }

            screenManager.SpriteBatch.End();

            cubeDrawable.Draw(gameTime);
        }

        #endregion
    }
}
