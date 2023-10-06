//include necessary libraries for the program
using System;  //used for basic functionalities
using System.Collections.Generic;  //provides data structures like lists
using System.Drawing;  //helps with visual graphics and design
using System.Linq;  //adds functionalities for working with data structures
using System.Windows.Forms;  //provides tools for creating graphical user interfaces
using System.Threading;  //used for controlling the program's flow

namespace Minesweeper {
    //create the main window for the game.
    public partial class Form1 : Form {
        //define constants and default settings for the game
        //width of the game board
        public int BOARD_WIDTH = 10;

        //height of the game board
        public int BOARD_HEIGHT = 10;

        //total number of mines on the board
        private int MINE_COUNT = 10;

        //define a timer to measure game duration
        private System.Windows.Forms.Timer gameTimer = new System.Windows.Forms.Timer();

        //variables to measure the elapsed time
        private int elapsedTime = 0;

        //create a grid of cells for the game board
        public Cell[,] cells;

        //boolean for player and PCSolver
        private bool playerPlaying = true;

        //define a function to pause the program and update the display
        private void Sleep(int ms) {
            //pause the program for a given number of milliseconds
            Thread.Sleep(ms);

            //then update the display
            Refresh();
        }

        //constructor for the main game window
        public Form1() {
            //set up the window components
            InitializeComponent();

            //assign a function to be called every time the timer ticks
            gameTimer.Tick += GameTimerTick;

            //set the timer to tick every 1 second
            gameTimer.Interval = 1000;

            //configure the display label for the timer
            this.lblTimer = new Label();
            this.lblTimer.Width = 100;
            this.lblTimer.Height = 30;
            this.lblTimer.Top = 5;

            //center the label horizontally
            this.lblTimer.Left = (ClientSize.Width - lblTimer.Width) / 2;

            //display the initial elapsed time
            this.lblTimer.Text = $"Time: {elapsedTime}s";

            //add the label to the window
            Controls.Add(lblTimer);

            //initially, hide the timer label
            this.lblTimer.Visible = false;
        }

        //function called every time the timer ticks
        private void GameTimerTick(object sender, EventArgs e) {
            //increment the elapsed time
            elapsedTime++;

            //update the display with the new elapsed time
            this.lblTimer.Text = $"Time: {elapsedTime}s";
        }

        //set up the game board
        private void InitializeBoard() {
            //initialize the grid of cells
            cells = new Cell[BOARD_WIDTH, BOARD_HEIGHT];

            //determine the width of each cell
            int cellWidth = ClientSize.Width / BOARD_WIDTH;

            //determine the height of each cell
            int cellHeight = ClientSize.Height / BOARD_HEIGHT;

            //loop through and set up each cell
            for (int i = 0; i < BOARD_WIDTH; i++) {
                for (int j = 0; j < BOARD_HEIGHT; j++) {
                    //create a new cell
                    cells[i, j] = new Cell(i, j);

                    //set its width
                    cells[i, j].Width = cellWidth;

                    //set its height
                    cells[i, j].Height = cellHeight;

                    //set its horizontal position
                    cells[i, j].Left = i * cellWidth;

                    //set its vertical position
                    cells[i, j].Top = j * cellHeight;

                    //assign a function for when the cell is clicked
                    cells[i, j].MouseDown += CellMouseDown;

                    //add the cell to the game window
                    Controls.Add(cells[i, j]);
                }
            }
            
            //place mines randomly on the board
            PlaceMines();
        }

        //randomly distribute mines across the board
        private void PlaceMines() {
            //create a tool for generating random numbers
            Random rnd = new Random();

            //track the number of placed mines
            int minesPlaced = 0;

            //continue until all mines are placed
            while (minesPlaced < MINE_COUNT) {
                //generate a random horizontal position
                int x = rnd.Next(BOARD_WIDTH);

                //generate a random vertical position
                int y = rnd.Next(BOARD_HEIGHT);

                //check if the cell at this position doesnt already have a mine
                if (!cells[x, y].HasMine) {
                    //place a mine at this position
                    cells[x, y].HasMine = true;

                    //increment the count of placed mines
                    minesPlaced++;
                }
            }
        }

