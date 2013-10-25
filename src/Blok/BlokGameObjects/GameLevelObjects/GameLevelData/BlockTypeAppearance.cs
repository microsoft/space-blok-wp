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
using Microsoft.Xna.Framework.Content;

namespace BlokGameObjects.GameLevelObjects.GameLevelData
{
    /// <summary>
    /// Defines how blocks of particular block type should look like.
    /// Also encapsulates graphical data for all blocks of this type 
    /// </summary>
    public class BlockTypeAppearance
    {
        public String Model { get; set; }
        public String Texture { get; set; }
        public Color Color { get; set; }
    }
}
