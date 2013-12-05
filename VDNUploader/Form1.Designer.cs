namespace VDNUploader
{
    partial class BaseForm
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblUsername = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblActiveThreads = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblChunksUploaded = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblChunksVerified = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTokenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveTokenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTokenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTicketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTicketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTicketToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aPIKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.proxyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_path = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_chunk_size = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_file_size = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_total_chunks = new System.Windows.Forms.TextBox();
            this.lstChunks = new System.Windows.Forms.ListView();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectRest = new System.Windows.Forms.Button();
            this.btnDeselectAll = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.btnComplete = new System.Windows.Forms.Button();
            this.manuallyEnterTokenDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblUsername,
            this.lblActiveThreads,
            this.lblChunksUploaded,
            this.lblChunksVerified});
            this.statusStrip1.Location = new System.Drawing.Point(0, 426);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(624, 24);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblUsername
            // 
            this.lblUsername.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(92, 19);
            this.lblUsername.Text = "(Not logged in)";
            // 
            // lblActiveThreads
            // 
            this.lblActiveThreads.Name = "lblActiveThreads";
            this.lblActiveThreads.Size = new System.Drawing.Size(105, 19);
            this.lblActiveThreads.Text = "0 Active Uploaders";
            // 
            // lblChunksUploaded
            // 
            this.lblChunksUploaded.Name = "lblChunksUploaded";
            this.lblChunksUploaded.Size = new System.Drawing.Size(110, 19);
            this.lblChunksUploaded.Text = "0 Chunks Uploaded";
            // 
            // lblChunksVerified
            // 
            this.lblChunksVerified.Name = "lblChunksVerified";
            this.lblChunksVerified.Size = new System.Drawing.Size(110, 19);
            this.lblChunksVerified.Text = "0/0 Chunks Verified";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToolStripMenuItem,
            this.uploadToolStripMenuItem,
            this.fToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(624, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTokenToolStripMenuItem,
            this.manuallyEnterTokenDataToolStripMenuItem,
            this.saveTokenToolStripMenuItem,
            this.loadTokenToolStripMenuItem,
            this.toolStripMenuItem1,
            this.quitToolStripMenuItem});
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.loginToolStripMenuItem.Text = "Login";
            // 
            // newTokenToolStripMenuItem
            // 
            this.newTokenToolStripMenuItem.Name = "newTokenToolStripMenuItem";
            this.newTokenToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.newTokenToolStripMenuItem.Text = "New Token...";
            this.newTokenToolStripMenuItem.Click += new System.EventHandler(this.newTokenToolStripMenuItem_Click);
            // 
            // saveTokenToolStripMenuItem
            // 
            this.saveTokenToolStripMenuItem.Name = "saveTokenToolStripMenuItem";
            this.saveTokenToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.saveTokenToolStripMenuItem.Text = "Save Token...";
            this.saveTokenToolStripMenuItem.Click += new System.EventHandler(this.saveTokenToolStripMenuItem_Click);
            // 
            // loadTokenToolStripMenuItem
            // 
            this.loadTokenToolStripMenuItem.Name = "loadTokenToolStripMenuItem";
            this.loadTokenToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.loadTokenToolStripMenuItem.Text = "Load Token...";
            this.loadTokenToolStripMenuItem.Click += new System.EventHandler(this.loadTokenToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(222, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // uploadToolStripMenuItem
            // 
            this.uploadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTicketToolStripMenuItem,
            this.loadTicketToolStripMenuItem,
            this.loadTicketToolStripMenuItem1});
            this.uploadToolStripMenuItem.Name = "uploadToolStripMenuItem";
            this.uploadToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.uploadToolStripMenuItem.Text = "Upload";
            // 
            // newTicketToolStripMenuItem
            // 
            this.newTicketToolStripMenuItem.Name = "newTicketToolStripMenuItem";
            this.newTicketToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.newTicketToolStripMenuItem.Text = "New Ticket...";
            this.newTicketToolStripMenuItem.Click += new System.EventHandler(this.newTicketToolStripMenuItem_Click);
            // 
            // loadTicketToolStripMenuItem
            // 
            this.loadTicketToolStripMenuItem.Name = "loadTicketToolStripMenuItem";
            this.loadTicketToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.loadTicketToolStripMenuItem.Text = "Save Ticket...";
            this.loadTicketToolStripMenuItem.Click += new System.EventHandler(this.loadTicketToolStripMenuItem_Click);
            // 
            // loadTicketToolStripMenuItem1
            // 
            this.loadTicketToolStripMenuItem1.Name = "loadTicketToolStripMenuItem1";
            this.loadTicketToolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
            this.loadTicketToolStripMenuItem1.Text = "Load Ticket...";
            this.loadTicketToolStripMenuItem1.Click += new System.EventHandler(this.loadTicketToolStripMenuItem1_Click);
            // 
            // fToolStripMenuItem
            // 
            this.fToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aPIKeyToolStripMenuItem,
            this.proxyToolStripMenuItem});
            this.fToolStripMenuItem.Name = "fToolStripMenuItem";
            this.fToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.fToolStripMenuItem.Text = "Connection";
            // 
            // aPIKeyToolStripMenuItem
            // 
            this.aPIKeyToolStripMenuItem.Name = "aPIKeyToolStripMenuItem";
            this.aPIKeyToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.aPIKeyToolStripMenuItem.Text = "API Key...";
            this.aPIKeyToolStripMenuItem.Click += new System.EventHandler(this.aPIKeyToolStripMenuItem_Click);
            // 
            // proxyToolStripMenuItem
            // 
            this.proxyToolStripMenuItem.Name = "proxyToolStripMenuItem";
            this.proxyToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.proxyToolStripMenuItem.Text = "Proxy...";
            this.proxyToolStripMenuItem.Click += new System.EventHandler(this.proxyToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "File:";
            // 
            // txt_path
            // 
            this.txt_path.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_path.Location = new System.Drawing.Point(53, 33);
            this.txt_path.Name = "txt_path";
            this.txt_path.ReadOnly = true;
            this.txt_path.Size = new System.Drawing.Size(559, 27);
            this.txt_path.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Chunk Size:";
            // 
            // txt_chunk_size
            // 
            this.txt_chunk_size.Location = new System.Drawing.Point(101, 74);
            this.txt_chunk_size.Name = "txt_chunk_size";
            this.txt_chunk_size.ReadOnly = true;
            this.txt_chunk_size.Size = new System.Drawing.Size(122, 27);
            this.txt_chunk_size.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(232, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "File Size:";
            // 
            // txt_file_size
            // 
            this.txt_file_size.Location = new System.Drawing.Point(304, 74);
            this.txt_file_size.Name = "txt_file_size";
            this.txt_file_size.ReadOnly = true;
            this.txt_file_size.Size = new System.Drawing.Size(136, 27);
            this.txt_file_size.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(446, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Total Chunks:";
            // 
            // txt_total_chunks
            // 
            this.txt_total_chunks.Location = new System.Drawing.Point(548, 74);
            this.txt_total_chunks.Name = "txt_total_chunks";
            this.txt_total_chunks.ReadOnly = true;
            this.txt_total_chunks.Size = new System.Drawing.Size(64, 27);
            this.txt_total_chunks.TabIndex = 9;
            // 
            // lstChunks
            // 
            this.lstChunks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstChunks.CheckBoxes = true;
            this.lstChunks.Enabled = false;
            this.lstChunks.Location = new System.Drawing.Point(16, 166);
            this.lstChunks.Name = "lstChunks";
            this.lstChunks.Size = new System.Drawing.Size(596, 205);
            this.lstChunks.TabIndex = 10;
            this.lstChunks.UseCompatibleStateImageBehavior = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Chunks:";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectAll.Enabled = false;
            this.btnSelectAll.Location = new System.Drawing.Point(283, 130);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(95, 30);
            this.btnSelectAll.TabIndex = 12;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnSelectRest
            // 
            this.btnSelectRest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectRest.Enabled = false;
            this.btnSelectRest.Location = new System.Drawing.Point(384, 130);
            this.btnSelectRest.Name = "btnSelectRest";
            this.btnSelectRest.Size = new System.Drawing.Size(107, 30);
            this.btnSelectRest.TabIndex = 13;
            this.btnSelectRest.Text = "Select Rest";
            this.btnSelectRest.UseVisualStyleBackColor = true;
            this.btnSelectRest.Click += new System.EventHandler(this.btnSelectRest_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeselectAll.Enabled = false;
            this.btnDeselectAll.Location = new System.Drawing.Point(497, 130);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(115, 30);
            this.btnDeselectAll.TabIndex = 14;
            this.btnDeselectAll.Text = "Deselect All";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpload.Enabled = false;
            this.btnUpload.Location = new System.Drawing.Point(16, 377);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(123, 31);
            this.btnUpload.TabIndex = 15;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnVerify.Enabled = false;
            this.btnVerify.Location = new System.Drawing.Point(145, 377);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(123, 31);
            this.btnVerify.TabIndex = 16;
            this.btnVerify.Text = "Verify / Resume";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnComplete
            // 
            this.btnComplete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnComplete.Enabled = false;
            this.btnComplete.Location = new System.Drawing.Point(489, 377);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(123, 31);
            this.btnComplete.TabIndex = 17;
            this.btnComplete.Text = "Finish";
            this.btnComplete.UseVisualStyleBackColor = true;
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // manuallyEnterTokenDataToolStripMenuItem
            // 
            this.manuallyEnterTokenDataToolStripMenuItem.Name = "manuallyEnterTokenDataToolStripMenuItem";
            this.manuallyEnterTokenDataToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.manuallyEnterTokenDataToolStripMenuItem.Text = "Manually Enter Token Data...";
            this.manuallyEnterTokenDataToolStripMenuItem.Click += new System.EventHandler(this.manuallyEnterTokenDataToolStripMenuItem_Click);
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(624, 450);
            this.Controls.Add(this.btnComplete);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.btnDeselectAll);
            this.Controls.Add(this.btnSelectRest);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lstChunks);
            this.Controls.Add(this.txt_total_chunks);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_file_size);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_chunk_size);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_path);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "BaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VimeoDotNet Uploader";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblActiveThreads;
        private System.Windows.Forms.ToolStripStatusLabel lblChunksUploaded;
        private System.Windows.Forms.ToolStripStatusLabel lblChunksVerified;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblUsername;
        private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTokenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveTokenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTokenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uploadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTicketToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTicketToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTicketToolStripMenuItem1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_path;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_chunk_size;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_file_size;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_total_chunks;
        private System.Windows.Forms.ListView lstChunks;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectRest;
        private System.Windows.Forms.Button btnDeselectAll;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnComplete;
        private System.Windows.Forms.ToolStripMenuItem fToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aPIKeyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem proxyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manuallyEnterTokenDataToolStripMenuItem;

    }
}

