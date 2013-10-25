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
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using BlokGameObjects;

namespace BlokGameObjects.Instancing
{
    /// <summary>
    /// InstancedMesh emulates instancing technology for rendering large amount of 
    /// mesh instances. This is done by adding mesh geometry data into large 
    /// vertex buffer several times.
    /// </summary>
    public class InstancedMesh<VertexType> where VertexType : struct, IVertexType
    {
        public class Instance
        {
            public int Index { get; set; }
        };

        #region Fields

        private GraphicsDevice device;

        private int maxAmountOfInstances;

        private List<Instance> instances = new List<Instance>();

        // Original mesh to be instanced
        private Matrix meshDataTransform;

        private VertexBuffer meshVertexBuffer;
        private IndexBuffer meshIndexBuffer;
        
        // The effect for rendering instanced meshes.
        private BasicEffect effect;
        private VertexData[] vertices;
        private short[] indices;

        private VertexData[] meshVertices;
        private short[] meshIndices;

        #endregion

        #region Properties

        public BasicEffect Effect { get { return effect; } }

        #endregion

        #region Initializing

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="device">device.</param>
        /// <param name="meshData">Mesh data to be used for instancing.</param>
        /// <param name="meshDataTransform">Local transform which will be applied to meshData before adding its instances to instancedMesh.</param>
        /// <param name="maxAmountOfInstances">Maximum amount of mesh instaces.</param>
        public InstancedMesh(GraphicsDevice device, ModelMeshPart meshData, Matrix meshDataTransform, int maxAmountOfInstances) : 
            this(device, meshData.VertexBuffer, meshData.IndexBuffer, meshDataTransform, maxAmountOfInstances)
        {
        }

        public SamplerState ss;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="device">device.</param>
        /// <param name="meshVertexBuffer">Vertex buffer of the mesh to be instanced.</param>
        /// <param name="indexBuffer">Index buffer of the mesh to be instanced.</param>
        /// <param name="meshDataTransform">Local transform which will be applied to meshData before adding its instances to instancedMesh.</param>
        /// <param name="maxAmountOfInstances">Maximum amount of mesh instaces.</param>
        public InstancedMesh(GraphicsDevice device, VertexBuffer meshVertexBuffer, IndexBuffer meshIndexBuffer, 
            Matrix meshDataTransform, int maxAmountOfInstances)
        {
            this.device = device;

            this.meshVertexBuffer = meshVertexBuffer;
            this.meshIndexBuffer = meshIndexBuffer;

            this.meshDataTransform = meshDataTransform;

            this.maxAmountOfInstances = maxAmountOfInstances;

            // Initialize vertex and index buffer and data arrays for it
            InitBuffers();

            // And also create effect for all this stuff
            effect = new BasicEffect(device);

            effect.LightingEnabled = true;
            effect.VertexColorEnabled = false;
            effect.PreferPerPixelLighting = false;

            effect.AmbientLightColor = new Vector3(0.25f, 0.25f, 0.25f);

            Vector3 lightDiffuseColor = new Vector3(0.75f, 0.75f, 0.75f);

            effect.DirectionalLight0.DiffuseColor = lightDiffuseColor;
            effect.DirectionalLight1.DiffuseColor = lightDiffuseColor;
            effect.DirectionalLight2.DiffuseColor = lightDiffuseColor;

            effect.FogColor = new Color(11, 26, 38).ToVector3();
            effect.FogStart = 30.0f;
            effect.FogEnd = 40.0f;
            effect.FogEnabled = true;

            ss = new SamplerState();
            ss.Filter = TextureFilter.Point;
        }

        public void InitBuffers()
        {
            // Get mesh vertex data
            meshVertices = new VertexData[meshVertexBuffer.VertexCount];
            meshVertexBuffer.GetData<VertexData>(meshVertices);

            // Get mesh index data
            meshIndices = new short[meshIndexBuffer.IndexCount];
            meshIndexBuffer.GetData<short>(meshIndices);

            // Create vertex and index data arrays for instances
            vertices = new VertexData[meshVertices.Count() * maxAmountOfInstances];
            indices = new short[meshIndices.Count() * maxAmountOfInstances];
        }

        #endregion

        #region Instances management

