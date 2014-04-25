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

namespace BlokGameObjects.Particles
{
    /// <summary>
    /// Holds list of particles belonging to this group.
    /// </summary>
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
