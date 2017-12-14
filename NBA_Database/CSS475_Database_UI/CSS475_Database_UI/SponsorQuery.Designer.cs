namespace NBA_Sponsor
{
   partial class SponsorQuery
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
            this.btnTeamGetSpContract = new System.Windows.Forms.Button();
            this.lstBoxOutput = new System.Windows.Forms.ListBox();
            this.txtTeamName = new System.Windows.Forms.TextBox();
            this.lblTeamName = new System.Windows.Forms.Label();
            this.btnGetTotalSponsorWAboveAvgFG = new System.Windows.Forms.Button();
            this.btnTotalPerIndustry = new System.Windows.Forms.Button();
            this.btnTopTenSponsored = new System.Windows.Forms.Button();
            this.lblGeneralQuery = new System.Windows.Forms.Label();
            this.lblTeamQuery = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnTeamGetSpContract
            // 
            this.btnTeamGetSpContract.Location = new System.Drawing.Point(18, 188);
            this.btnTeamGetSpContract.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTeamGetSpContract.Name = "btnTeamGetSpContract";
            this.btnTeamGetSpContract.Size = new System.Drawing.Size(460, 78);
            this.btnTeamGetSpContract.TabIndex = 0;
            this.btnTeamGetSpContract.Text = "Get all player sponsor contracts\r\n(including expired contracts)";
            this.btnTeamGetSpContract.UseVisualStyleBackColor = true;
            this.btnTeamGetSpContract.Click += new System.EventHandler(this.btnTeamGetSpContract_Click);
            // 
            // lstBoxOutput
            // 
            this.lstBoxOutput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBoxOutput.FormattingEnabled = true;
            this.lstBoxOutput.ItemHeight = 28;
            this.lstBoxOutput.Location = new System.Drawing.Point(516, 19);
            this.lstBoxOutput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstBoxOutput.Name = "lstBoxOutput";
            this.lstBoxOutput.Size = new System.Drawing.Size(1436, 844);
            this.lstBoxOutput.TabIndex = 1;
            // 
            // txtTeamName
            // 
            this.txtTeamName.Location = new System.Drawing.Point(154, 117);
            this.txtTeamName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTeamName.Name = "txtTeamName";
            this.txtTeamName.Size = new System.Drawing.Size(322, 31);
            this.txtTeamName.TabIndex = 2;
            // 
            // lblTeamName
            // 
            this.lblTeamName.AutoSize = true;
            this.lblTeamName.Location = new System.Drawing.Point(18, 122);
            this.lblTeamName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTeamName.Name = "lblTeamName";
            this.lblTeamName.Size = new System.Drawing.Size(128, 25);
            this.lblTeamName.TabIndex = 3;
            this.lblTeamName.Text = "Team Name";
            // 
            // btnGetTotalSponsorWAboveAvgFG
            // 
            this.btnGetTotalSponsorWAboveAvgFG.Location = new System.Drawing.Point(18, 286);
            this.btnGetTotalSponsorWAboveAvgFG.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGetTotalSponsorWAboveAvgFG.Name = "btnGetTotalSponsorWAboveAvgFG";
            this.btnGetTotalSponsorWAboveAvgFG.Size = new System.Drawing.Size(460, 78);
            this.btnGetTotalSponsorWAboveAvgFG.TabIndex = 4;
            this.btnGetTotalSponsorWAboveAvgFG.Text = "Get total sponsorship amount for players \r\nwith above average Field Goal %";
            this.btnGetTotalSponsorWAboveAvgFG.UseVisualStyleBackColor = true;
            this.btnGetTotalSponsorWAboveAvgFG.Click += new System.EventHandler(this.btnGetTotalSponsorWAboveAvgFG_Click);
            // 
            // btnTotalPerIndustry
            // 
            this.btnTotalPerIndustry.Location = new System.Drawing.Point(22, 528);
            this.btnTotalPerIndustry.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTotalPerIndustry.Name = "btnTotalPerIndustry";
            this.btnTotalPerIndustry.Size = new System.Drawing.Size(460, 78);
            this.btnTotalPerIndustry.TabIndex = 5;
            this.btnTotalPerIndustry.Text = "Get the total financial sponsorship amount\r\nper business industry";
            this.btnTotalPerIndustry.UseVisualStyleBackColor = true;
            this.btnTotalPerIndustry.Click += new System.EventHandler(this.btnTotalPerIndustry_Click);
            // 
            // btnTopTenSponsored
            // 
            this.btnTopTenSponsored.Location = new System.Drawing.Point(18, 633);
            this.btnTopTenSponsored.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTopTenSponsored.Name = "btnTopTenSponsored";
            this.btnTopTenSponsored.Size = new System.Drawing.Size(460, 78);
            this.btnTopTenSponsored.TabIndex = 6;
            this.btnTopTenSponsored.Text = "Get the top 10 highest sponsored players\r\nand their teams";
            this.btnTopTenSponsored.UseVisualStyleBackColor = true;
            this.btnTopTenSponsored.Click += new System.EventHandler(this.btnTopTenSponsored_Click);
            // 
            // lblGeneralQuery
            // 
            this.lblGeneralQuery.AutoSize = true;
            this.lblGeneralQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGeneralQuery.Location = new System.Drawing.Point(100, 473);
            this.lblGeneralQuery.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGeneralQuery.Name = "lblGeneralQuery";
            this.lblGeneralQuery.Size = new System.Drawing.Size(320, 31);
            this.lblGeneralQuery.TabIndex = 7;
            this.lblGeneralQuery.Text = "General Sponsor Query";
            // 
            // lblTeamQuery
            // 
            this.lblTeamQuery.AutoSize = true;
            this.lblTeamQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTeamQuery.Location = new System.Drawing.Point(116, 66);
            this.lblTeamQuery.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTeamQuery.Name = "lblTeamQuery";
            this.lblTeamQuery.Size = new System.Drawing.Size(290, 31);
            this.lblTeamQuery.TabIndex = 8;
            this.lblTeamQuery.Text = "Team Sponsor Query";
            // 
            // SponsorQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1972, 884);
            this.Controls.Add(this.lblTeamQuery);
            this.Controls.Add(this.lblGeneralQuery);
            this.Controls.Add(this.btnTopTenSponsored);
            this.Controls.Add(this.btnTotalPerIndustry);
            this.Controls.Add(this.btnGetTotalSponsorWAboveAvgFG);
            this.Controls.Add(this.lblTeamName);
            this.Controls.Add(this.txtTeamName);
            this.Controls.Add(this.lstBoxOutput);
            this.Controls.Add(this.btnTeamGetSpContract);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "SponsorQuery";
            this.Text = "Sponsor_Query";
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button btnTeamGetSpContract;
      private System.Windows.Forms.ListBox lstBoxOutput;
      private System.Windows.Forms.TextBox txtTeamName;
      private System.Windows.Forms.Label lblTeamName;
      private System.Windows.Forms.Button btnGetTotalSponsorWAboveAvgFG;
      private System.Windows.Forms.Button btnTotalPerIndustry;
      private System.Windows.Forms.Button btnTopTenSponsored;
      private System.Windows.Forms.Label lblGeneralQuery;
      private System.Windows.Forms.Label lblTeamQuery;
   }
}

