﻿namespace YourDeath
{
    partial class YourForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YourForm));
            this.RulesButton = new System.Windows.Forms.Label();
            this.Death_Timer = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // RulesButton
            // 
            this.RulesButton.AutoSize = true;
            this.RulesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RulesButton.ForeColor = System.Drawing.Color.Red;
            this.RulesButton.Location = new System.Drawing.Point(12, 399);
            this.RulesButton.Name = "RulesButton";
            this.RulesButton.Size = new System.Drawing.Size(119, 42);
            this.RulesButton.TabIndex = 0;
            this.RulesButton.Text = "Rules";
            this.RulesButton.Click += new System.EventHandler(this.RulesButton_Click);
            // 
            // Death_Timer
            // 
            this.Death_Timer.AutoSize = true;
            this.Death_Timer.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Death_Timer.ForeColor = System.Drawing.Color.Red;
            this.Death_Timer.Location = new System.Drawing.Point(669, 399);
            this.Death_Timer.Name = "Death_Timer";
            this.Death_Timer.Size = new System.Drawing.Size(117, 42);
            this.Death_Timer.TabIndex = 1;
            this.Death_Timer.Text = "00:00";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::YourDeath.Properties.Resources.NoSleeper;
            this.pictureBox1.Location = new System.Drawing.Point(-1, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(803, 397);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // YourForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Death_Timer);
            this.Controls.Add(this.RulesButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "YourForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "You are dead!";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.YourForm_FormClosing);
            this.Load += new System.EventHandler(this.YourForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label RulesButton;
        private System.Windows.Forms.Label Death_Timer;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}