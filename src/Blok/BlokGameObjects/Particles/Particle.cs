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
using BlokGameObjects.Instancing;

namespace BlokGameObjects.Particles
{
    /// <summary>
    /// Holds the atrtibutes of particle, such a position, direction and
    /// the force currently affecting to the particle.
    /// </summary>
    public class Particle
    {
        public InstancedMesh<VertexData>.Instance Instance { get; set; }

        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }

        public float Slowdown { get; set; }
        public Vector3 Force { get; set; }
    }
}
