using System;
using System.Windows.Forms;
using System.Drawing;

namespace Minesweeper {
    partial class Form1 {

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        //initialize UI components of the form
        private void InitializeComponent() {
            lblWelcome = new Label();
            btnMenu = new Button();
            btnQuit = new Button();
            btnStartGame = new Button();
            btnCustomizeGrid = new Button();
            btnSolveByPC = new Button();
            lblTimer = new Label();
            SuspendLayout();
            // 
            // lblWelcome
            // 
            lblWelcome.Location = new Point(420, 20);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(300, 40);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "Welcome to the Minesweeper game";
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnMenu
            // 
            btnMenu.Location = new Point(460, 300);
            btnMenu.Name = "btnMenu";
            btnMenu.Size = new Size(200, 40);
            btnMenu.TabIndex = 1;
            btnMenu.Text = "Menu";
            btnMenu.Click += btnMenu_Click;
            // 
            // btnQuit
            // 
            btnQuit.Location = new Point(460, 350);
            btnQuit.Name = "btnQuit";
            btnQuit.Size = new Size(200, 40);
            btnQuit.TabIndex = 2;
            btnQuit.Text = "Quit";
            btnQuit.Click += btnQuit_Click;
            // 
            // btnStartGame
            // 
            btnStartGame.Location = new Point(460, 200);
            btnStartGame.Name = "btnStartGame";
            btnStartGame.Size = new Size(200, 40);
            btnStartGame.TabIndex = 3;
            btnStartGame.Text = "Start Game";
            btnStartGame.Visible = false;
            btnStartGame.Click += btnStartGame_Click;
            // 
            // btnCustomizeGrid
            // 
            btnCustomizeGrid.Location = new Point(460, 250);
            btnCustomizeGrid.Name = "btnCustomizeGrid";
            btnCustomizeGrid.Size = new Size(200, 40);
            btnCustomizeGrid.TabIndex = 4;
            btnCustomizeGrid.Text = "Customize Grid";
            btnCustomizeGrid.Visible = false;
            btnCustomizeGrid.Click += btnCustomizeGrid_Click;
            // 
            // btnSolveByPC
            // 
            btnSolveByPC.Location = new Point(460, 300);
            btnSolveByPC.Name = "btnSolveByPC";
            btnSolveByPC.Size = new Size(200, 40);
            btnSolveByPC.TabIndex = 5;
            btnSolveByPC.Text = "Solve by PC";
            btnSolveByPC.Visible = false;
            btnSolveByPC.Click += btnSolveByPC_Click;
            // 
            // lblTimer
            // 
            lblTimer.AutoSize = true;
            lblTimer.Location = new Point(536, 19);
            lblTimer.Name = "lblTimer";
            lblTimer.Size = new Size(47, 20);
            lblTimer.TabIndex = 6;
            lblTimer.Text = "Timer";
            lblTimer.TextAlign = ContentAlignment.MiddleCenter;
            lblTimer.Visible = false;
            lblTimer.Click += lblTimer_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1121, 683);
            Controls.Add(lblTimer);
            Controls.Add(lblWelcome);
            Controls.Add(btnMenu);
            Controls.Add(btnQuit);
            Controls.Add(btnStartGame);
            Controls.Add(btnCustomizeGrid);
            Controls.Add(btnSolveByPC);
            Name = "Form1";
            Text = "Minesweeper";
            ResumeLayout(false);
            PerformLayout();
        }

        //declare UI components
        private Label lblWelcome;
        private Button btnMenu;
        private Button btnQuit;
        private Button btnStartGame;
        private Button btnCustomizeGrid;
        private Button btnSolveByPC;
        private Label lblTimer;
    }
    partial class Customization {
        //UI components for customization of the game
        private TextBox txtWidth;
        private TextBox txtHeight;
        private TextBox txtMinesCount;
        private Button btnSave;
        private Label labelWidth;
        private Label labelHeight;
        private Label labelMinesCount;

        private void InitializeComponent() {
            //specify the locations and dimensions of the input fields for customization
            this.txtWidth = new TextBox() { Location = new Point(90, 50), Size = new Size(100, 25) };
            this.txtHeight = new TextBox() { Location = new Point(90, 80), Size = new Size(100, 25) };
            this.txtMinesCount = new TextBox() { Location = new Point(90, 110), Size = new Size(100, 25) };
            this.btnSave = new Button() { Location = new Point(90, 150), Text = "Save", Size = new Size(100, 25) };
            this.labelWidth = new Label() { Location = new Point(20, 50), Text = "Width", Size = new Size(100, 25) };
            this.labelHeight = new Label() { Location = new Point(20, 80), Text = "Height", Size = new Size(100, 25) };
            this.labelMinesCount = new Label() { Location = new Point(20, 110), Text = "Mines", Size = new Size(100, 25) };

            //event handler for save button
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            //add controls to the form
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.txtMinesCount);
            this.Controls.Add(this.btnSave);

            //TU SOM TOTOK PRIDAL
            this.Controls.Add(this.labelWidth);
            this.Controls.Add(this.labelHeight);
            this.Controls.Add(this.labelMinesCount);
        }
    }
}