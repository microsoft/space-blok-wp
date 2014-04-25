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
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Constraints.SingleEntity;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.CollisionShapes;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.CollisionRuleManagement;

using BEPUphysics.MathExtensions;
using BlokGameObjects.Instancing;
using Microsoft.Xna.Framework.Content;

using BlokContentData.GameLevelObjects.GameLevelData;
using BlokGameObjects.GameLevelObjects.GameLevelData;
using BEPUphysics.DataStructures;
using BlokGameObjects;


namespace BlokGameObjects.GameLevelObjects
{
    /// <summary>
    /// Represents game level consist of blocks of different types
    /// </summary>
    public class GameLevel : DrawableGameComponent
    {
        #region Fields & properties
        // Mass of overall compound body.
        private const float COMPOUND_BODY_MASS = 100.0f;
        // Angular damping of overall compound body.
        private const float COMPOUND_BODY_ANGULAR_DAMPING = 0.01f;

        // Physics space where compound body of blocks will be added.
        private ISpace space;

        // Compoud body for the blocks.
        private CompoundBody compoundBody;
        // Constraint which prevents compound body from moving.
        // TODO: investigate more proper way to prevent moving.
        private MaximumLinearSpeedConstraint constraint;

        // Collisiong group for removed blocks. Blocks in this group will not be interacting with the balls.
        private CollisionGroup removedBlocksGroup;

        // Camera for the rendering.
        private Camera camera;

        // Dictionary of block types.
        private Dictionary<String, BlockType> blockTypes = new Dictionary<string, BlockType>();
        // List of blocks.
        private List<Block> blocks = new List<Block>();

        private Dictionary<String, InstancedMesh<VertexData>> blockInstancedMeshes = new Dictionary<String, InstancedMesh<VertexData>>();

        /// <summary>
        /// Collision group for removed groups.
        /// </summary>
        public CollisionGroup RemovedBlocksGroup { get { return removedBlocksGroup; } }

        // Why should this be public?
        public Entity Body { get { return compoundBody; } }
        // This also. Hmm..
        public List<Block> Blocks { get { return blocks; } }

        public Camera Camera { get { return camera; } }

        #endregion

        #region Initializing

        public GameLevel(Game game)
            : base(game)
        {
            space = (ISpace)game.Services.GetService(typeof(ISpace));
            camera = (Camera)game.Services.GetService(typeof(Camera));
        }

        #endregion

        #region Blocks management

