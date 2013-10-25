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
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUphysics.CollisionTests;
using Microsoft.Xna.Framework;
using BEPUphysics.CollisionRuleManagement;
using BlokGameObjects.GameLevelObjects;
using BlokGameObjects.GameLevelObjects.BlockEvents;
using BlokGameObjects.GamePlatforms;

namespace Blok
{
    class CollisionManager
    {
        private List<Platform> platforms;
        private GameLevelEx gameLevel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="platforms"></param>
        /// <param name="gameField"></param>
        public CollisionManager(List<Platform> platforms, GameLevelEx gameLevel)
        {
            this.platforms = platforms;
            this.gameLevel = gameLevel;

            foreach (Platform platform in platforms)
            {
                CollisionGroupPair groupPair = new CollisionGroupPair(gameLevel.RemovedBlocksGroup, platform.BallsGroup);
                CollisionRules.CollisionGroupRules.Add(groupPair, CollisionRule.NoBroadPhase);
            }

            gameLevel.Body.CollisionInformation.Events.ContactCreated += handleContactCreated;
        }

        /// <summary>
        /// Handles contact
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="other"></param>
        /// <param name="pair"></param>
        /// <param name="contact"></param>
        private void handleContactCreated(EntityCollidable sender, Collidable other, CollidablePairHandler pair, ContactData contact)
        {
            var compoundBody = sender as CompoundCollidable;
            var ballEntity = other as EntityCollidable;


            // Find exact block being hit by ball
            float minDistance = 0;
            CompoundChild closestChild = null;
            foreach (CompoundChild child in compoundBody.Children)
            {
                if (child.CollisionInformation.CollisionRules.Group != gameLevel.RemovedBlocksGroup)
                {
                    Vector3 p = Vector3.Transform(child.Entry.LocalTransform.Position, compoundBody.WorldTransform.Matrix);

                    float distance = 0.0f;
                    Vector3.DistanceSquared(ref contact.Position, ref p, out distance);

                    if (closestChild != null)
                    {
                        if (minDistance > distance)
                        {
                            closestChild = child;
                            minDistance = distance;
                        }
                    }
                    else
                    {
                        closestChild = child;
                        minDistance = distance;
                    }
                }
            }

            // Obtain block instance
            Block hittedBlock = null;
            foreach (Block block in gameLevel.Blocks)
            {
                if (block.Entity == closestChild)
                {
                    hittedBlock = block;
                    break;
                }
            }

            // Obtain ball instance
            Ball hittingBall = null;
            foreach (Platform platform in platforms)
            {
                foreach (Ball ball in platform.Balls)
                {
                    if (ball.Active == true && ball.Entity.CollisionInformation == ballEntity)
                    {
                        hittingBall = ball;
                        break;
                    }
                }
            }

            // We have obtained block and ball which are collided, now we can run game logic
            if (hittedBlock != null && hittingBall != null)
            {
                // Get hit points from the ball
                int ballHitPoints = hittingBall.HitPoints;

                int gainedHitPoints = Math.Min(hittedBlock.HitPoints, hittingBall.HitPoints);
                gainedHitPoints *= hittedBlock.BlockType.ScoresPerHitPoint;

                hittedBlock.HitPoints -= hittingBall.HitPoints;

                // Run block logic
                BlockType blockType = hittedBlock.BlockType;

                gameLevel.AnimateHit(hittedBlock, contact.Position, hittingBall.Platform);

                foreach (BlockEvent blockAction in blockType.Events)
                {
                    blockAction.HandleEvent(hittedBlock);
                }

                if (hittedBlock.HitPoints <= 0)
                {
                    blockType.Owner.RemoveBlock(hittedBlock);
                }
            }
        }
    }
}
