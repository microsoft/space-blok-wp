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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BlokGameObjects;
using BlokGameObjects.GameLevelObjects;
using BlokGameObjects.GamePlatforms;

namespace BlokContentData.GameLevelObjects.Animations
{
    /// <summary>
    /// Animates blok hit by making the blok transparent.
    /// </summary>
    public class BlockHitAnimation : Animation
    {
        public static float ANIMATION_TIME = 500;

        public Block block;
        public float time = 0;

        public DrawableComponent light;

        public Vector3 hitPos;

        private Platform platform;
        private Vector3 platformPos;

        // Scores
        private Random randomGenerator = new Random();

        private class Score
        {
            public Vector3 pos;
            public Vector3 dir;
            public float dirSlowdown = 0.95f;

            public Vector3 force = Vector3.Zero;

            public Score(Vector3 pos, Vector3 dir)
            {
                this.pos = pos;
                this.dir = dir;
            }
        }

        private List<Score> scores = new List<Score>();

        public BlockHitAnimation(Block block, Vector3 hitPos, Platform platform)
        {
            this.block = block;

            GameLevel level = block.BlockType.Owner;


            light = new DrawableComponent(block.BlockType.Model, level.Camera, level.Game);
            light.LocalTransform = Matrix.CreateScale(block.Scale) * Matrix.CreateScale(1.1f);
            light.DiffuseColor = block.BlockType.Color;

            this.platform = platform;
            this.platformPos = platform.Position;
        }

        public bool Update(GameTime gameTime)
        {
            bool finished = false;

            // Update lighter model
            if (time < ANIMATION_TIME)
            {
                light.Transparency = 1.0f - time / ANIMATION_TIME;
            }

            time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;


            if (time > ANIMATION_TIME)
            {
                finished = true;
            }

            return finished;
        }

        public void Draw(GameTime gameTime)
        {
            light.WorldTransform = block.Entity.Entry.LocalTransform.Matrix * block.BlockType.Owner.Body.WorldTransform;
            light.Draw(gameTime);
        }
    }
}
