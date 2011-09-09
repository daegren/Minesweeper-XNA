using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper_XNA
{
    public class MineGrid : DrawableGameComponent
    {
        int gridSize = 5;
        int tileSize = 64;
        int gridOffset = 50;
        private Mine[,] grid = new Mine[6,6];
        private Texture2D uncoveredGridItem;
        private Texture2D hiddenGridItem;
        private SpriteFont font;

        private bool isRightButtonPressed = false;
        private bool isLeftButtonPressed = false;

        private Mine selectedMine;
        private Mine tempMine;

        private SpriteBatch spriteBatch;

        public MineGrid(Game game)
            : base(game)
        {
            
        }

        public override void Initialize()
        {
            selectedMine = new Mine(Game, false, 0, 0);
            tempMine = selectedMine;
            // initialize 3x3 grid with 1 mine
            //Random rnd = new Random();
            int[] mine1 = { 2, 1 };
            int[] mine2 = { 4, 1 };
            int[] mine3 = { 1, 3 };
            int[] mine4 = { 4, 4 };            

            for (int i = 0; i <= gridSize; i++)
            {
                for (int j = 0; j <= gridSize; j++)
                {
                    bool t;
                    if ((i == mine1[0] && j == mine1[1]) ||
                        (i == mine2[0] && j == mine2[1]) ||
                        (i == mine3[0] && j == mine3[1]) ||
                        (i == mine4[0] && j == mine4[1]))
                    {
                        t = true;
                    }
                    else
                    {
                        t = false;
                    }
                    Mine m = new Mine(Game, t, gridOffset + i * tileSize, gridOffset + j * tileSize);
                    grid[i, j] = m;
                    Game.Components.Add(m);
                }
            }

            for (int i = 0; i <= gridSize; i++)
            {
                for (int j = 0; j <= gridSize; j++)
                {
                    Mine m = grid[i, j];
                    for (int k = i - 1; k <= i + 1; k++)
                    {
                        for (int l = j - 1; l <= j + 1; l++)
                        {
                            if (k < 0 || l < 0 || k > gridSize || l > gridSize || (i == k && j == l))
                                continue;
                            Mine t = grid[k, l];
                            if (t.IsMine)
                                m.SurroundingMines++;
                        }
                    }
                }
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            uncoveredGridItem = Game.Content.Load<Texture2D>("grid-0");
            hiddenGridItem = Game.Content.Load<Texture2D>("grid-unknown");
            font = Game.Content.Load<SpriteFont>("console");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();
            // figure out position of mouse click
            int x = (state.X - gridOffset) / tileSize;
            int y = (state.Y - gridOffset) / tileSize;
            Mine m = null;
            if (!(x < 0 || x > gridSize || y < 0 || y > gridSize))
                m = grid[x, y];

            if (state.LeftButton == ButtonState.Pressed)
            {
                // change state to down but not pressed
                // dont if there its flagged
                if (m != null)
                {
                    m.MinePressed(gameTime);
                    selectedMine = m;
                }
            }
            else if (state.LeftButton == ButtonState.Released)
            {
                // uncover current grid item unless cursor is no longer over grid item
                if (m == selectedMine)
                {
                    m.MineReleased(gameTime, true);
                    selectedMine = tempMine;
                }
                else if (m != null)
                    m.MineReleased(gameTime, false);
                
            }
            if (state.RightButton == ButtonState.Pressed)
            {
                isRightButtonPressed = true;
            }
            if (state.RightButton == ButtonState.Released)
            {
                // place flag
                if (m != null && isRightButtonPressed)
                {
                    m.MineFlagged(gameTime);
                    isRightButtonPressed = false;
                }
            }
            if (state.LeftButton == ButtonState.Released &&
                     state.RightButton == ButtonState.Released)
            {
                // if number matches number of flags, uncover surrounding grid items
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            int gridSize = hiddenGridItem.Height;
            spriteBatch.Begin();

            spriteBatch.End();
        }

        private int surroundingMines(int x, int y)
        {
            return 0;
        }
    }
}
