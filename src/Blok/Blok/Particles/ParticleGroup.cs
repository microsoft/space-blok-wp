using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlokGameObjects.Particles
{
    class ParticleGroup
    {
        public List<Particle> Particles { get; private set; }

        public ParticleGroup()
        {
            Particles = new List<Particle>();
        }

        public void Update()
        {
        }
    }
}
