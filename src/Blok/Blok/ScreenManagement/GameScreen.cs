using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Blok
{
    /// <summary>
    /// Base class for all game screens
    /// </summary>
    public abstract class GameScreen
    {
        #region Fields & properties

        protected ScreenManager screenManager;

        public ScreenManager ScreenManager { get { return screenManager; } set { screenManager = value; } }

        #endregion

        #region Initialization

        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }

        #endregion

        #region Update & rendering

        public virtual void HandleInput(InputState input) { }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }

        #endregion
    }
}
