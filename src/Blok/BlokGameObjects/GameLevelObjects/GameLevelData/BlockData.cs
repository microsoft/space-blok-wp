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
using Microsoft.Xna.Framework.Content;

namespace BlokContentData.GameLevelObjects.GameLevelData
{
    /// <summary>
    /// Holds the transform and scale information.
    /// </summary>
    public class BlockData
    {
        public Matrix Transform { get; set; }

        [ContentSerializerIgnore]
        public Vector3 Scale { get; set; }

        public BlockData()
        { 
        }

        public BlockData(Matrix transform)
        {
            Transform = transform;
        }
    }
}