        private BlockType LoadBlockType(String blockTypeName, int maxBlocks)
        {
            // Load block type data
            BlockTypeData blockTypeData = Game.Content.Load<BlockTypeData>(@"Game\BlockTypes\" + blockTypeName);

            // Create block type instance
            BlockType blockType = new BlockType(blockTypeName, this);

            // Populate type instance with data
            blockType.HitPoints = blockTypeData.HitPoints;
            blockType.ScoresPerHitPoint = blockTypeData.ScoresPerHitPoint;
            blockType.Events = blockTypeData.Events;

            // Load model for this block type
            Model model = Game.Content.Load<Model>(@"Models\" + blockTypeData.Appearance.Model);
            blockType.Model = model;
            blockType.Color = blockTypeData.Appearance.Color.ToVector3();

            // Create instance mesh for blocks of this type
            // TODO: Think about how mane instances this InstancedMesh should have
            InstancedMesh<VertexData> instancedMesh = new InstancedMesh<VertexData>(Game.GraphicsDevice, model.Meshes[0].MeshParts[0], Matrix.Identity, maxBlocks);

            if (String.IsNullOrEmpty(blockTypeData.Appearance.Texture) == false)
            {
                Texture2D texture = Game.Content.Load<Texture2D>(@"Images\" + blockTypeData.Appearance.Texture);
                instancedMesh.Effect.Texture = texture;
                instancedMesh.Effect.TextureEnabled = true;
                blockType.Texture = texture;
            }

            // Set color for instance mesh effect
            instancedMesh.Effect.DiffuseColor = blockTypeData.Appearance.Color.ToVector3();

            // Associate instance mesh with block type
            blockInstancedMeshes.Add(blockType.Name, instancedMesh);

            // Store block type by its name
            blockTypes.Add(blockTypeName, blockType);

            return blockType;
        }

        public void LoadLevel(String fileName)
        { 
            // Load level data
            GameLevelContent levelData = Game.Content.Load<GameLevelContent>(fileName);
            int blocksCount = levelData.BlocksCount();

            // List of blocks chapes for compound body
            List<CompoundShapeEntry> shapes = new List<CompoundShapeEntry>();

            // Create block groups and block physics
            foreach (BlockGroupData blockGroup in levelData.BlockGroups)
            {
                // Create block group
                LoadBlockType(blockGroup.BlockTypeName, blocksCount);

                // Create physical representation of blocks in this group
                Vector3 scale;
                Quaternion rotation;
                Vector3 translation;
                foreach (BlockData blockData in blockGroup.Blocks)
                {
                    // Extract size, position and orientation values from block data transform
                    blockData.Transform.Decompose(out scale, out rotation, out translation);
                    blockData.Scale = scale;

                    // Create physical shape of the block to be part of compound body of blocks
                    BoxShape blockShape = new BoxShape(scale.X, scale.Y, scale.Z);

                    // Create compound shape entry for compund body of blocks
                    CompoundShapeEntry entry = new CompoundShapeEntry(blockShape, new RigidTransform(translation, rotation));
                    shapes.Add(entry);
                }
            }

            // Create compound body
            compoundBody = new CompoundBody(shapes, COMPOUND_BODY_MASS);

            compoundBody.PositionUpdateMode = BEPUphysics.PositionUpdating.PositionUpdateMode.Continuous;
            compoundBody.AngularDamping = COMPOUND_BODY_ANGULAR_DAMPING;

            // Compound body has Position and LocalPosition (in Collision information)
            // Position property is position of mass center in global space - it is calculated automatically.
            // LocalPosition property is position of geometry of the body in its local space.
            // So in order to create compound body which is rotated around desired position ((0,0,0) for now)
            // We should switch Position and LocalPosition properties of our compound body.
            compoundBody.CollisionInformation.LocalPosition = compoundBody.Position;
            compoundBody.Position = Vector3.Zero;

            // Add constraint prevents compound body from moving
            constraint = new MaximumLinearSpeedConstraint(compoundBody, 0.0f);

            // Create collision group for removed blocks
            removedBlocksGroup = new CollisionGroup();

            // Create blocks
            int childCollidableIndex = 0;
            foreach (BlockGroupData blockGroup in levelData.BlockGroups)
            {
                Matrix localPosTransform = Matrix.CreateTranslation(compoundBody.CollisionInformation.LocalPosition);

                foreach(BlockData blockData in blockGroup.Blocks)
                {
                    // Obtain block type and instanced mesh for the block
                    BlockType blockType = blockTypes[blockGroup.BlockTypeName];
                    InstancedMesh<VertexData> instancedMesh = blockInstancedMeshes[blockGroup.BlockTypeName];

                    // Obtain physics body (a part of compound body) for the block
                    CompoundChild child = compoundBody.CollisionInformation.Children[childCollidableIndex];

                    // Create instance of the block in instanced mesh
                    InstancedMesh<VertexData>.Instance instance = instancedMesh.AppendInstance(Matrix.CreateScale(blockData.Scale) * child.Entry.LocalTransform.Matrix * localPosTransform);

                    // Store new block instance to the list
                    Block block = new Block(blockType, instance, child);
                    blocks.Add(block);

                    block.Scale = blockData.Scale;

                    childCollidableIndex++;
                }
            }

            // Add compound body and its constraints to physics space
            space.Add(compoundBody);
            space.Add(constraint);
        }

        public void RemoveBlock(Block block)
        {
            // Move block's physics entity to group of removed blocks 
            // so it will not collide with the balls
            block.Entity.CollisionInformation.CollisionRules.Group = removedBlocksGroup;

            // Remove graphical representation of the block
            String blockTypeName = block.BlockType.Name;
            InstancedMesh<VertexData> instancedMesh = blockInstancedMeshes[blockTypeName];
            instancedMesh.RemoveInstance(block.Instance);

            // Remove block from the list of blocks
            blocks.Remove(block);
        }

        public void ChangleBlockType(Block block, String newBlockTypeName)
        {
            // Obtain new block type for the block
            BlockType newBlockType = null;

            try
            {
                newBlockType = blockTypes[newBlockTypeName];
            }
            catch (KeyNotFoundException exc)
            { 
                // New block type does not exists in the game, try to load it
                newBlockType = LoadBlockType(newBlockTypeName, Blocks.Count);
            }

            if (newBlockType == null)
            { 
                // If not able to load new block type, just do nothing.
                // TODO: handle this error properly
                return;
            }

            // Remove graphical representation of the block
            String blockTypeName = block.BlockType.Name;
            InstancedMesh<VertexData> instancedMesh = blockInstancedMeshes[blockTypeName];
            instancedMesh.RemoveInstance(block.Instance);

            // Add new graphical representation of the block
            InstancedMesh<VertexData> newInstancedMesh = blockInstancedMeshes[newBlockTypeName];

            Matrix localPosTransform = Matrix.CreateTranslation(compoundBody.CollisionInformation.LocalPosition);
            InstancedMesh<VertexData>.Instance instance = newInstancedMesh.AppendInstance(Matrix.CreateScale(block.Scale) * block.Entity.Entry.LocalTransform.Matrix * localPosTransform);

            // Make changes in block instance
            block.ChangeBlockType(newBlockType, instance);
        }

        #endregion

        #region Rendering

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Hack: set blocks to initial position every frame.
            // TODO: implement this properly
            compoundBody.Position = Vector3.Zero;
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (KeyValuePair<String, InstancedMesh<VertexData>> pair in blockInstancedMeshes)
            {
                pair.Value.Draw(camera, compoundBody.WorldTransform);
            }
        }

        #endregion
    }
}
