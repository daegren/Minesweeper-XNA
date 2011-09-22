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
        private Mine[,] grid = new Mine[6, 6];
        private Texture2D uncoveredGridItem;
        private Texture2D hiddenGridItem;
        private SpriteFont font;

        private bool isRightButtonPressed = false;
        private bool isLeftButtonPressed = false;

        private Mine selectedMine;
        private Mine tempMine;
        private String selectedMineState = "";

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
            if (state.X - gridOffset < gridSize)
                x = -1;
            if (state.Y - gridOffset < gridSize)
                y = -1;
            Mine m = null;
            if ((x >= 0 && x <= gridSize) && (y >= 0 && y <= gridSize))
                m = grid[x, y];

            if (state.LeftButton == ButtonState.Pressed)
            {
                if (m != null) // if a mine is selected
                {
                    if (selectedMine != m) // if the selected mine is not the same one from last update
                    {
                        selectedMine.MineReleased(gameTime, false); // hide old mine
                        m.MinePressed(gameTime); // press new mine
                        selectedMine = m; // set new mine to selected
                    }
                }
            }
            else if (state.LeftButton == ButtonState.Released)
            {
                if (m == null) // if no mine selected
                {
                    selectedMine.MineReleased(gameTime, false); // hide selected mine
                }
                else if (selectedMine == m) // if selected mine is same as current mine
                {
                    m.MineReleased(gameTime, true); // show mine
                }
                selectedMine = tempMine; // reset selected mine
            }
            if (m == null)
                selectedMineState = selectedMine.MineState.ToString();
            else
                selectedMineState = m.MineState.ToString();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, selectedMineState, new Vector2(500, 120), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private int surroundingMines(int x, int y)
        {
            return 0;
        }
    }
}
