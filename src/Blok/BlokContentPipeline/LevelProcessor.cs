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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

using BlokContentData.GameLevelObjects.GameLevelData;

// TODO: replace these with the processor input and output types.
//using TInput = System.String;
//using TOutput = System.String;

namespace BlokContentPipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "Blok LevelProcessor")]
    public class LevelProcessor : ContentProcessor<NodeContent, GameLevelContent>
    {
        private const string BLOCK_NAME_PREFIX = "Block_";

        public override GameLevelContent Process(NodeContent input, ContentProcessorContext context)
        {
            // Create game level data object
            GameLevelContent levelContent = new GameLevelContent();
            levelContent.BlockGroups = new List<BlockGroupData>();

            // Go through NodeContent tree and get block data
            ParseNodes(input, levelContent);

            return levelContent;
        }

        private void ParseNodes(NodeContent nodeContent, GameLevelContent levelContent)
        { 

            // Check current item
            if (nodeContent.Name.StartsWith(BLOCK_NAME_PREFIX) == true)
            { 
                // TODO: using regex for this
                // Get type name
                string typeName = nodeContent.Name.Substring(BLOCK_NAME_PREFIX.Length, nodeContent.Name.Length - BLOCK_NAME_PREFIX.Length - 4);
                
                BlockGroupData blockGroup = null;

                // Check if block group for this type exists
                foreach (BlockGroupData bg in levelContent.BlockGroups)
                {
                    if (typeName.Equals(bg.BlockTypeName) == true)
                    {
                        blockGroup = bg;
                        break;
                    }
                }

                // Create new block group if it is not exists yet
                if (blockGroup == null)
                {
                    blockGroup = new BlockGroupData();
                    blockGroup.BlockTypeName = typeName;
                    blockGroup.Blocks = new List<BlockData>();

                    levelContent.BlockGroups.Add(blockGroup);
                }

                // Create block itself and add it to the group
                BlockData block = new BlockData();
                block.Transform = nodeContent.Transform;

                blockGroup.Blocks.Add(block);
            }

            // Go through this node's children
            foreach (NodeContent child in nodeContent.Children)
            {
                ParseNodes(child, levelContent);
            }
        }
    }
}