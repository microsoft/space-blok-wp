using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BlokGameObjects.Instancing;

namespace BlokGameObjects.Particles
{
    public class Particle
    {
        public InstancedMesh<VertexData>.Instance Instance { get; set; }

        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }

        public float Slowdown { get; set; }
        public Vector3 Force { get; set; }
    }
}
