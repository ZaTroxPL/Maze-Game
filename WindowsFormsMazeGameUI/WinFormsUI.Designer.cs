namespace Maze_Game
{
    partial class WinFormsUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.North = new System.Windows.Forms.Button();
            this.East = new System.Windows.Forms.Button();
            this.South = new System.Windows.Forms.Button();
            this.West = new System.Windows.Forms.Button();
            this.userInput = new System.Windows.Forms.TextBox();
            this.PlayerInfoDisplay = new System.Windows.Forms.FlowLayoutPanel();
            this.PlayerName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.PlayerInfoDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // North
            // 
            this.North.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.North.Location = new System.Drawing.Point(824, 407);
            this.North.Name = "North";
            this.North.Size = new System.Drawing.Size(83, 53);
            this.North.TabIndex = 0;
            this.North.Text = "North";
            this.North.UseVisualStyleBackColor = true;
            this.North.Click += new System.EventHandler(this.GoNorth);
            // 
            // East
            // 
            this.East.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.East.Location = new System.Drawing.Point(907, 466);
            this.East.Name = "East";
            this.East.Size = new System.Drawing.Size(83, 53);
            this.East.TabIndex = 1;
            this.East.Text = "East";
            this.East.UseVisualStyleBackColor = true;
            this.East.Click += new System.EventHandler(this.GoEast);
            // 
            // South
            // 
            this.South.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.South.Location = new System.Drawing.Point(824, 525);
            this.South.Name = "South";
            this.South.Size = new System.Drawing.Size(83, 53);
            this.South.TabIndex = 2;
            this.South.Text = "South";
            this.South.UseVisualStyleBackColor = true;
            this.South.Click += new System.EventHandler(this.GoSouth);
            // 
            // West
            // 
            this.West.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.West.Location = new System.Drawing.Point(741, 466);
            this.West.Name = "West";
            this.West.Size = new System.Drawing.Size(83, 53);
            this.West.TabIndex = 3;
            this.West.Text = "West";
            this.West.UseVisualStyleBackColor = true;
            this.West.Click += new System.EventHandler(this.GoWest);
            // 
            // userInput
            // 
            this.userInput.Font = new System.Drawing.Font("Calibri", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userInput.Location = new System.Drawing.Point(25, 466);
            this.userInput.Name = "userInput";
            this.userInput.Size = new System.Drawing.Size(687, 53);
            this.userInput.TabIndex = 4;
            // 
            // PlayerInfoDisplay
            // 
            this.PlayerInfoDisplay.BackColor = System.Drawing.SystemColors.ControlText;
            this.PlayerInfoDisplay.Controls.Add(this.PlayerName);
            this.PlayerInfoDisplay.Location = new System.Drawing.Point(741, 21);
            this.PlayerInfoDisplay.Name = "PlayerInfoDisplay";
            this.PlayerInfoDisplay.Size = new System.Drawing.Size(249, 183);
            this.PlayerInfoDisplay.TabIndex = 6;
            // 
            // PlayerName
            // 
            this.PlayerName.AutoSize = true;
            this.PlayerName.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayerName.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.PlayerName.Location = new System.Drawing.Point(10, 10);
            this.PlayerName.Margin = new System.Windows.Forms.Padding(10, 10, 0, 0);
            this.PlayerName.Name = "PlayerName";
            this.PlayerName.Size = new System.Drawing.Size(110, 19);
            this.PlayerName.TabIndex = 0;
            this.PlayerName.Text = "Player\'s Name: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(116, 210);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(529, 82);
            this.label1.TabIndex = 7;
            this.label1.Text = "Game Text Display";
            // 
            // WinFormsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 601);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PlayerInfoDisplay);
            this.Controls.Add(this.userInput);
            this.Controls.Add(this.West);
            this.Controls.Add(this.South);
            this.Controls.Add(this.East);
            this.Controls.Add(this.North);
            this.Name = "WinFormsUI";
            this.Text = "WinFormsUI";
            this.PlayerInfoDisplay.ResumeLayout(false);
            this.PlayerInfoDisplay.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button North;
        private System.Windows.Forms.Button East;
        private System.Windows.Forms.Button South;
        private System.Windows.Forms.Button West;
        private System.Windows.Forms.TextBox userInput;
        private System.Windows.Forms.FlowLayoutPanel PlayerInfoDisplay;
        private System.Windows.Forms.Label PlayerName;
        private System.Windows.Forms.Label label1;
    }
}