        //count how many neighboring cells of a given cell have mines
        private int CountNeighboringMines(int x, int y) {
            int count = 0;

            //loop through all neighboring cells
            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    int newX = x + i;
                    int newY = y + j;

                    //check if this neighboring cell is within the board and has a mine
                    if (newX >= 0 && newX < BOARD_WIDTH && newY >= 0 && newY < BOARD_HEIGHT && cells[newX, newY].HasMine) {
                        count++;  //increment the count of neighboring mines
                    }
                }
            }
            return count;  //return the total count
        }

        //reveals the specified cell at (x, y) and performs necessary checks/actions
        private void RevealCell(int x, int y) {

            //ensure the coords (x, y) are within the boards bounds
            if (x < 0 || x >= BOARD_WIDTH || y < 0 || y >= BOARD_HEIGHT) return;

            //if the cell is already revealed or flagged, exit the function
            if (cells[x, y].IsRevealed || cells[x, y].IsFlagged) return;

            //if the cell contains a mine, end the game as a loss
            if (cells[x, y].HasMine) {
                EndGame(false);
                return;
            }

            //reveal cells recursively
            cells[x, y].Reveal(cells, BOARD_WIDTH, BOARD_HEIGHT, CountNeighboringMines, UpdateCellAppearance);
        }

        //updates the appearance of the cell based on the state of the cell and its neighbors
        private void UpdateCellAppearance(Cell cell) {
            //count how many neighboring mines the cell has
            int neighboringMines = CountNeighboringMines(cell.X, cell.Y);

            //update the color of the number based on the count of neighboring mines
            switch (neighboringMines) {
                case 1: cell.ForeColor = Color.Blue; break;
                case 2: cell.ForeColor = Color.Green; break;
                case 3: cell.ForeColor = Color.Red; break;
                case 4: cell.ForeColor = Color.Purple; break;
                case 5: cell.ForeColor = Color.Maroon; break;
                case 6: cell.ForeColor = Color.Turquoise; break;
                case 7: cell.ForeColor = Color.Gray; break;
                case 8: cell.ForeColor = Color.Black; break;
                default: cell.ForeColor = Color.Black; break;
            }
        }

        //ends the game with either win or a loss message
        private void EndGame(bool won) {
            //disable all cells if the game is lost
            if (!won) {
                for (int i = 0; i < BOARD_WIDTH; i++) {
                    for (int j = 0; j < BOARD_HEIGHT; j++) {
                        cells[i, j].Enabled = false;

                        //mark all the cells with a mine by M
                        if (cells[i, j].HasMine) {
                            cells[i, j].Text = "M";
                        }
                    }
                }
            }

            //stop the timer and indicate the game result
            gameTimer.Stop();
            DialogResult value;

            //ensure the timer is shown
            if (playerPlaying) {
                value = MessageBox.Show(won ? $"You Won!    Time: {elapsedTime}s" : $"You Lost :c    Time: {elapsedTime}s");
            } else {
                //value = MessageBox.Show(won ? $"            I Won!" : $"           I Lost :c");
                value = MessageBox.Show(won ? $"I Won!    Time: {elapsedTime}s" : $"I Lost :c    Time: {elapsedTime}s");
            }

            //after player clicks OK, prepare for next game
            if (value == DialogResult.OK) {
                foreach (Cell cell in cells) {
                    cell.Dispose();
                }
                this.btnMenu.Visible = true;
                this.btnQuit.Visible = true;
                resetBoardSize();
            }
        }

        //function to check if player has won during the game
        private bool CheckWin() {
            //iterate through all the cells
            foreach (Cell cell in cells) {

                //if any cell that doesnt contain a mine is still unrevealed, the player hasnt won yet
                if (!cell.HasMine && !cell.IsRevealed) {
                    return false;
                }
            }
            //if the loop completes without returning false, the player has won
            return true;
        }

        //event handler for mouse actions on a cell
        private void CellMouseDown(object sender, MouseEventArgs e) {
            //cast the sender to a Cell object
            Cell cell = sender as Cell;

            //attempt to reveal cell
            if (e.Button == MouseButtons.Left && !cell.IsFlagged) {
                //if the cell has a mine, mark it and end the game with a loss
                if (cell.HasMine) {
                    cell.Text = "M";
                    EndGame(false);
                }
                //otherwise, attempt to reveal the cell
                else {
                    RevealCell(cell.X, cell.Y);
                }
            }

            //handle right mouse button press (toggle cell flagging)
            else if (e.Button == MouseButtons.Right) {
                //if cell isnt flagged, flag it
                if (!cell.IsFlagged) {
                    cell.Text = "F";
                    cell.IsFlagged = true;
                }
                //if its flagged, remove the flag
                else {
                    cell.Text = "";
                    cell.IsFlagged = false;
                }

                //check if the action resulted in winning
                if (CheckWin()) {
                    EndGame(true);
                }
            }
        }

        //method to return neighboring cells of a given cell position (x, y)
        private List<Cell> GetNeighboringCells(int x, int y) {
            var neighbors = new List<Cell>();

            //loop through potential neighboring positions
            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    int newX = x + i;
                    int newY = y + j;

                    //check if the neighboring position is valid and not the original cell itself
                    if (newX >= 0 && newX < BOARD_WIDTH && newY >= 0 && newY < BOARD_HEIGHT && !(i == 0 && j == 0)) {
                        neighbors.Add(cells[newX, newY]);
                    }
                }
            }

            return neighbors;
        }

        //BFS method to reveal connected cells from a given starting cell
        private void BFS(Cell startCell) {
            //queue to maintain cells to be processed
            Queue<Cell> queue = new Queue<Cell>();

            //hashset to keep track of cells already visited or processed
            HashSet<Cell> visited = new HashSet<Cell>();

            //begin with the provided start cell
            queue.Enqueue(startCell);
            visited.Add(startCell);

            //continue processing cells until there are none left in the queue
            while (queue.Count > 0) {
                //retrieve the next cell from the queue
                Cell current = queue.Dequeue();

                //check all neighboring cells of the current cell
                foreach (Cell neighbor in GetNeighboringCells(current.X, current.Y)) {
                    //if the neighboring cell is unvisited and neither flagged nor revealed
                    if (!visited.Contains(neighbor) && !neighbor.IsFlagged && !neighbor.IsRevealed) {
                        //mark the neighbor as visited
                        visited.Add(neighbor);

                        //reveal the neighboring cell
                        RevealCell(neighbor.X, neighbor.Y);

                        //pause for 1 second to reveal action
                        Sleep(1000);

                        //if the neighboring cell has a value of 0 (indicating no mines around), add it to the queue for further processing
                        if (neighbor.Value == 0)
                            queue.Enqueue(neighbor);
                    }
                }
            }
        }

        //method to automatically solve the Minesweeper game using logical steps and random choices
        private void SolveByPC() {
            //initialize the board for a new game
            InitializeBoard();
            
            //find a safe starting position and reveal it
            int startX = 0;
            int startY = 0;
            while (cells[startX, startY].HasMine) {
                startX = (startX + 1) % BOARD_WIDTH;
                if (startX == 0)
                    startY = (startY + 1) % BOARD_HEIGHT;
            }

            //reveal initial/starting cell
            RevealCell(startX, startY);

            //if after cell is revealed, and it is empty, run bfs
            if (cells[startX, startY].Value == 0)
                BFS(cells[startX, startY]);

            bool gameSolved = false;
            bool gameLost = false;

            //continue making moves until the game is solved or lost
            while (!gameSolved && !gameLost) {
                //track if a move has been made during a loop
                bool changed = false;
                
                //loop through all cells on the board
                for (int x = 0; x < BOARD_WIDTH; x++) {
                    for (int y = 0; y < BOARD_HEIGHT; y++) {
                        //look for revealed cells with numbers (indicating surrounding mines)
                        if (cells[x, y].IsRevealed && (cells[x, y].Value > 0)) {
                            List<Cell> neighboringCells = GetNeighboringCells(x, y);
                            var coveredNeighbors = neighboringCells.Where(cell => !cell.IsRevealed && !cell.IsFlagged).ToList();
                            var flaggedNeighbors = neighboringCells.Where(cell => cell.IsFlagged).ToList();

                            //if the sum of covered and flagged neighbors equals the cell's value, flag all covered neighbors
                            if (coveredNeighbors.Count + flaggedNeighbors.Count == cells[x, y].Value && flaggedNeighbors.Count < MINE_COUNT) {
                                foreach (var cell in coveredNeighbors) {
                                    cell.Text = "F";
                                    cell.IsFlagged = true;
                                    changed = true;
                                    //add a 1 second delay between steps to show the process
                                    Sleep(1000);
                                }
                            }

                            //if the number of flagged neighbors matches the cell's value, reveal all covered neighbors
                            else if (flaggedNeighbors.Count == cells[x, y].Value) {
                                foreach (var cell in coveredNeighbors) {
                                    //if a mine is  revealed, the game is lost
                                    if (cell.HasMine) {
                                        //add a small delay to indicate a loss
                                        Sleep(1500);
                                        gameLost = true;
                                        break;
                                    }

                                    //reveal cells, add 1s delay between steps
                                    RevealCell(cell.X, cell.Y);

                                    //if revealed cell is 0/empty, execute bfs
                                    if (cell.Value == 0) {
                                        BFS(cell);
                                    } else {
                                        //if not, pause to reveal action
                                        Sleep(1000);
                                    }

                                    //a move was made
                                    changed = true;
                                }
                            }
                        }

                        //if a move was made, restart the loop
                        if (changed) {
                            
                            //update the timer
                            GameTimerTick(null, EventArgs.Empty);
                            break;
                        }
                    }

                    //if a move was made or the game ended, restart the loop
                    if (changed || gameLost)
                        break;
                }

                //if no logical move was found, choose a random unrevealed cell and reveal it
                if (!changed) {
                    var unrevealedCells = cells.Cast<Cell>().Where(cell => !cell.IsRevealed && !cell.IsFlagged).ToList();
                    if (unrevealedCells.Count > 0) {
                        var randomCell = unrevealedCells[new Random().Next(unrevealedCells.Count)];

                        //if a mine is accidentally revealed, the game is lost
                        if (randomCell.HasMine) {
                            gameLost = true;
                        } else {
                            //if not, reveal the chosen cell
                            RevealCell(randomCell.X, randomCell.Y);

                            //run the bfs algorithm when we find an empty cell
                            if (randomCell.Value == 0)
                                BFS(randomCell);

                            //a move was made
                            changed = true;
                        }
                    }
                }

                //after each move, check if the game has been solved
                gameSolved = CheckWin();
            }

            //if all non-mine cells are revealed, the game is won
            if (gameSolved)

                //considered "win"
                EndGame(true);

            else if (gameLost)

                //considered "loss"
                EndGame(false);

            else

                //method to handle "unsolvable" scenario
                CannotSolveGame();
        }

        //makes sure to reset the dimensions and number of mines 
        private void resetBoardSize() {
            BOARD_WIDTH = 10;

            BOARD_HEIGHT = 10;

            MINE_COUNT = 10;
        }

        //a method to handle the scenario where the game cannot be solved
        private void CannotSolveGame() {
            MessageBox.Show("Can't solve. The solver exhausted all known strategies without fully revealing the board.");
        }

        //the event for the "Solve by PC" button, which occurs when the user clicks on the button
        private void btnSolveByPC_Click(object sender, EventArgs e) {
            //this section specifies which buttons/labels are visible/invisible after user clicks on the button
            this.btnStartGame.Visible = false;
            this.btnCustomizeGrid.Visible = false;
            this.btnSolveByPC.Visible = false;
            this.btnQuit.Visible = false;
            this.btnMenu.Visible = false;
            this.lblWelcome.Visible = false;
            this.lblTimer.Visible = true;

            //PCSolver is playing
            playerPlaying = false;

            //stop the timer, to properly prepare it
            gameTimer.Stop();

            //reset the time
            elapsedTime = 0;

            //start the timer
            gameTimer.Start();

            //update the timer
            GameTimerTick(null, EventArgs.Empty);

            //run the function
            SolveByPC();
        }

        private void btnStartGame_Click(object sender, EventArgs e) {
            //this section specifies which buttons/labels are visible/invisible after user clicks on the button
            this.btnStartGame.Visible = false;
            this.btnCustomizeGrid.Visible = false;
            this.btnSolveByPC.Visible = false;
            this.btnQuit.Visible = false;
            this.btnMenu.Visible = false;
            this.lblWelcome.Visible = false;
            this.lblTimer.Visible = true;

            //player is playing
            playerPlaying = true;

            //stop the timer, to properly prepare it
            gameTimer.Stop();

            //reset the time
            elapsedTime = 0;

            //start the timer
            gameTimer.Start();

            //update the timer
            GameTimerTick(null, EventArgs.Empty);

            //initialize and show the game board
            InitializeBoard();
        }

        private void btnQuit_Click(object sender, EventArgs e) {
            //close the game when the "close" button is clicked
            this.Close();
        }

        private void btnMenu_Click(object sender, EventArgs e) {
            //toggle visibility of the buttons
            this.btnStartGame.Visible = !this.btnStartGame.Visible;
            this.btnCustomizeGrid.Visible = !this.btnCustomizeGrid.Visible;
            this.btnSolveByPC.Visible = !this.btnSolveByPC.Visible;
            this.lblWelcome.Visible = false;

            //hide the menu button itself after it is clicked
            this.btnMenu.Visible = false;
        }

        private void btnCustomizeGrid_Click(object sender, EventArgs e) {
            //this section specifies which buttons/labels are visible/invisible after user clicks on the button
            this.lblWelcome.Visible = false;
            this.btnStartGame.Visible = false;
            this.btnCustomizeGrid.Visible = false;
            this.btnSolveByPC.Visible = false;
            this.btnQuit.Visible = false;
            this.btnMenu.Visible = false;
            this.lblTimer.Visible = false;

            //specify the use of the timer
            using (var customizationForm = new Customization(gameTimer)) {
                //when "OK" is clicked
                if (customizationForm.ShowDialog() == DialogResult.OK) {
                    //the save button was clicked in customizationForm
                    this.lblTimer.Visible = true;

                    //start the timer
                    gameTimer.Start();

                    //set the dimensions
                    BOARD_WIDTH = customizationForm.BoardWidth;
                    BOARD_HEIGHT = customizationForm.BoardHeight;
                    MINE_COUNT = customizationForm.MinesCount;

                    //redefine the cells array based on the new dimensions
                    cells = new Cell[BOARD_WIDTH, BOARD_HEIGHT];

                    //start the game after customization
                    //stop the timer
                    gameTimer.Stop();

                    //reset the time
                    elapsedTime = 0;

                    //start the timer
                    gameTimer.Start();

                    //update the timer
                    GameTimerTick(null, EventArgs.Empty);

                    //initialize and show the game board
                    InitializeBoard();
                }
            }
        }

        //specify the events after clicking on the timer, essentially nonexistent in this scenario
        private void lblTimer_Click(object sender, EventArgs e) {
            this.lblWelcome.Visible = false;
        }
    }
}


