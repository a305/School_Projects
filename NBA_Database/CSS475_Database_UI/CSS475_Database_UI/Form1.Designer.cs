namespace CSS475_Database_UI
{
    partial class HomePage
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.table1Combo = new System.Windows.Forms.ComboBox();
            this.attribute1_1 = new System.Windows.Forms.ComboBox();
            this.attribute1_2 = new System.Windows.Forms.ComboBox();
            this.execute = new System.Windows.Forms.Button();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.moreQueries = new System.Windows.Forms.Button();
            this.otherQueries = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(273, 42);
            this.label1.TabIndex = 1;
            this.label1.Text = "Output Window";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(34, 637);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 37);
            this.label2.TabIndex = 2;
            this.label2.Text = "Table";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(274, 637);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 37);
            this.label4.TabIndex = 4;
            this.label4.Text = "Attribute 1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(520, 637);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(164, 37);
            this.label5.TabIndex = 5;
            this.label5.Text = "Attribute 2";
            // 
            // table1Combo
            // 
            this.table1Combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.table1Combo.Location = new System.Drawing.Point(41, 677);
            this.table1Combo.Name = "table1Combo";
            this.table1Combo.Size = new System.Drawing.Size(175, 33);
            this.table1Combo.TabIndex = 8;
            this.table1Combo.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // attribute1_1
            // 
            this.attribute1_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.attribute1_1.Location = new System.Drawing.Point(281, 677);
            this.attribute1_1.Name = "attribute1_1";
            this.attribute1_1.Size = new System.Drawing.Size(175, 33);
            this.attribute1_1.TabIndex = 9;
            this.attribute1_1.SelectedIndexChanged += new System.EventHandler(this.attribue1_1_SelectedIndexChanged);
            // 
            // attribute1_2
            // 
            this.attribute1_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.attribute1_2.Location = new System.Drawing.Point(527, 677);
            this.attribute1_2.Name = "attribute1_2";
            this.attribute1_2.Size = new System.Drawing.Size(175, 33);
            this.attribute1_2.TabIndex = 10;
            this.attribute1_2.SelectedIndexChanged += new System.EventHandler(this.attribute1_2_SelectedIndexChanged);
            // 
            // execute
            // 
            this.execute.Location = new System.Drawing.Point(41, 763);
            this.execute.Name = "execute";
            this.execute.Size = new System.Drawing.Size(175, 41);
            this.execute.TabIndex = 14;
            this.execute.Text = "Execute";
            this.execute.UseVisualStyleBackColor = true;
            this.execute.Click += new System.EventHandler(this.execute_Click);
            // 
            // dataGrid
            // 
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToDeleteRows = false;
            this.dataGrid.AllowUserToResizeRows = false;
            this.dataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Location = new System.Drawing.Point(35, 73);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            this.dataGrid.RowTemplate.Height = 33;
            this.dataGrid.Size = new System.Drawing.Size(1439, 531);
            this.dataGrid.TabIndex = 16;
            // 
            // moreQueries
            // 
            this.moreQueries.Location = new System.Drawing.Point(846, 637);
            this.moreQueries.Name = "moreQueries";
            this.moreQueries.Size = new System.Drawing.Size(236, 92);
            this.moreQueries.TabIndex = 17;
            this.moreQueries.Text = "Complex Sponsor Queries";
            this.moreQueries.UseVisualStyleBackColor = true;
            this.moreQueries.Click += new System.EventHandler(this.moreQueries_Click);
            // 
            // otherQueries
            // 
            this.otherQueries.Location = new System.Drawing.Point(1171, 637);
            this.otherQueries.Name = "otherQueries";
            this.otherQueries.Size = new System.Drawing.Size(236, 92);
            this.otherQueries.TabIndex = 18;
            this.otherQueries.Text = "Misc. Complex Queries";
            this.otherQueries.UseVisualStyleBackColor = true;
            this.otherQueries.Click += new System.EventHandler(this.otherQueries_Click);
            // 
            // HomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1512, 893);
            this.Controls.Add(this.otherQueries);
            this.Controls.Add(this.moreQueries);
            this.Controls.Add(this.dataGrid);
            this.Controls.Add(this.execute);
            this.Controls.Add(this.attribute1_2);
            this.Controls.Add(this.attribute1_1);
            this.Controls.Add(this.table1Combo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "HomePage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HomePage";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox table1Combo;
        private System.Windows.Forms.ComboBox attribute1_1;
        private System.Windows.Forms.ComboBox attribute1_2;
        private System.Windows.Forms.Button execute;
        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.Button moreQueries;
        private System.Windows.Forms.Button otherQueries;
    }
}

