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

using BlokGameObjects.GameLevelObjects.BlockActions;

namespace BlokGameObjects.GameLevelObjects.BlockEvents
{
    /// <summary>
    /// Base class for Blok events.
    /// </summary>
    public abstract class BlockEvent
    {
        public IBlockAction BlockAction { get; set; }

        public abstract void HandleEvent(Block block);
    }
}
