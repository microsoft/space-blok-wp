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

namespace BlokContentData.GameLevelObjects.GameLevelData
{
    /// <summary>
    /// Holds the list of different blok types.
    /// </summary>
    public class GameLevelContent
    {
        public List<BlockGroupData> BlockGroups { get; set; }

        public int BlocksCount()
        { 
            int blocksCount = 0;

            foreach (BlockGroupData group in BlockGroups)
            {
                blocksCount += group.Blocks.Count;
            }

            return blocksCount;
        }
    }
}
