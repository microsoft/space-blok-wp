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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using BlokGameObjects.ScreenManagement;

namespace Blok
{
    /// <summary>
    /// Represents the pause screen in-game-menu.
    /// </summary>
    class PauseScreen : GameScreen
    {
        private Texture2D bgTex;
        private List<Button> buttons = new List<Button>();

        private Vector2 screenPos;

        #region Initialization

        public override void LoadContent() 
        {
            bgTex = screenManager.Game.Content.Load<Texture2D>(@"Images\gui\menu_bg");

            int screenWidth = screenManager.GraphicsDevice.DisplayMode.Width;
            int screenHeight = screenManager.GraphicsDevice.DisplayMode.Height;

            screenPos = new Vector2((screenHeight - bgTex.Width) * 0.5f, (screenWidth - bgTex.Height) * 0.5f);

            Texture2D btnDefTex = screenManager.Game.Content.Load<Texture2D>(@"Images\gui\button_def");
            Texture2D btnPressedTex = screenManager.Game.Content.Load<Texture2D>(@"Images\gui\button_pressed");
            SpriteFont font = screenManager.Game.Content.Load<SpriteFont>(@"Fonts\MenuFont");

            Button exitToMainMenuBtn = new Button(new Vector2(screenPos.X + 45, screenPos.Y + 25), new Vector2(200, 60), "Main menu", font);
            buttons.Add(exitToMainMenuBtn);
            exitToMainMenuBtn.buttonPressedEvent += new Button.ButtonPressed(ExitButtonPressed);

            Button resumeGameBtn = new Button(new Vector2(screenPos.X + 45, screenPos.Y + 85), new Vector2(200, 60), "Resume", font);
            buttons.Add(resumeGameBtn);
            resumeGameBtn.buttonPressedEvent += new Button.ButtonPressed(ResumeGameButtonPressed);
        }

        private void ResumeGameButtonPressed()
        {
            screenManager.ExitScreen();
        }

        private void ExitButtonPressed()
        {
            // Exit from this screen
            screenManager.ExitScreen();
            // Exit from gameplay screen
            screenManager.ExitScreen();
            // Add main menu screen
            screenManager.AddScreen(new MainMenuScreen());
        }

        public override void UnloadContent() 
        { 
        }

        #endregion

        #region Update & rendering

        public override void HandleInput(InputState input) 
        {
            foreach (Button btn in buttons)
            {
                if (btn.HandleInput(input) == true)
                {
                    return;
                }
            }

            if (input.GamepadState.Buttons.Back == ButtonState.Pressed)
            {
                // Exit from this screen
                screenManager.ExitScreen();
            }
        }

        public override void Update(GameTime gameTime) 
        { 
        }

        public override void Draw(GameTime gameTime) 
        {
            screenManager.SpriteBatch.Begin();
            screenManager.SpriteBatch.Draw(bgTex, screenPos, Color.White);

            foreach (Button btn in buttons)
            {
                btn.Draw(screenManager.SpriteBatch);
            }

            screenManager.SpriteBatch.End();
        }

        #endregion
    }
}
