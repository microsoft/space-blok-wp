using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.Constraints.SingleEntity;
using BEPUphysics.CollisionShapes;
using Microsoft.Xna.Framework;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.CollisionRuleManagement;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.CollisionTests;
using BEPUphysics.Entities;

namespace Blok
{
    /// <summary>
    /// GameField class contains physical simulation and graphical representation
    /// of piles of blocks.
    /// </summary>
    class GameField : DrawableGameComponent
    {
        #region Fields & properties

        private const int GAME_FIELD_SIZE = 10;
        private const float BLOCK_SIZE = 1.0f;

        private const float COMPOUND_BODY_MASS = 150.0f;
        private const float COMPOUND_BODY_ANGULAR_DAMPING = 0.01f;

        private Space space;

        private CompoundBody compoundBody;
        private MaximumLinearSpeedConstraint constraint;

        private CollisionGroup removedBlocksGroup;

        GraphicsDevice device;

        private Camera camera;

        private Dictionary<String, InstancedMesh> blockInstancedMeshes;
        private List<Block> blocks;

        public CollisionGroup RemovedBlocksGroup { get { return removedBlocksGroup; } }

        public Entity Body { get { return compoundBody; } }

        public List<Block> Blocks { get { return blocks; } }

        #endregion

        #region Initializing

        //private struct Block
        //{
        //    public InstancedMesh instancedMesh;
        //    public int instanceIndex;

        //    public Block(InstancedMesh instancedMesh, int instanceIndex)
        //    {
        //        this.instancedMesh = instancedMesh;
        //        this.instanceIndex = instanceIndex;
        //    }
        //}

        //private Dictionary<CompoundChild, Block> blocks;

        public GameField(Game game, Space space, Camera camera) : base(game)
        {
            this.device = game.GraphicsDevice;
            this.space = space;
            this.camera = camera;

            Init();
        }

        private void Init()
        {
            blockInstancedMeshes = new Dictionary<string, InstancedMesh>();
            //blocks = new Dictionary<CompoundChild, Block>();
            blocks = new List<Block>();
            CreateCompoundBody();
        }

        private void CreateCompoundBody()
        {
            List<CompoundShapeEntry> shapes = new List<CompoundShapeEntry>();

            // Create parts of compound shape
            Vector3 blockPos = new Vector3();
            for (int z = 0; z < GAME_FIELD_SIZE; ++z)
            {
                for (int y = 0; y < GAME_FIELD_SIZE; ++y)
                {
                    for (int x = 0; x < GAME_FIELD_SIZE; ++x)
                    {
                        blockPos.X = (x - GAME_FIELD_SIZE * 0.5f) * BLOCK_SIZE + BLOCK_SIZE * 0.5f;
                        blockPos.Y = (y - GAME_FIELD_SIZE * 0.5f) * BLOCK_SIZE + BLOCK_SIZE * 0.5f;
                        blockPos.Z = (z - GAME_FIELD_SIZE * 0.5f) * BLOCK_SIZE + BLOCK_SIZE * 0.5f;

                        BoxShape blockShape = new BoxShape(BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE);

                        CompoundShapeEntry entry = new CompoundShapeEntry(blockShape, blockPos);
                        shapes.Add(entry);
                    }
                }
            }

            // Create compound body
            compoundBody = new CompoundBody(shapes, COMPOUND_BODY_MASS);

            compoundBody.PositionUpdateMode = BEPUphysics.PositionUpdating.PositionUpdateMode.Continuous;
            compoundBody.AngularDamping = COMPOUND_BODY_ANGULAR_DAMPING;

            // Add constraint prevents compound body from moving
            constraint = new MaximumLinearSpeedConstraint(compoundBody, 0.0f);

            // Create collision group for removed blocks
            removedBlocksGroup = new CollisionGroup();

            // Mark all chapes in compound body to be removed for now
            foreach (CompoundChild child in compoundBody.CollisionInformation.Children)
            {
                child.CollisionInformation.CollisionRules.Group = removedBlocksGroup;
            }

            // Add compound body and its constraints to physics space
            space.Add(compoundBody);
            space.Add(constraint);

            //compoundBody.CollisionInformation.Events.ContactCreated += handleContactCreated;
        }

        #endregion

        #region Blocks management

        public InstancedMesh AddBlockType(String blockType, ModelMeshPart meshData, Matrix transform, int maxAmountOfInstances)
        {
            InstancedMesh instancedMesh = new InstancedMesh(device, meshData, transform, maxAmountOfInstances);
            blockInstancedMeshes.Add(blockType, instancedMesh);
            return instancedMesh;
        }

        public void AddBlock(String blockType, int x, int y, int z)
        { 
            // Obtain compound child for the block
            int index = x + y * GAME_FIELD_SIZE + z * GAME_FIELD_SIZE * GAME_FIELD_SIZE;
            CompoundChild child = compoundBody.CollisionInformation.Children[index];

            child.CollisionInformation.CollisionRules.Group = null;

            // Obtain instanced mesh for the block
            InstancedMesh instancedMesh = blockInstancedMeshes[blockType];

            // Create instance of the block in instanced mesh
            Matrix transform = child.Entry.LocalTransform.Matrix; //Matrix.CreateTranslation(x + BLOCK_SIZE * 0.5f, y + BLOCK_SIZE * 0.5f, z + BLOCK_SIZE * 0.5f);
            int instanceIndex = instancedMesh.AppendInstance(transform);

            // Store new instance to the list
            Block block = new Block(instancedMesh, instanceIndex, child);

            blocks.Add(block);
        }

        public void AddBlock(String blockType, Matrix transform)
        {
            // Obtain instanced mesh for the block
            InstancedMesh instancedMesh = blockInstancedMeshes[blockType];

            int instanceIndex = instancedMesh.AppendInstance(transform);
        }

        public void RemoveBlock(Block block)
        {
            block.entity.CollisionInformation.CollisionRules.Group = removedBlocksGroup;

            // Remove graphical representation of the block
            block.instancedMesh.RemoveInstance(block.instanceIndex);
            block.instancedMesh.CommitChanges();

            blocks.Remove(block);
        }

        public void CommitChanges()
        {
            foreach (KeyValuePair<String, InstancedMesh> pair in blockInstancedMeshes)
            {
                pair.Value.CommitChanges();
            }
        }

        #endregion

        #region Rendering

        public override void Draw(GameTime gameTime)
        {
            foreach (KeyValuePair<String, InstancedMesh> pair in blockInstancedMeshes)
            {
                pair.Value.Draw(camera, compoundBody.WorldTransform);
            }
        }

        #endregion
    }
}
