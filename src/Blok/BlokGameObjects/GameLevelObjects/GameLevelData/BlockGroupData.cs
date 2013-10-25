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

namespace BlokContentData.GameLevelObjects.GameLevelData
{
    /// <summary>
    /// Used to store information of blok type name and list of BlokData.
    /// </summary>
    public class BlockGroupData
    {
        public String BlockTypeName { get; set; }
        public List<BlockData> Blocks { get; set; }
    }
}
