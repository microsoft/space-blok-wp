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

using BlokGameObjects.GameLevelObjects.BlockEvents;

namespace BlokGameObjects.GameLevelObjects.GameLevelData
{
    /// <summary>
    /// Defines the hit points, score per hit, appearance of the blok type.
    /// </summary>
    public class BlockTypeData
    {
        /// <summary>
        /// Hit points of the block of this type
        /// </summary>
        public int HitPoints { get; set; }
        /// <summary>
        /// How many scores player will get per 1 hp when hitting block of this type
        /// </summary>
        public int ScoresPerHitPoint { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public BlockTypeAppearance Appearance { get; set; }
        /// <summary>
        /// Block events
        /// </summary>
        public List<BlockEvent> Events { get; set; }

    }
}
