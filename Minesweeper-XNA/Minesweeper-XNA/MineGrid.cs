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
        private Mine[,] grid = new Mine[3,3];
        private Texture2D uncoveredGridItem;
        private Texture2D hiddenGridItem;
        private SpriteFont font;

        private SpriteBatch spriteBatch;

        public MineGrid(Game game)
            : base(game)
        {
            
        }

        public override void Initialize()
        {
            // initialize 3x3 grid with 1 mine
            Random rnd = new Random();
            int mineX = rnd.Next(0, 2);
            int mineY = rnd.Next(0, 2);

            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    Mine m;
                    if (i == mineX && j == mineY)
                    {
                        m = new Mine(true);
                    }
                    else
                    {
                        m = new Mine(false);
                    }
                    grid[i, j] = m;
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
            if (state.LeftButton == ButtonState.Pressed)
            {
                // change state to down but not pressed
            }
            else if (state.LeftButton == ButtonState.Released)
            {
                // uncover current grid item unless cursor is no longer over grid item
            }
            else if (state.RightButton == ButtonState.Released)
            {
                // place flag
            }
            else if (state.LeftButton == ButtonState.Released &&
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

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; i < grid.GetLength(1); j++)
                {
                    spriteBatch.Draw(hiddenGridItem, 
                                     new Rectangle(100 + i * gridSize,
                                                   100 + j * gridSize,
                                                   gridSize,
                                                   gridSize),
                                     Color.White);
                }
            }

            spriteBatch.End();
        }

        private int surroundingMines(int x, int y)
        {
            return 0;
        }
    }
}
