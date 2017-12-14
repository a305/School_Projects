namespace CSS475_Database_UI
{
    partial class MiscQueries
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
            this.listByFGPercent = new System.Windows.Forms.Button();
            this.divisionWinsPercent = new System.Windows.Forms.Button();
            this.coachesByWins = new System.Windows.Forms.Button();
            this.stadiumByTeam = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listByFGPercent
            // 
            this.listByFGPercent.Location = new System.Drawing.Point(96, 58);
            this.listByFGPercent.Name = "listByFGPercent";
            this.listByFGPercent.Size = new System.Drawing.Size(273, 115);
            this.listByFGPercent.TabIndex = 0;
            this.listByFGPercent.Text = "List by FG Percentage";
            this.listByFGPercent.UseVisualStyleBackColor = true;
            this.listByFGPercent.Click += new System.EventHandler(this.listByFGPercent_Click);
            // 
            // divisionWinsPercent
            // 
            this.divisionWinsPercent.Location = new System.Drawing.Point(96, 236);
            this.divisionWinsPercent.Name = "divisionWinsPercent";
            this.divisionWinsPercent.Size = new System.Drawing.Size(273, 115);
            this.divisionWinsPercent.TabIndex = 1;
            this.divisionWinsPercent.Text = "List by Division Wins Percentage";
            this.divisionWinsPercent.UseVisualStyleBackColor = true;
            this.divisionWinsPercent.Click += new System.EventHandler(this.divisionWinsPercent_Click);
            // 
            // coachesByWins
            // 
            this.coachesByWins.Location = new System.Drawing.Point(96, 420);
            this.coachesByWins.Name = "coachesByWins";
            this.coachesByWins.Size = new System.Drawing.Size(273, 115);
            this.coachesByWins.TabIndex = 2;
            this.coachesByWins.Text = "List Coaches by wins";
            this.coachesByWins.UseVisualStyleBackColor = true;
            this.coachesByWins.Click += new System.EventHandler(this.coachesByWins_Click);
            // 
            // stadiumByTeam
            // 
            this.stadiumByTeam.Location = new System.Drawing.Point(96, 602);
            this.stadiumByTeam.Name = "stadiumByTeam";
            this.stadiumByTeam.Size = new System.Drawing.Size(273, 115);
            this.stadiumByTeam.TabIndex = 3;
            this.stadiumByTeam.Text = "List Stadiums by Teams";
            this.stadiumByTeam.UseVisualStyleBackColor = true;
            this.stadiumByTeam.Click += new System.EventHandler(this.stadiumByTeam_Click);
            // 
            // MiscQueries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(459, 766);
            this.Controls.Add(this.stadiumByTeam);
            this.Controls.Add(this.coachesByWins);
            this.Controls.Add(this.divisionWinsPercent);
            this.Controls.Add(this.listByFGPercent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MiscQueries";
            this.Text = "MiscQueries";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button listByFGPercent;
        private System.Windows.Forms.Button divisionWinsPercent;
        private System.Windows.Forms.Button coachesByWins;
        private System.Windows.Forms.Button stadiumByTeam;
    }
}