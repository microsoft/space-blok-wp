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

namespace BlokContentData.GameLevelObjects.Animations
{
    interface Animation
    {
        /// <summary>
        /// Updates animation.
        /// </summary>
        /// <param name="gameTime">game time</param>
        /// <returns>true if animation finished and could be disposed, false - otherwise</returns>
        bool Update(GameTime gameTime);
        /// <summary>
        /// Draws animation
        /// </summary>
        /// <param name="gameTime"></param>
        void Draw(GameTime gameTime);
    }
}
