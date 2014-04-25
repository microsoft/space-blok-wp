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
using BEPUphysics.Collidables.MobileCollidables;
using Microsoft.Xna.Framework;
using BlokGameObjects;
using BlokGameObjects.Instancing;

namespace BlokGameObjects.GameLevelObjects
{
    /// <summary>
    /// Defines a single blok. Holds the visual representation and the collision
    /// object as well. Holds also the hit points of the blok.
    /// </summary>
    public class Block
    {
        public BlockType BlockType { get; private set; }
        
        public InstancedMesh<VertexData>.Instance Instance { get; private set; }

        public CompoundChild Entity { get; private set; }

        public int HitPoints { get; set; }

        public Vector3 Scale { get; set; }

        public Block(BlockType blockType, InstancedMesh<VertexData>.Instance instance, CompoundChild entity)
        {
            BlockType = blockType;
            Instance = instance;
            Entity = entity;

            HitPoints = blockType.HitPoints;
        }

        public void ChangeBlockType(BlockType blockType, InstancedMesh<VertexData>.Instance instance)
        {
            BlockType = blockType;
            Instance = instance;
            HitPoints = blockType.HitPoints;
        }
    }
}
