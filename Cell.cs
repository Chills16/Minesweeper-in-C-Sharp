using System;
using System.Windows.Forms;

namespace Minesweeper
{
    public class Cell : Button
    {
        //properties of the tiles
        public int X { get; set; }
        public int Y { get; set; }
        public bool HasMine { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public int NeighboringMines { get; set; }

        public bool HasValue => Value > 0;
        public int Value { get; set; }

        //constructor for each tile
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            HasMine = false;
            IsRevealed = false;
            IsFlagged = false;
            NeighboringMines = 0;

            //original color is gray
            this.BackColor = Color.Gray; 
        }

        //count the flagged neighbors for the current cell
        public int CountFlaggedNeighbors(Cell[,] cells, int BOARD_WIDTH, int BOARD_HEIGHT)
        {
            int count = 0;

            //loop through each neighboring cell
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int newX = X + i;
                    int newY = Y + j;

                    //ensure that the neighboring cell is within the board
                    if (newX >= 0 && newX < BOARD_WIDTH && newY >= 0 && newY < BOARD_HEIGHT)
                    {
                        if (cells[newX, newY].IsFlagged)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        //count the hidden (not revealed) neighbors for the current cell
        public int CountHiddenNeighbors(Cell[,] cells, int BOARD_WIDTH, int BOARD_HEIGHT)
        {
            int count = 0;

            //loop through each neighboring cell
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int newX = X + i;
                    int newY = Y + j;

                    //ensure that the neighboring cell is within the board
                    if (newX >= 0 && newX < BOARD_WIDTH && newY >= 0 && newY < BOARD_HEIGHT)
                    {
                        if (!cells[newX, newY].IsRevealed)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        //reveal the current cell
        public void Reveal(Cell[,] cells, int BOARD_WIDTH, int BOARD_HEIGHT, Func<int, int, int> countNeighboringMines, Action<Cell> updateCellAppearance)
        {
            //if the cell is already revealed or flagged, we dont need to do anything
            if (IsRevealed || IsFlagged) 
                return;

            IsRevealed = true;

            if (HasMine)
            {
                //if the cell contains a mine, mark it by letter M
                this.Text = "M";
            }
            else
            {
                //change to white if empty tile
                this.BackColor = Color.White; 

                //if the cell doesnt contain a mine, update appearance based on neighboring mines
                Value = countNeighboringMines(X, Y);
                this.Text = HasValue ? Value.ToString() : "";
                updateCellAppearance(this);

                //if the cell has no neighboring mines, reveal its neighbors
                if (Value == 0)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            int newX = X + i;
                            int newY = Y + j;

                            //ensure that the neighboring cell is within the board and not the current cell itself
                            if (newX >= 0 && newX < BOARD_WIDTH && newY >= 0 && newY < BOARD_HEIGHT && !(i == 0 && j == 0))
                            {
                                cells[newX, newY].Reveal(cells, BOARD_WIDTH, BOARD_HEIGHT, countNeighboringMines, updateCellAppearance);
                            }
                        }
                    }
                }
            }
        }

        //toggle flag status for the current cell
        public void Flag()
        {
            IsFlagged = !IsFlagged;  //toggle the flag status
            this.Text = IsFlagged ? "F" : ""; //update the cell text based on flag status
        }
    }
}



