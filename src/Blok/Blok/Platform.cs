using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics;
using BEPUphysics.CollisionRuleManagement;
using Blok.Particles;
using BlokGameObjects;

namespace Blok
{
    /// <summary>
    /// Defines platform game object consist of platform itself 
    /// and list of balls belonging to this platform.
    /// </summary>
    class Platform : DrawableGameComponent
    {
        // The minimum distance needed before swipe is registered
        private const float MINIMUM_SWIPE_DISTANCE = 40.0f;
        // Constant by which the velocity of the swipe is scaled (both dimensions separately). Bigger value means faster ball.
        private const float SWIPE_FORCE_MULTIPLIER = 15.0f;
        // Defines how big area measured from the platform center is allowed for starting points of the swipes
        private const float PLATFROM_SCREEN_SIZE = 60.0f;
        // The standard ball mass
        private const float STANDARD_BALL_MASS = 4.0f;
        // The standard ball bounceness
        private const float STANDARD_BALL_BOUNCINESS = 0.6f;

        private Camera camera;

        // Drawable for platform
        private DrawableComponent platformDrawable;

        // Collision group for the balls belonging to the platform
        private CollisionGroup ballsGroup = new CollisionGroup();

        // List of balls which are belong to the platform
        public List<Ball> balls = new List<Ball>();
        // Ball which is currently placed on platform ready to be launched.
        public Ball currentBall = null;

        // Data for handling user input
        private TouchLocation pressedLocations;
        private double pressedTimes;

        // Data for counting when exactly place new ball to the platform
        private float timePassed = 0.0f;
        private bool timeExpired = true;

        // Particles 
        ParticleSystem psystem;
        Ball pBall;
        double pTime;
        float distance;
        Vector3 lastPosition;

        /// <summary>
        /// Orientation angle of the platform, defines orientation of platform on the screen
        /// and direction where user can swipe the ball.
        /// </summary>
        public float OrientationAngle { get; private set; }
        /// <summary>
        /// Position of platform in 3D world space.
        /// </summary>
        public Vector3 Position { get; private set; }
        /// <summary>
        /// Position in screen space from where user can launch the ball by swiping.
        /// </summary>
        public Vector2 SwipeCenterPos { get; private set; }
        /// <summary>
        /// Default ball color
        /// </summary>
        public Vector3 BallColor { get; private set; }

        /// <summary>
        /// List of balls beloning to the platform.
        /// </summary>
        public List<Ball> Balls { get { return balls; } }
        /// <summary>
        /// Collision group for the balls belonging to the platform
        /// </summary>
        public CollisionGroup BallsGroup { get { return ballsGroup; } }

        public float Score { get; set; } 

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="orientationAngle"></param>
        /// <param name="swipeCenterPos"></param>
        /// <param name="ballColor"></param>
        /// <param name="game"></param>
        /// <param name="camera"></param>
        public Platform(Vector3 position, float orientationAngle, Vector2 swipeCenterPos, Vector3 ballColor, Game game, Camera camera) : base(game)
        {
            this.Position = position;
            this.OrientationAngle = orientationAngle;
            this.SwipeCenterPos = swipeCenterPos;
            this.BallColor = ballColor;
            this.camera = camera;
        }


        public override void Initialize()
        {
            // Load model for platform drawable
            Model platformModel = Game.Content.Load<Model>(@"Models\platform");

            // Create platform itself
            platformDrawable = new DrawableComponent(platformModel, camera, Game);

            platformDrawable.LocalTransform *= Matrix.CreateScale(2.5f);
            platformDrawable.LocalTransform *= Matrix.CreateRotationX(MathHelper.ToRadians(-90.0f));
            platformDrawable.LocalTransform *= Matrix.CreateRotationY(MathHelper.ToRadians(45.0f));
            platformDrawable.LocalTransform *= Matrix.CreateRotationY(MathHelper.ToRadians(OrientationAngle));

            platformDrawable.WorldTransform *= Matrix.CreateTranslation(Position);

            // Obtain physics space instance
            ISpace space = (ISpace)Game.Services.GetService(typeof(ISpace));

            // Load model for the balls
            Model ballModel = Game.Content.Load<Model>(@"Models\sphere");

            // Create balls
            for (int n = 0; n < 4; n++)
            {
                // Create entity for the ball
                Entity ballEntity = new Sphere(Position + new Vector3(0.0f, 0.5f, 0.0f), 0.5f);

                ballEntity.PositionUpdateMode = BEPUphysics.PositionUpdating.PositionUpdateMode.Continuous;
                ballEntity.CollisionInformation.CollisionRules.Group = ballsGroup;
                ballEntity.Material.Bounciness = STANDARD_BALL_BOUNCINESS;

                //space.Add(ballEntity);

                // Create drawable for the ball
                DrawableComponent ballDrawable = new DrawableComponent(ballModel, camera, Game);
                ballDrawable.DiffuseColor = BallColor;
                ballDrawable.LocalTransform = Matrix.CreateScale(0.5f);

                // Create ball instance
                Ball ball = new Ball(ballDrawable, ballEntity, this);

                balls.Add(ball);
            }

            // Set one ball to be active
            currentBall = balls[0];
            currentBall.Active = true;

            //// Just test for particle systems
            //Model model = Game.Content.Load<Model>(@"Meshes\plane");
            //Texture2D texture = Game.Content.Load<Texture2D>(@"Images\particle01");

            //psystem = new ParticleSystem(model, texture, 1000, Game, camera);
            //psystem.Initialize();
            //pBall = Balls[0];
        }

