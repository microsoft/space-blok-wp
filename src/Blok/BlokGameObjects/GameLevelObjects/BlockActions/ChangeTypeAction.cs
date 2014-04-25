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

namespace BlokGameObjects.GameLevelObjects.BlockActions
{
    /// <summary>
    /// Changes the type of blok (color) when it is hit.
    /// </summary>
    public class ChangeTypeAction : IBlockAction
    {
        Random randomGenerator = new Random();

        public String NewTypeName
        {
            get;
            set;
        }

        public void DoAction(Block block)
        {
            GameLevel level = block.BlockType.Owner;

            if ("RandomBasic".Equals(NewTypeName))
            {
                int number = randomGenerator.Next(0, 3);
                switch (number)
                {
                    case 0:
                        level.ChangleBlockType(block, "YellowBasic");
                        break;
                    case 1:
                        level.ChangleBlockType(block, "GreenBasic");
                        break;
                    case 2:
                        level.ChangleBlockType(block, "BlueBasic");
                        break;
                }
            }
            else
            {
                level.ChangleBlockType(block, NewTypeName);
            }
        }
    }
}
