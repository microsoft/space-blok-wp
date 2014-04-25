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
using BlokGameObjects.Instancing;
using BlokGameObjects;

namespace BlokGameObjects.Particles
{
    /// <summary>
    /// Represents a particle system.
    /// </summary>
    public abstract class ParticleSystem : DrawableGameComponent
    {
        protected InstancedMesh<VertexData> instancedMesh;

        protected List<Particle> particles = new List<Particle>();
        protected List<Particle> particlesPool = new List<Particle>();

        private Camera camera;

        public int VisibleParticles()
        {
            return particles.Count;
        }

        public ParticleSystem(Model model, Texture2D texture, int maxAmountOfParticles, Game game, Camera camera)
            : base(game)
        {
            Matrix transform = Matrix.CreateRotationX(MathHelper.ToRadians(-90.0f));
            transform *= Matrix.CreateScale(0.75f);

            instancedMesh = new InstancedMesh<VertexData>(game.GraphicsDevice, model.Meshes[0].MeshParts[0], transform, maxAmountOfParticles);

            instancedMesh.Effect.Texture = texture;
            instancedMesh.Effect.TextureEnabled = true;
            instancedMesh.Effect.LightingEnabled = false;
            //instancedMesh.Effect.Alpha = 0.5f;

            this.camera = camera;
        }

        public Particle CreateParticle()
        {
            Particle particle = null;

            if (particlesPool.Count() > 0)
            { 
                particle = particlesPool[0];
                particlesPool.RemoveAt(0);

                particle.Position = Vector3.Zero;
                particle.Direction = Vector3.Zero;
                particle.Force = Vector3.Zero;
            }
            else
            {
                particle = new Particle();
            }

            // Create mesh for the particle
            InstancedMesh<VertexData>.Instance instance = instancedMesh.AppendInstance(Matrix.Identity);
            particle.Instance = instance;

            particles.Add(particle);

            return particle;
        }

        public void RemoveParticle(Particle particle)
        {
            // Remove graphical representation of the particle
            instancedMesh.RemoveInstance(particle.Instance);

            // Mode particle from list of live particles to particles pool
            particles.Remove(particle);
            particlesPool.Add(particle);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            DepthStencilState prevDepthStencilState = GraphicsDevice.DepthStencilState;
            GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            instancedMesh.Draw(camera, Matrix.Identity);

            GraphicsDevice.DepthStencilState = prevDepthStencilState;
        }
    }
}
