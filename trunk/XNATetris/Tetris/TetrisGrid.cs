using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace XNATetris
{
    class TetrisGrid : TetrisConstants
    {

        //false when empty-true when occupied
        private bool[] mCells;
        private int mTileW;
        private int mTileH;
        //xna values
        private Rectangle background;

        public TetrisGrid()
        {
            background = new Rectangle();
            mCells = new bool[PLAYFIELD_ROWS * PLAYFIELD_COLS];
            init();
        }

        public void init()
        {
            for (int i = 0; i < mCells.Length; i++)
            {
                mCells[i] = false;
            }
        }

        public void setBackGroundDimentions(int w, int h)
        {
            if (PLAYFIELD_USE_MARGINS)
            {
                background.X = MARGIN_LEFT;
                background.Y = MARGIN_TOP;
                w -= background.X + MARGIN_RIGHT;
                h -= background.Y + MARGIN_BOTTOM;
            }
           // else
            {
               // mTop = mLeft = 0;
            }

            //int cols = PLAYFIELD_COLS;
            int rows = PLAYFIELD_ROWS;
            mTileH = h / rows;
            //mTileW = w / cols;
            //hack force square
            mTileW = mTileH;


            background.Width = (mTileW * PLAYFIELD_COLS);
            background.Height = (mTileH * PLAYFIELD_ROWS);
            
        }

        public int getColumn(int index)
        {
            if (index < 0)
                return -((Math.Abs(index) % PLAYFIELD_COLS) + 1);
            else
                return (index % PLAYFIELD_COLS);
        }

        public int getRow(int index)
        {
            if (index < 0)
                return -((Math.Abs(index) / PLAYFIELD_COLS) + 1);
            else
                return (index / PLAYFIELD_COLS);
        }

        public bool IsCellValid(int index)
        {
            return (index >= 0 && index < mCells.Length);
        }

        public bool IsCellFree(int index)
        {
            if (IsCellValid(index))
                return !mCells[index];
            else
                return false;
        }

        private bool checkForRunOff(int[] from, int[] to)
        {

            int[] testArray = (int[])to.Clone();
            for (int i = testArray.Length - 1; i >= 0; i--)
            {
                testArray[i] -= testArray[0];
                testArray[i] += START_CELL;
            }

            //normalize the test array to test rowDiff
            for (int i = 0; i < to.Length; i++)
            {
                if (getRow(to[i]) - getRow(to[0]) != getRow(testArray[i]) - getRow(testArray[0]))
                    return false;
            }
            return true;
        }

        public bool tryToMoveCells(int[] from, int[] to)
        {

            //test grid
            if (!checkForRunOff(from, to))
                return false;

            bool validMove = false;
            for (int i = 0; i < to.Length; i++)
            {
                bool cellAboveGrid = to[i] < 0; //can happen on init
                if (!XNATetris.Tools.ArrayTools.IsInArray(to[i], from))
                    if (IsCellFree(to[i]) || cellAboveGrid)
                        validMove = true;
                    else
                        return false;
            }

            //write to grid
            if (validMove)
            {
                for (int i = 0; i < from.Length; i++)
                {
                    if (IsCellValid(from[i]))
                        mCells[from[i]] = false;
                }
                for (int i = 0; i < to.Length; i++)
                {
                    bool cellAboveGrid = to[i] < 0; //can happen on init
                    if (!cellAboveGrid)
                        mCells[to[i]] = true;
                }
            }

            return validMove;
        }


        public void paint()
        {
            //paint bg
            TetrisGame.spriteBatch.Draw(TetrisGame.Pixel, background, Color.Gray);

            //paint elems
            int l, t;
            for (int i = 0; i < mCells.Length; i++)
            {
                l = background.Left + (i % PLAYFIELD_COLS) * mTileW;
                t = background.Top + (i / PLAYFIELD_COLS) * mTileH;
                Rectangle cell = new Rectangle(l, t, mTileW, mTileH);
                TetrisGame.ShapeManager.DrawRect(TetrisGame.Pixel, TetrisGame.spriteBatch, cell, Color.Yellow);

                if(mCells[i])//more fancy graphics for occupied cells
                {
                    TetrisGame.spriteBatch.Draw(TetrisGame.Pixel, cell, Color.Yellow);
                }
            }

            TetrisGame.ShapeManager.DrawRect(TetrisGame.Pixel, TetrisGame.spriteBatch, background, Color.Red);

        }

        public int update()
        {
            int points = 0;
            for (int row = PLAYFIELD_ROWS - 1; row >= 0; row--)
            {
                if (CheckRowForSame(row, false))
                    break;
                if (CheckRowForSame(row, true))
                {
                    points++;
                    SetAllRowTo(row, false);
                    MakeGridCollapse(row - 1);
                    row++;//we collapsed grid onto this row so we need to check it again by cancelling the upcomming decrement
                }
            }
            return points;
        }

        private void MakeGridCollapse(int row)
        {
            for (int r = row; r >= 0; r--)
            {
                if (CheckRowForSame(r, false))
                    break;
                ShiftRowBy(r, C_DOWN);
                SetAllRowTo(r, false);
            }
        }

        private void ShiftRowBy(int row, int down)
        {
            int index;
            for (int i = 0; i < PLAYFIELD_COLS; i++)
            {
                index = (row * PLAYFIELD_COLS) + i;
                mCells[index + down] = mCells[index];
            }

        }

        private void SetAllRowTo(int row, bool b)
        {
            for (int i = 0; i < PLAYFIELD_COLS; i++)
            {
                mCells[(row * PLAYFIELD_COLS) + i] = b;
            }
        }

        private bool CheckRowForSame(int row, bool b)
        {
            for (int i = 0; i < PLAYFIELD_COLS; i++)
            {
                if (mCells[(row * PLAYFIELD_COLS) + i] != b)
                    return false;
            }
            return true;
        }
    }
}
