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
using Microsoft.Xna.Framework.Input.Touch;
using BlokGameObjects.ScreenManagement;

namespace Blok
{
    /// <summary>
    /// Class for simple button, handles presss input and triggers event on button press.
    /// </summary>
    class Button
    {
        private Rectangle boundingRect;
        private String text;
        private SpriteFont font;

        private bool pressed = false;

        public Button(Vector2 position, Vector2 size, String text, SpriteFont font)
        {
            this.text = text;
            this.font = font;
            
            boundingRect = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        public bool HandleInput(InputState input)
        {
            bool result = false;

            foreach (TouchLocation touchLocation in input.TouchState)
            {
                if (touchLocation.State == TouchLocationState.Pressed)
                {
                    if (boundingRect.Contains((int)touchLocation.Position.X, (int)touchLocation.Position.Y))
                    {
                        pressed = true;
                        result = true;
                    }
                }
                else if (touchLocation.State == TouchLocationState.Released)
                {
                    if (boundingRect.Contains((int)touchLocation.Position.X, (int)touchLocation.Position.Y))
                    {
                        pressed = false;
                        result = true;

                        if (buttonPressedEvent != null)
                        {
                            buttonPressedEvent();
                        }
                    }
                    else
                    {
                        pressed = false;
                        result = false;
                    }
                }
            }

            return result;
        }

        public Rectangle GetBoundingRect() 
        {
            return boundingRect;
        }

        public delegate void ButtonPressed();
        public event ButtonPressed buttonPressedEvent;

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 pos = new Vector2(boundingRect.X + boundingRect.Width / 2 - (font.MeasureString(text).X / 2),
                                      boundingRect.Y + boundingRect.Height / 2 - (font.MeasureString(text).Y / 2));

            if (pressed == false)
            {
                spriteBatch.DrawString(font, text, pos, Color.White);
            }
            else 
            {
                spriteBatch.DrawString(font, text, pos, Color.LightBlue);
            }
        }
    }
}
