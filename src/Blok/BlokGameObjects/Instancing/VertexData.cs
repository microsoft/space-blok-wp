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

namespace BlokGameObjects.Instancing
{
    /// <summary>
    /// 
    /// </summary>
    public struct VertexData : IVertexType
    {
        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
        );

        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 TexCoords { get; set; }

        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }
    };
}
