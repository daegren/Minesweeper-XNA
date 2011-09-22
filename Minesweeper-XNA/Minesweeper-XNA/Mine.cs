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
        private Texture2D questionTexture;
        private Texture2D mineTexture;

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
            questionTexture = cm.Load<Texture2D>("grid-question");
            mineTexture = cm.Load<Texture2D>("mine");
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
            else if (mineState == Minesweeper_XNA.MineState.UncoveredNone ||
                     mineState == Minesweeper_XNA.MineState.UncoveredNumber ||
                     mineState == Minesweeper_XNA.MineState.UncoveredMine ||
                     mineState == Minesweeper_XNA.MineState.HiddenSelected)
                tex = visibleTexture;
            else if (mineState == Minesweeper_XNA.MineState.HiddenQuestion)
                tex = questionTexture;

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
            Vector2 a = new Vector2(gridPos.X + 8, gridPos.Y + 8);
            if (isMine && mineState == Minesweeper_XNA.MineState.UncoveredMine)
                spriteBatch.Draw(mineTexture, a, Color.White);
            spriteBatch.DrawString(consoleFont, SurroundingMines + ": " + (isMine ? "T" : "F"), a, c);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void MinePressed(GameTime gameTime)
        {
            if (mineState == Minesweeper_XNA.MineState.Hidden ||
                mineState == Minesweeper_XNA.MineState.HiddenQuestion)
                mineState = Minesweeper_XNA.MineState.HiddenSelected;
        }

        /// <summary>
        /// Tells the mine it's been released
        /// </summary>
        /// <param name="gameTime">the gameTime object of the update call</param>
        /// <param name="sameMine">True if the mine should be revealed</param>
        public void MineReleased(GameTime gameTime, bool sameMine)
        {
            if (mineState == Minesweeper_XNA.MineState.HiddenSelected)
            {
                if (!sameMine)
                {
                    mineState = Minesweeper_XNA.MineState.Hidden;
                }
                else if (IsMine)
                {
                    mineState = Minesweeper_XNA.MineState.UncoveredMine;
                }
                else if (surroundingMines > 0)
                {
                    mineState = Minesweeper_XNA.MineState.UncoveredNumber;
                }
                else
                {
                    mineState = Minesweeper_XNA.MineState.UncoveredNone;
                    // TODO: uncover neihboring tiles w/ 0 or number too
                }
            }
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
        HiddenSelected,
        UncoveredNumber,
        UncoveredMine,
        UncoveredNone
    }
}
