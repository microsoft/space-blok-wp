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
using BlokGameObjects;
using BlokGameObjects.ScreenManagement;
using BlokGameObjects.GamePlatforms;


namespace Blok
{
    /// <summary>
    /// Represents the winning screen to show the scores of each player.
    /// </summary>
    class WinningScreen : GameScreen
    {
        private Texture2D bgTex;
        private Texture2D resultsTex;
        private List<KeyValuePair<float, Vector3>> scores = new List<KeyValuePair<float, Vector3>>();
        private SpriteFont font;

        private Model model;
        private DrawableComponent ballDrawable;
        private Camera camera;

        private StringBuilder stringBuilder = new StringBuilder();

        private static int CompareScores(KeyValuePair<float, Vector3> a, KeyValuePair<float, Vector3> b)
        {
            if (a.Key < b.Key)
            {
                return 1;
            }
            else if (a.Key == b.Key)
            {
                return 0;
            }

            return -1;
        }

        public WinningScreen(List<Platform> platforms)
        {
            foreach (Platform platform in platforms)
            {
                scores.Add(new KeyValuePair<float, Vector3>(platform.Score, platform.BallColor));
            }

            scores.Sort(CompareScores);
        }

        #region Initialization

        public override void LoadContent()
        {
            // Load logo
            bgTex = screenManager.Game.Content.Load<Texture2D>(@"Images\gui\winning_screen_bg");
            resultsTex = screenManager.Game.Content.Load<Texture2D>(@"Images\gui\text_results");
            font = screenManager.Game.Content.Load<SpriteFont>(@"Fonts\MenuFont");
            model = screenManager.Game.Content.Load<Model>(@"Models\sphere");

            // Create camera
            camera = new Camera(screenManager.GraphicsDevice.Viewport);
            camera.Initialize();
            camera.View = Matrix.CreateTranslation(new Vector3(0.0f, 0.0f, -5.0f));

            // Create drawable component to display the model
            ballDrawable = new DrawableComponent(model, camera, screenManager.Game);
            ballDrawable.LightingEnable = true;
            ballDrawable.DiffuseColor = new Vector3(0.0f, 0.7f, 0.7f);
        }

        public override void UnloadContent()
        {
        }

        #endregion

        #region Update & rendering

        public override void HandleInput(InputState input)
        {
            if (input.GamepadState.Buttons.Back == ButtonState.Pressed)
            {
                // Exit from WinningScreen
                screenManager.ExitScreen();

                // Exit from GameplayScreen
                screenManager.ExitScreen();

                // Show the MainMenuScreen
                screenManager.AddScreen(new MainMenuScreen());
            }
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            screenManager.SpriteBatch.Begin();

            screenManager.SpriteBatch.Draw(bgTex, new Vector2(0, 0), Color.White);
            screenManager.SpriteBatch.Draw(resultsTex, new Vector2(220, 40), Color.White);
            
            Vector2 scorePos = new Vector2(330, 40 + resultsTex.Height + 20);

            int i=1;
            foreach (KeyValuePair<float, Vector3> score in scores)
            {
                stringBuilder.Remove(0, stringBuilder.Length);
                stringBuilder.AppendFormat("{0}", score.Key);
                screenManager.SpriteBatch.DrawString(font, stringBuilder, scorePos, Color.White, 0, new Vector2(0, 0), 1.6f, SpriteEffects.None, 0.0f);
                scorePos.Y += font.MeasureString(stringBuilder).Y + 5;
                i++;
            }
            screenManager.SpriteBatch.End();

            i = 0;
            foreach (KeyValuePair<float, Vector3> score in scores)
            {
                ballDrawable.DiffuseColor = score.Value;

                Matrix matrix = Matrix.CreateScale(0.1f);
                matrix *= Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalSeconds);
                matrix *= Matrix.CreateTranslation(-0.61f, i * -0.33f + 0.27f, 0.0f);
                ballDrawable.LocalTransform = matrix;

                ballDrawable.Draw(gameTime);
                i++;
            }
         }

        #endregion
    }
}
