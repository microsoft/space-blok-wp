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
using BlokContentData.GameLevelObjects.Animations;
using BlokGameObjects.GamePlatforms;
using BlokGameObjects.GameLevelObjects.ScoresParticles;

namespace BlokGameObjects.GameLevelObjects
{
    /// <summary>
    /// Game level class.
    /// Implements blocks logic and special effects
    /// </summary>
    public class GameLevelEx : GameLevel
    {
        private List<Animation> animations = new List<Animation>();
        private DepthStencilState depthStencilState = new DepthStencilState() { DepthBufferWriteEnable = false };

        private Dictionary<Platform, ScoresParticleSystem> scoresParticles = new Dictionary<Platform, ScoresParticleSystem>();

        public GameLevelEx(Game game)
            : base(game)
        { 
        }

        /// <summary>
        /// Create animation when ball hits the block
        /// </summary>
        /// <param name="block"></param>
        /// <param name="hitPos"></param>
        /// <param name="force"></param>
        public void AnimateHit(Block block, Vector3 hitPos, Platform platform)
        {
            ScoresParticleSystem pSystem = null;

            if (scoresParticles.ContainsKey(platform) == false)
            {
                // Lazy init for now
                Model model = Game.Content.Load<Model>(@"Models\plane");
                Texture2D texture = Game.Content.Load<Texture2D>(@"Images\particle01");

                pSystem = new ScoresParticleSystem(model, texture, 500, Game, Camera);
                pSystem.Initialize();
                pSystem.Platform = platform;

                scoresParticles.Add(platform, pSystem);
            }
            else
            { 
                pSystem = scoresParticles[platform];
            }

            pSystem.EmitParticles(hitPos, block.BlockType.ScoresPerHitPoint);

            animations.Add(new BlockHitAnimation(block, hitPos, platform));
        }

        /// <summary>
        /// Updates game level logic and effects.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Update animations and remove those which are ended
            int n = 0;
            while (n < animations.Count())
            {
                Animation animation = animations[n];

                bool finished = animation.Update(gameTime);

                if (finished == true)
                {
                    animations.Remove(animation);
                }
                else
                {
                    n++;
                }
            }

            foreach (KeyValuePair<Platform, ScoresParticleSystem> pair in scoresParticles)
            {
                pair.Value.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw special effects.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            DepthStencilState prevState = Game.GraphicsDevice.DepthStencilState;
            Game.GraphicsDevice.DepthStencilState = depthStencilState;

            foreach (BlockHitAnimation animation in animations)
            {
                animation.Draw(gameTime);
            }

            foreach (KeyValuePair<Platform, ScoresParticleSystem> pair in scoresParticles)
            {
                pair.Value.Draw(gameTime);
            }

            Game.GraphicsDevice.DepthStencilState = prevState;
        }

        public bool isVisibleParticles()
        {
            foreach (KeyValuePair<Platform, ScoresParticleSystem> score in scoresParticles)
            {
                if (score.Value.VisibleParticles() > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
