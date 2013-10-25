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

namespace Blok.Particles
{
    /// <summary>
    /// Quad mesh for particle system
    /// </summary>
    class ParticleMesh
    {
        //// Vertex declaration for the mesh
        //private const VertexDeclaration vertexDecl = new VertexDeclaration
        //(
        //    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
        //    new VertexElement(12, VertexElementFormat.Vector4, VertexElementUsage.Color, 0),
        //    new VertexElement(28, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
        //);

        //// Geometry data for mesh vertex
        //private struct VertexData
        //{
        //    public Vector3 position;
        //    public Vector4 color;
        //    public Vector2 texCoords;
        //};

        //// Vertices of the mesh
        //private const VertexData[] vertices = new VertexData[] 
        //{
        //    new VertexData() 
        //    {
        //        position = new Vector3(-0.5f, 0.0f, -0.5f),
        //        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
        //        texCoords = new Vector2(0.0f, 1.0f)
        //    },
        //    new VertexData() 
        //    {
        //        position = new Vector3(-0.5f, 0.0f, 0.5f),
        //        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
        //        texCoords = new Vector2(0.0f, 0.0f)
        //    },
        //    new VertexData() 
        //    {
        //        position = new Vector3(0.5f, 0.0f, 0.5f),
        //        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
        //        texCoords = new Vector2(1.0f, 0.0f)
        //    },
        //    new VertexData() 
        //    {
        //        position = new Vector3(0.5f, 0.0f, -0.5f),
        //        color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
        //        texCoords = new Vector2(1.0f, 1.0f)
        //    }
        //};

        //// Indices of the mesh
        //private const short[] indices = new short[] 
        //{
        //    0, 1, 2, 
        //    0, 2, 3  
        //};

        ///// <summary>
        ///// Vertex declaration for mesh vertices
        ///// </summary>
        //public VertexDeclaration VertexDecl { get { return vertexDecl; } }
        ///// <summary>
        ///// Vertices of the mesh
        ///// </summary>
        //public VertexData[] Vertices { get { return vertices; } }
        ///// <summary>
        ///// Indices of the mesh
        ///// </summary>
        //public short[] Indices { get { return indices; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public ParticleMesh()
        { 
            
        }
    }
}