        /// <summary>
        /// Picks platform from screen coordinates.
        /// </summary>
        /// <returns>True if this platform is being picked, false otherwise.</returns>
        private bool PickPlatform(Vector2 pickPos)
        {
            bool picked = false;

            float squaredDistance = (SwipeCenterPos - pickPos).LengthSquared();
            if (squaredDistance < (PLATFROM_SCREEN_SIZE * PLATFROM_SCREEN_SIZE))
            {
                picked = true;
            }

            return picked;
        }

        /// <summary>
        /// Returns  true if the given swipe direction is allowed for the given platform.
        /// </summary>
        /// <param name="swipeDirection">Swipe direction in screen space</param>
        private bool CheckSwipeDirection(Vector2 swipeDirection)
        {
            bool result = false;

            Vector2 platformDirection = new Vector2(0.0f, 1.0f);

            platformDirection = Vector2.Transform(platformDirection, Matrix.CreateRotationZ(MathHelper.ToRadians(-OrientationAngle)));

            platformDirection.Normalize();
            swipeDirection.Normalize();

            float dotProduct = Vector2.Dot(platformDirection, swipeDirection);
            if (dotProduct > Math.Cos(MathHelper.ToRadians(45.0f)))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Handles input
        /// </summary>
        /// <param name="input"></param>
        public void HandleInput(InputState input)
        {
            if (currentBall == null)
            {
                return;
            }

            Vector2 result = Vector2.Zero;

            foreach (TouchLocation touchLocation in input.TouchState)
            {
                if (touchLocation.State == TouchLocationState.Pressed)
                {
                    // Is this platform picked?
                    bool picked = PickPlatform(touchLocation.Position);
                    // Ignore presses outside platforms
                    if (picked)
                    {
                        // Store the pressed touch location and timestamp
                        pressedTimes = DateTime.Now.TimeOfDay.TotalMilliseconds;
                        pressedLocations = touchLocation;
                    }
                }
                else if (touchLocation.State == TouchLocationState.Released)
                {
                    // Ignore releases which have no matching pressed id, ie. the swipe happens somewhere else than in one of the platforms
                    if (pressedLocations.Id == touchLocation.Id)
                    {
                        // Calculate the swipe data
                        double swipeTime = DateTime.Now.TimeOfDay.TotalMilliseconds - pressedTimes;
                        Vector2 swipeDir = touchLocation.Position - pressedLocations.Position;

                        // React to the swipe only, if the distance is longer than specified minimum (prevent accidental swipes)
                        if (swipeDir.LengthSquared() > (MINIMUM_SWIPE_DISTANCE * MINIMUM_SWIPE_DISTANCE))
                        {
                            swipeDir *= (float)(SWIPE_FORCE_MULTIPLIER / swipeTime);

                            // Check that the ball is in the starting position before accepting the swipe
                            if (CheckSwipeDirection(swipeDir))
                            {
                                // Add ball to physics space and to 
                                ISpace space = (ISpace)Game.Services.GetService(typeof(ISpace));
                                space.Add(currentBall.Entity);

                                // Launch the ball
                                currentBall.Entity.BecomeDynamic(STANDARD_BALL_MASS);
                                currentBall.Entity.ApplyImpulse(currentBall.Entity.Position, new Vector3(swipeDir.X, 0.0f, swipeDir.Y));
                                
                                // Move current ball to active balls list
                                //activeBalls.Add(currentBall);
                                currentBall = null;

                                // Start timer
                                timePassed = 0.0f;
                                timeExpired = false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates state
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Check if ball has fallen down. In this case make it inactive.
            foreach(Ball ball in balls)
            {
                ball.Update(gameTime);

                if (ball.Active == true && ball.Entity.Position.Y < -20.0f)
                {
                    ball.Entity.BecomeKinematic();

                    ball.Entity.LinearVelocity = Vector3.Zero;
                    ball.Entity.LinearMomentum = Vector3.Zero;
                    ball.Entity.AngularVelocity = Vector3.Zero;
                    ball.Entity.AngularMomentum = Vector3.Zero;

                    ball.Entity.Position = Position;

                    ISpace space = (ISpace)Game.Services.GetService(typeof(ISpace));
                    space.Remove(ball.Entity);

                    ball.Active = false;

                    if (ball == pBall)
                    {
                        lastPosition = Position;
                        distance = 0;
                    }
                }
            }

            // Check if it is time to place new ball to the platform
            if (timeExpired == false)
            {
                timePassed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (timePassed > 750.0f)
                {
                    timeExpired = true;
                    timePassed = 0.0f;
                }
            }
            else 
            {
                // Time expired but we still don't have ball on the platform.
                if (currentBall == null)
                {
                    // Find first unused ball.
                    Ball unusedBall = null;
                    foreach (Ball ball in balls)
                    {
                        if (ball.Active == false)
                        {
                            unusedBall = ball;
                            break;
                        }
                    }

                    // If some unused ball exists, place it to the platform.
                    if (unusedBall != null)
                    {
                        currentBall = unusedBall;
                        currentBall.Active = true;
                    }
                }
            }

            //// 
            //float pPeriod = 0.05f;

            //Vector3 ballPos = pBall.Entity.Position;
            //distance += (ballPos - lastPosition).Length();
            //lastPosition = ballPos;

            //while (distance >= pPeriod)
            //{
            //    psystem.AddParticle(ballPos, 1000);
            //    distance -= pPeriod;
            //}

            //for (int n = 0; n < 2; n++ )
            //    psystem.Update(gameTime);
        }

        /// <summary>
        /// Draws
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // Render platfrom
            platformDrawable.Draw(gameTime);

            // Render balls
            foreach (Ball ball in balls)
            {
                if (ball.Active == true)
                {
                    ball.Drawable.Draw(gameTime);
                }
            }

            //psystem.Draw(gameTime);
        }
    }
}
