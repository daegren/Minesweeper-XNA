using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper_XNA
{
    public class MousePointer : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Vector2 mousePos;

        private MouseState currentState;
        private MouseState previousState;

        private Color pointerColor = Color.White;

        public Texture2D PointerTexture { get; set; }
        public Rectangle RestrictZone { get; set; }

        public MousePointer(Game game)
            : base(game)
        {
            //TODO: construct any child components here
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            currentState = Mouse.GetState();
            mousePos.X = currentState.X;
            mousePos.Y = currentState.Y;

            if (mousePos.X < 0)
                mousePos.X = 0;
            if (mousePos.X > RestrictZone.Width)
                mousePos.X = RestrictZone.Width;
            if (mousePos.Y < 0)
                mousePos.Y = 0;
            if (mousePos.Y > RestrictZone.Height)
                mousePos.Y = RestrictZone.Height;

            if (currentState.LeftButton == ButtonState.Pressed)
                pointerColor = Color.Blue;
            else if (currentState.RightButton == ButtonState.Pressed)
                pointerColor = Color.Red;
            else
                pointerColor = Color.White;

            previousState = currentState;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(PointerTexture, mousePos, pointerColor);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
