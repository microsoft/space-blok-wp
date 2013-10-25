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
using BlokGameObjects.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BlokGameObjects.GamePlatforms;

namespace BlokGameObjects.GameLevelObjects.ScoresParticles
{
    /// <summary>
    /// Defines the particle system for score particles.
    /// </summary>
    class ScoresParticleSystem : ParticleSystem
    {
        private Random randomGenerator;

        public Platform Platform { get; set; }

        public ScoresParticleSystem(Model model, Texture2D texture, int maxAmountOfParticles, Game game, Camera camera)
            : base(model, texture, maxAmountOfParticles, game, camera)
        {
            randomGenerator = new Random();
        }

        public void EmitParticles(Vector3 position, int count)
        {
            for (int n = 0; n < count; n++)
            {
                Vector3 dir = new Vector3();

                dir.X = (float)randomGenerator.NextDouble() - 0.5f;
                dir.Y = (float)randomGenerator.NextDouble() - 0.5f;
                dir.Z = (float)randomGenerator.NextDouble() - 0.5f;

                dir.Normalize();

                dir *= 0.3f;

                Particle p = CreateParticle();

                p.Position = position;
                p.Direction = dir;
                p.Slowdown = 0.95f;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            int n = 0;
            while (n < particles.Count())
            {
                Particle particle = particles[n];

                if (Vector3.DistanceSquared(Platform.Position, particle.Position) > 2.0f)
                {
                    particle.Position += particle.Direction;
                    particle.Direction *= particle.Slowdown;

                    // Update dir
                    Vector3 force = Vector3.Subtract(Platform.Position, particle.Position);
                    float ls = force.LengthSquared();
                    force.Normalize();
                    force *= 1 / ls;
                    particle.Force += force;

                    particle.Direction += 1.8f * force;

                    n++;

                    //Update graphics
                    instancedMesh.TransformInstance(particle.Instance, Matrix.CreateTranslation(particle.Position));
                }
                else
                {
                    RemoveParticle(particle);
                    Platform.AddScore(1);
                }
            }
        }
    }
}
