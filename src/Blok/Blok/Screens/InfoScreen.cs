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
using Microsoft.Xna.Framework.Input.Touch;
using BlokGameObjects;
using BlokGameObjects.ScreenManagement;

namespace Blok
{
    /// <summary>
    /// Represents the info screen holding the info text how to play the game,
    /// credits, and license information of BEPU physics library.
    /// </summary>
    class InfoScreen : GameScreen
    {
        private Texture2D bgTex;

        private String infoText =
            "Space Blok is a 1-4 player game where the goal is to\n" +
            "gather as much points as possible by destroying the game\n" +
            "bloks.\n\n" +
            "This Nokia Developer example is hosted at \n" +
            "http://projects.developer.nokia.com/blok/\n\n" +
            "To start a new game, select New game from the main menu.\n" +
            "Game board is shown with four platforms on each corner\n" +
            "of the view.\n\n" +
            "Place the phone on the table so that each player are on\n" +
            "each corner of the phone beside the colored ball. The\n" +
            "ball can be shot by swiping from the top of the ball\n" +
            "towards game structure in the center of the view. The\n" +
            "direction and the speed of the swipe affects to the\n" +
            "ball trajectory, use fast swipes to hit the nearest\n" +
            "game bloks, and use slow swipes to hit the furthest\n" +
            "bloks. The game level will begin to rotate when the\n" +
            "bloks are hit. Some bloks requires several hits in\n" +
            "order them to be destroyed.\n\n" +
            "Good game strategy is to shoot the ball inside the game\n" +
            "level from one of its endings and let the ball bounce\n" +
            "inside the structure giving loads of points.\n\n" +
            "Credits\n\n" +
            "Original development:\n\n" +
            "Denis Kudinkin\n" +
            "Jani Rönkkönen\n" +
            "Juha Pynnönen\n\n" +
            "Further development:\n\n" +
            "Antti Krats\n" +
            "Kari Kantola\n\n\n" +
            "BEPU physics license:\n\n" +
            "Licensed under the Apache License, Version 2.0 (the\n" +
            "\"License\"); you may not use this file except in compliance\n" +
            "with the License.\n" +
            "You may obtain a copy of the License at\n\n" +
            "    http://www.apache.org/licenses/LICENSE-2.0\n\n" +
            "Unless required by applicable law or agreed to in writing,\n" +
            "software distributed under the License is distributed on an\n" +
            "\"AS IS\" BASIS, WITHOUT WARRANTIES OR CONDITIONS\n" +
            "OF ANY KIND, either express or implied. See the License\n" +
            "for the specific language governing permissions and\n" +
            "limitations under the License.";


        private SpriteFont font;
        private float textHeight;
        private bool dragging = false;

        float minY = -800;
        float maxY = 800;

        private Vector2 textPos;
        private float lastY = 0;
        private float speed = 0;

        private TouchLocation touchLocation;

        #region Initialization

        public InfoScreen()
        {
        }

        public override void LoadContent()
        {
            // Load background
            bgTex = screenManager.Game.Content.Load<Texture2D>(@"Images\gui\info_view_menu_bg");
            
            // Load font
            font = screenManager.Game.Content.Load<SpriteFont>(@"Fonts\InfoScreenFont");
            textHeight = font.MeasureString(infoText).Y;
            textPos = new Vector2(60, 0);

            // The app is on LandscapeLeft, so we use Width instead of Height.
            minY = -textHeight + 440;
            maxY = 0;
        }

        public override void UnloadContent()
        {
        }

        #endregion

        #region Update & rendering

        public override void HandleInput(InputState input)
        {
            if (input.TouchState.Count() > 0)
            {
                touchLocation = input.TouchState[0];
                if (touchLocation.State == TouchLocationState.Pressed)
                {
                    dragging = true;
                    lastY = touchLocation.Position.Y;
                }
                else if (touchLocation.State == TouchLocationState.Released)
                {
                    dragging = false;
                }
            }

            if (dragging)
            {
                speed = lastY - touchLocation.Position.Y;
                lastY = touchLocation.Position.Y;
            }
            else
            {
                if (textPos.Y < minY) 
                {
                    if (speed > 0)
                    {
                        speed -= (minY - textPos.Y) * 0.02f;
                    }
                    else
                    {
                        speed = (textPos.Y - minY) * 0.1f;
                    }
                }
	            else if (maxY < textPos.Y)
                {
                    if (speed < 0)
                    {
                        speed -= (maxY - textPos.Y) * 0.02f;
                    }
                    else
                    {
                        speed = (textPos.Y - maxY) * 0.1f;
                    }
                }
            }

            speed *= 0.95f;
            textPos.Y -= speed;
            

            if (input.GamepadState.Buttons.Back == ButtonState.Pressed)
            {
                // Exit from AboutScreen
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

            screenManager.SpriteBatch.End();
            
            // Implement clipping for the infoText by using the smaller viewport.
            Viewport oldView = screenManager.GraphicsDevice.Viewport;

            Viewport newView = new Viewport();
            newView.X = 0;
            //newView.Y = 160;
            newView.Y = 20;
            newView.Width = 800;
            newView.Height = 440;//300;
            newView.MinDepth = 0f;
            newView.MaxDepth = 1f;
            screenManager.GraphicsDevice.Viewport = newView;
            
            screenManager.SpriteBatch.Begin();
            screenManager.SpriteBatch.DrawString(font, infoText, textPos, Color.White);
            screenManager.SpriteBatch.End();

            // Return the original viewport.
            screenManager.GraphicsDevice.Viewport = oldView;
        }

        #endregion
    }
}