        public Instance AppendInstance(Matrix localTransform)
        {
            int instanceIndex = instances.Count();
            
            int vertexCount = meshVertices.Count();
            int startVertexIndex = instanceIndex * vertexCount;

            // Copy vertex data into instances array
            meshVertices.CopyTo(vertices, startVertexIndex);

            // Compute transform for indices
            Matrix transform = meshDataTransform * localTransform;

            // Obtain position part of the vertex
            int posOffset = -1;

            VertexElement[] vertexElements = VertexData.VertexDeclaration.GetVertexElements();
            foreach (VertexElement element in vertexElements)
            {
                if (element.VertexElementUsage == VertexElementUsage.Position)
                {
                    posOffset = element.Offset;
                }
            }

            // Transform verties of instance
            for (int n = 0; n < vertexCount; ++n)
            { 
                //VertexData vertex = vertices[startVertexIndex + n];

                // Change position
                vertices[startVertexIndex + n].Position = Vector3.Transform(vertices[startVertexIndex + n].Position, transform);
                vertices[startVertexIndex + n].Normal = Vector3.TransformNormal(vertices[startVertexIndex + n].Normal, transform);

            }

            // Now indices..
            int indexCount = meshIndices.Count();
            int startIndexIndex = instanceIndex * indexCount;

            // Copy index data into instances array
            meshIndices.CopyTo(indices, startIndexIndex);

            // Fix indices of instance
            for (int n = 0; n < indexCount; ++n)
            {
                indices[startIndexIndex + n] += (short)(startVertexIndex);
            }

            // Create instance
            Instance instance = new Instance();
            instance.Index = instanceIndex;
            instances.Add(instance);

            return instance;
        }

        /// <summary>
        /// Apply tranform to instance.
        /// </summary>
        /// <param name="instanceIndex">Index of instance.</param>
        /// <param name="localTransform">Transform to be applied to instance</param>
        public void TransformInstance(Instance instance, Matrix localTransform)
        {
            int vertexCount = meshVertices.Count();
            int startVertexIndex = instance.Index * vertexCount;

            // Copy vertex data into instances array
            meshVertices.CopyTo(vertices, startVertexIndex);

            // Compute transform for indices
            Matrix transform = meshDataTransform * localTransform;

            // Transform verties of instance
            for (int n = 0; n < vertexCount; ++n)
            {
                //VertexData vertex = vertices[startVertexIndex + n];

                // Change position
                vertices[startVertexIndex + n].Position = Vector3.Transform(vertices[startVertexIndex + n].Position, transform);
                vertices[startVertexIndex + n].Normal = Vector3.TransformNormal(vertices[startVertexIndex + n].Normal, transform);
            }
        }

        /// <summary>
        /// Removes mesh instance.
        /// This operation must be committed by calling commitChanges() method. 
        /// </summary>
        /// <param name="instanceIndex">Index of the instance to be removed. This is value returned by appendMesh method.</param>
        public void RemoveInstance(Instance instance)
        {
            if (instances.Count() == 0)
            {
                return;
            }

            if (instances.Count() > 1)
            {

                // Find instance with last index
                Instance maxInstance = null;
                foreach (Instance inst in instances)
                {
                    if (maxInstance == null)
                    {
                        maxInstance = inst;
                    }
                    else
                    {
                        if (inst.Index > maxInstance.Index)
                        {
                            maxInstance = inst;
                        }
                    }
                }

                // Move last instance on place of instance to be deleted
                int meshVertexCount = meshVertices.Count();
                int removeStartIndex = instance.Index * meshVertexCount;
                int moveStartIndex = maxInstance.Index * meshVertexCount;

                for (int n = 0; n < meshVertexCount; ++n)
                {
                    vertices[removeStartIndex + n] = vertices[moveStartIndex + n];
                }

                maxInstance.Index = instance.Index;
            }

            // Remove instance
            instances.Remove(instance);
        }

        #endregion

        #region Rendering

        /// <summary>
        /// Redners instanced meshes.
        /// </summary>
        /// <param name="camera">Camera.</param>
        /// <param name="transform">Transform.</param>
        public void Draw(Camera camera, Matrix transform)
        {
            if (instances.Count() > 0)
            {
                device.SamplerStates[0] = ss;

                effect.World = transform;
                effect.View = camera.View;
                effect.Projection = camera.Projection;

                effect.CurrentTechnique.Passes[0].Apply(); // TODO: implement rendering code in proper way. Apply all effect passes.

                int vertexCount = instances.Count() * meshVertices.Count();
                int primitiveCount = instances.Count() * meshIndices.Count() / 3;

                device.DrawUserIndexedPrimitives<VertexData>(PrimitiveType.TriangleList, vertices, 0, vertexCount, indices, 0, primitiveCount);//, vertexDecl);
            }
        }

        #endregion
    }
}
