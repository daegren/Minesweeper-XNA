using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Minesweeper_XNA
{
    public class Mine : DrawableGameComponent
    {
        private bool isMine;
        private MineState mineState;
        private Vector2 gridPos;
        private Texture2D hiddenTexture;
        private Texture2D visibleTexture;
        private SpriteFont consoleFont;
        private Texture2D flagTexture;
        private SpriteBatch spriteBatch;
        private int surroundingMines;

        public Vector2 GridPosition { get { return gridPos; } }
        public MineState MineState { get { return mineState; } }
        public bool IsMine { get { return isMine; } }
        public int SurroundingMines { get { return surroundingMines; } set { surroundingMines = value; } }

        public Mine(Game game, bool isMine, int x, int y)
            : base(game)
        {
            this.isMine = isMine;
            gridPos = new Vector2(x, y);
        }

        public override void Initialize()
        {
            mineState = MineState.Hidden;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            ContentManager cm = Game.Content;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hiddenTexture = cm.Load<Texture2D>("grid-unknown");
            visibleTexture = cm.Load<Texture2D>("grid-0");
            flagTexture = cm.Load<Texture2D>("grid-flag");
            consoleFont = cm.Load<SpriteFont>("console");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Texture2D tex = hiddenTexture;
            Color c = Color.White;
            if (mineState == MineState.Hidden)
                tex = hiddenTexture;
            else if (mineState == MineState.HiddenFlaged)
                tex = flagTexture;
            else if (mineState == Minesweeper_XNA.MineState.UncoveredNone)
                tex = visibleTexture;
            else if (mineState == Minesweeper_XNA.MineState.UncoveredMine)
                tex = flagTexture;

            if (surroundingMines == 1)
                c = Color.Blue;
            else if (surroundingMines == 2)
                c = Color.Green;
            else if (surroundingMines == 3)
                c = Color.Red;
            else if (surroundingMines == 4)
                c = Color.DarkBlue;
                    
            spriteBatch.Begin();
            spriteBatch.Draw(tex, gridPos, Color.White);
            Vector2 a = new Vector2(gridPos.X + 16, gridPos.Y + 16);
            spriteBatch.DrawString(consoleFont, SurroundingMines + ": " + (isMine ? "T" : "F"), a, c);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void MinePressed(GameTime gameTime)
        {

        }

        public void MineReleased(GameTime gameTime)
        {
        }

        public void MineFlagged(GameTime gameTime)
        {
            if (mineState == Minesweeper_XNA.MineState.UncoveredMine ||
                mineState == Minesweeper_XNA.MineState.UncoveredNone ||
                mineState == Minesweeper_XNA.MineState.UncoveredNumber)
                return;
            if (mineState == Minesweeper_XNA.MineState.HiddenFlaged)
                mineState = Minesweeper_XNA.MineState.HiddenQuestion;
            else if (mineState == Minesweeper_XNA.MineState.HiddenQuestion)
                mineState = Minesweeper_XNA.MineState.Hidden;
            else if (mineState == Minesweeper_XNA.MineState.Hidden)
                mineState = Minesweeper_XNA.MineState.HiddenFlaged;
        }
    }

    public enum MineState
    {
        Hidden,
        HiddenFlaged,
        HiddenQuestion,
        UncoveredNumber,
        UncoveredMine,
        UncoveredNone
    }
}
