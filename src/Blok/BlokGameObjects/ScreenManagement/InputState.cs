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
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace BlokGameObjects.ScreenManagement
{
    /// <summary>
    /// InputState class contains updated state of touch panel and gamepad
    /// </summary>
    public class InputState
    {
        #region
        
        private TouchCollection touchState;
        private GamePadState gamepadState;

        public TouchCollection TouchState { get { return touchState; } }
        public GamePadState GamepadState { get { return gamepadState; } }
        
        #endregion

        #region Updating

        /// <summary>
        /// Updates state of touch panel and gamepad
        /// </summary>
        public void Update()
        {
            touchState = TouchPanel.GetState();
            gamepadState = GamePad.GetState(PlayerIndex.One);
        }

        #endregion
    }
}
