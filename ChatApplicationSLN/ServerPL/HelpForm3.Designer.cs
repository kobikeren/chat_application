namespace ServerPL
{
    partial class HelpForm3
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnShowRecords = new System.Windows.Forms.Button();
            this.btnFindByWord = new System.Windows.Forms.Button();
            this.btnFindByDate = new System.Windows.Forms.Button();
            this.txtWord = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDay = new System.Windows.Forms.TextBox();
            this.txtMonth = new System.Windows.Forms.TextBox();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(33, 138);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(647, 309);
            this.dataGridView1.TabIndex = 0;
            // 
            // btnShowRecords
            // 
            this.btnShowRecords.Location = new System.Drawing.Point(443, 24);
            this.btnShowRecords.Name = "btnShowRecords";
            this.btnShowRecords.Size = new System.Drawing.Size(122, 23);
            this.btnShowRecords.TabIndex = 1;
            this.btnShowRecords.Text = "Show all data";
            this.btnShowRecords.UseVisualStyleBackColor = true;
            this.btnShowRecords.Click += new System.EventHandler(this.btnShowRecords_Click);
            // 
            // btnFindByWord
            // 
            this.btnFindByWord.Location = new System.Drawing.Point(443, 60);
            this.btnFindByWord.Name = "btnFindByWord";
            this.btnFindByWord.Size = new System.Drawing.Size(122, 23);
            this.btnFindByWord.TabIndex = 2;
            this.btnFindByWord.Text = "Find by word";
            this.btnFindByWord.UseVisualStyleBackColor = true;
            this.btnFindByWord.Click += new System.EventHandler(this.btnFindByWord_Click);
            // 
            // btnFindByDate
            // 
            this.btnFindByDate.Location = new System.Drawing.Point(443, 96);
            this.btnFindByDate.Name = "btnFindByDate";
            this.btnFindByDate.Size = new System.Drawing.Size(122, 23);
            this.btnFindByDate.TabIndex = 3;
            this.btnFindByDate.Text = "Find by date";
            this.btnFindByDate.UseVisualStyleBackColor = true;
            this.btnFindByDate.Click += new System.EventHandler(this.btnFindByDate_Click);
            // 
            // txtWord
            // 
            this.txtWord.Location = new System.Drawing.Point(304, 61);
            this.txtWord.Name = "txtWord";
            this.txtWord.Size = new System.Drawing.Size(100, 20);
            this.txtWord.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(244, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "/";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(315, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "/";
            // 
            // txtDay
            // 
            this.txtDay.Location = new System.Drawing.Point(191, 97);
            this.txtDay.Name = "txtDay";
            this.txtDay.Size = new System.Drawing.Size(47, 20);
            this.txtDay.TabIndex = 7;
            // 
            // txtMonth
            // 
            this.txtMonth.Location = new System.Drawing.Point(262, 97);
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Size = new System.Drawing.Size(47, 20);
            this.txtMonth.TabIndex = 8;
            // 
            // txtYear
            // 
            this.txtYear.Location = new System.Drawing.Point(333, 97);
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(71, 20);
            this.txtYear.TabIndex = 9;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(443, 469);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(122, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // HelpForm3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 535);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtYear);
            this.Controls.Add(this.txtMonth);
            this.Controls.Add(this.txtDay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtWord);
            this.Controls.Add(this.btnFindByDate);
            this.Controls.Add(this.btnFindByWord);
            this.Controls.Add(this.btnShowRecords);
            this.Controls.Add(this.dataGridView1);
            this.Name = "HelpForm3";
            this.Text = "Messages";
            this.Activated += new System.EventHandler(this.HelpForm3_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnShowRecords;
        private System.Windows.Forms.Button btnFindByWord;
        private System.Windows.Forms.Button btnFindByDate;
        private System.Windows.Forms.TextBox txtWord;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDay;
        private System.Windows.Forms.TextBox txtMonth;
        private System.Windows.Forms.TextBox txtYear;
        private System.Windows.Forms.Button btnClose;
    }
}