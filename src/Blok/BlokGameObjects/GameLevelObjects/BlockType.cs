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
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using BlokGameObjects.GameLevelObjects.BlockEvents;
using Microsoft.Xna.Framework;

namespace BlokGameObjects.GameLevelObjects
{
    /// <summary>
    /// Represents block type
    /// </summary>
    public class BlockType
    {
        /// <summary>
        /// The name of the type
        /// </summary>
        public String Name { get; private set; }
        /// <summary>
        /// Hit points of the block of this type
        /// </summary>
        public int HitPoints { get; set; }
        /// <summary>
        /// How many scores player will get per 1 hp when hitting block of this type
        /// </summary>
        public int ScoresPerHitPoint { get; set; }
        /// <summary>
        /// Block events
        /// </summary>
        public List<BlockEvent> Events { get; set; }

        public Model Model;
        public Texture2D Texture;
        public Vector3 Color;

        /// <summary>
        /// Set owner of this block type
        /// </summary>
        public GameLevel Owner { get; private set; }

        public BlockType(String name, GameLevel owner)
        {
            Name = name;
            Owner = owner;
        }
    }
}
