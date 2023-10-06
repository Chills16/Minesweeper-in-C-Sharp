using System;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace Minesweeper
{
    //defines the Customization class, which inherits from Form
    public partial class Customization : Form
    {
        //declare a timer to track game time
        private System.Windows.Forms.Timer gameTimer;

        //declare a variable to store the elapsed game time in seconds
        private int elapsedTime = 0;

        //declare a label to display the game timer on the form
        private Label lblTimer;

        //constructor for the Customization class
        public Customization(System.Windows.Forms.Timer timer)
        {
            //initialize everything within the form
            InitializeComponent();

            //assign the provided timer to the local gameTimer variable
            this.gameTimer = timer;

            //attach the GameTimerTick method to be called every tick of the timer
            gameTimer.Tick += GameTimerTick; 

            //initialize the Timer Label
            this.lblTimer = new Label();
            this.lblTimer.Width = 100;
            this.lblTimer.Height = 30;
            this.lblTimer.Top = 5;

            //center the label
            this.lblTimer.Left = (ClientSize.Width - lblTimer.Width) / 2;

            //display the time
            this.lblTimer.Text = $"Time: {elapsedTime}s";

            //add the label to form
            Controls.Add(lblTimer);
        }

        //method that gets executed every tick of the game timer
        private void GameTimerTick(object sender, EventArgs e)
        {
            //increment time
            elapsedTime++;

            //update the timer
            this.lblTimer.Text = $"Time: {elapsedTime}s";
        }

        //properties to store board dimensions and mine count
        public int BoardWidth { get; private set; }
        public int BoardHeight { get; private set; }
        public int MinesCount { get; private set; }

        //minimum and maximum allowed dimensions
        private const int MAX_BOARD_WIDTH = 30;
        private const int MIN_BOARD_WIDTH = 3;
        private const int MAX_BOARD_HEIGHT = 20;
        private const int MIN_BOARD_HEIGHT = 3;

        //method that gets executed when the "Save" button is clicked
        private void btnSave_Click(object sender, EventArgs e)
        {
            //temporary variables to hold parsed values from text boxes
            int boardWidthTemp, boardHeightTemp, minesCountTemp;

            //try to parse the text values to integers; if any fail, show an error message
            if (!int.TryParse(txtWidth.Text, out boardWidthTemp) ||
                !int.TryParse(txtHeight.Text, out boardHeightTemp) ||
                !int.TryParse(txtMinesCount.Text, out minesCountTemp))
            {
                MessageBox.Show("Please enter valid numbers", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //set the parsed values to the properties
            this.BoardWidth = boardWidthTemp;
            this.BoardHeight = boardHeightTemp;
            this.MinesCount = minesCountTemp;

            //validate that the board width is within the allowed range
            if (BoardWidth < MIN_BOARD_WIDTH || BoardWidth > MAX_BOARD_WIDTH)
            {
                MessageBox.Show($"Please enter a valid board width between {MIN_BOARD_WIDTH} and {MAX_BOARD_WIDTH}!", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //set focus back to the width text box
                txtWidth.Focus();
                return;
            }

            //validate that the board height is within the allowed range
            if (BoardHeight < MIN_BOARD_HEIGHT || BoardHeight > MAX_BOARD_HEIGHT)
            {
                MessageBox.Show($"Please enter a valid board height between {MIN_BOARD_HEIGHT} and {MAX_BOARD_HEIGHT}!", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //set focus back to the height text box
                txtHeight.Focus();
                return;
            }

            // Check mines count constraints
            if (MinesCount < 1)
            {
                MessageBox.Show("You should have at least one mine!", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMinesCount.Focus();
                return;
            }

            //condition to check the validity of number of mines
            if (MinesCount > BoardWidth * BoardHeight)
            {
                MessageBox.Show("Mines count can't be greater than total cells", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //set focus back to the mines count text box
                txtMinesCount.Focus();
                return;
            }

            //make the timer label visible
            this.lblTimer.Visible = true;

            //close the form and indicate that the dialog result was "OK"
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}



