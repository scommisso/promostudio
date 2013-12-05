namespace VDNUploader
{
    partial class TicketForm
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
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rdoB = new System.Windows.Forms.RadioButton();
            this.rdoKB = new System.Windows.Forms.RadioButton();
            this.rdoMB = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFileSize = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtChunkSize = new System.Windows.Forms.TextBox();
            this.txtChunksCount = new System.Windows.Forms.TextBox();
            this.btnGet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Path:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(505, 12);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(34, 27);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(59, 12);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(440, 27);
            this.txtPath.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(266, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "File Size:";
            // 
            // rdoB
            // 
            this.rdoB.AutoSize = true;
            this.rdoB.Location = new System.Drawing.Point(59, 49);
            this.rdoB.Name = "rdoB";
            this.rdoB.Size = new System.Drawing.Size(62, 24);
            this.rdoB.TabIndex = 4;
            this.rdoB.Text = "Bytes";
            this.rdoB.UseVisualStyleBackColor = true;
            this.rdoB.CheckedChanged += new System.EventHandler(this.rdoB_CheckedChanged);
            // 
            // rdoKB
            // 
            this.rdoKB.AutoSize = true;
            this.rdoKB.Checked = true;
            this.rdoKB.Location = new System.Drawing.Point(127, 49);
            this.rdoKB.Name = "rdoKB";
            this.rdoKB.Size = new System.Drawing.Size(45, 24);
            this.rdoKB.TabIndex = 5;
            this.rdoKB.TabStop = true;
            this.rdoKB.Text = "KB";
            this.rdoKB.UseVisualStyleBackColor = true;
            this.rdoKB.CheckedChanged += new System.EventHandler(this.rdoKB_CheckedChanged);
            // 
            // rdoMB
            // 
            this.rdoMB.AutoSize = true;
            this.rdoMB.Location = new System.Drawing.Point(178, 49);
            this.rdoMB.Name = "rdoMB";
            this.rdoMB.Size = new System.Drawing.Size(49, 24);
            this.rdoMB.TabIndex = 6;
            this.rdoMB.Text = "MB";
            this.rdoMB.UseVisualStyleBackColor = true;
            this.rdoMB.CheckedChanged += new System.EventHandler(this.rdoMB_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Units:";
            // 
            // txtFileSize
            // 
            this.txtFileSize.Location = new System.Drawing.Point(338, 46);
            this.txtFileSize.Name = "txtFileSize";
            this.txtFileSize.ReadOnly = true;
            this.txtFileSize.Size = new System.Drawing.Size(201, 27);
            this.txtFileSize.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(249, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Chunk Size:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "Chunks Count:";
            // 
            // txtChunkSize
            // 
            this.txtChunkSize.Location = new System.Drawing.Point(338, 79);
            this.txtChunkSize.Name = "txtChunkSize";
            this.txtChunkSize.Size = new System.Drawing.Size(201, 27);
            this.txtChunkSize.TabIndex = 11;
            this.txtChunkSize.TextChanged += new System.EventHandler(this.txtChunkSize_TextChanged);
            // 
            // txtChunksCount
            // 
            this.txtChunksCount.Location = new System.Drawing.Point(119, 79);
            this.txtChunksCount.Name = "txtChunksCount";
            this.txtChunksCount.Size = new System.Drawing.Size(108, 27);
            this.txtChunksCount.TabIndex = 12;
            this.txtChunksCount.TextChanged += new System.EventHandler(this.txtChunksCount_TextChanged);
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(439, 133);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(100, 36);
            this.btnGet.TabIndex = 13;
            this.btnGet.Text = "Get Ticket";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // TicketForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(551, 181);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.txtChunksCount);
            this.Controls.Add(this.txtChunkSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtFileSize);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rdoMB);
            this.Controls.Add(this.rdoKB);
            this.Controls.Add(this.rdoB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "TicketForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Upload Ticket";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoB;
        private System.Windows.Forms.RadioButton rdoKB;
        private System.Windows.Forms.RadioButton rdoMB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFileSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtChunkSize;
        private System.Windows.Forms.TextBox txtChunksCount;
        private System.Windows.Forms.Button btnGet;
    }
}