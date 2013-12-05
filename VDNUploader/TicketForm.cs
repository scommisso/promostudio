using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using File = System.IO.File;
using System.IO;

namespace VDNUploader
{
    public partial class TicketForm : Form
    {
        public TicketForm()
        {
            InitializeComponent();
        }

        public static Vimeo.API.Ticket Ticket = null;

        public static FileInfo UploadFile = null;
        public static int ChunkSize;

        FileInfo file = null;
        long chunkSize = 1024 * 1024;

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var d = new OpenFileDialog();
            d.Title = "Pick a video file";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (File.Exists(d.FileName))
                {
                    txtPath.Text = d.FileName;
                    file = new FileInfo(d.FileName);

                    if (rdoB.Checked)
                    {
                        txtFileSize.Text = file.Length.ToString();
                        txtChunkSize.Text = chunkSize.ToString();
                    }

                    else if (rdoKB.Checked)
                    {
                        txtFileSize.Text = (file.Length / 1024).ToString();
                        txtChunkSize.Text = (1024).ToString();
                    }

                    else
                    {
                        txtFileSize.Text = (file.Length / (1024 * 1024)).ToString();
                        txtChunkSize.Text = (1).ToString();
                    }

                    txtChunksCount.Text = ApiKeyForm.vc.GetChunksCount(file.Length, (int)chunkSize).ToString();
                }
            }
        }

        private void rdoB_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoB.Checked)
            {
                txtFileSize.Text = file.Length.ToString();
                txtChunkSize.Text = chunkSize.ToString();
            }
        }

        private void rdoKB_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoKB.Checked)
            {
                txtFileSize.Text = (file.Length/1024).ToString();
                txtChunkSize.Text = (chunkSize/1024).ToString();
            }
        }

        private void rdoMB_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoMB.Checked)
            {
                txtFileSize.Text = (file.Length / (1024*1024)).ToString();
                txtChunkSize.Text = (chunkSize / (1024*1024)).ToString();
            }
        }

        void RemoveLetters(TextBox box)
        {
            for (int i = 0; i < box.Text.Length; i++)
            {
                if (!(box.Text[i] >= '0' && box.Text[i] <= '9'))
                {
                    box.Text = box.Text.Replace(box.Text[i].ToString(), "");
                }
            }
        }

        bool lockc = false;

        private void txtChunkSize_TextChanged(object sender, EventArgs e)
        {
            if (lockc) return;
            lockc = true;
            RemoveLetters(txtChunkSize);
            if (string.IsNullOrWhiteSpace(txtChunkSize.Text)) return;

            if (rdoB.Checked) chunkSize = long.Parse(txtChunkSize.Text);
            else if (rdoKB.Checked) chunkSize = 1024 * long.Parse(txtChunkSize.Text);
            else chunkSize = 1024 * 1024 * long.Parse(txtChunkSize.Text);

            txtChunksCount.Text = ApiKeyForm.vc.GetChunksCount(file.Length, (int)chunkSize).ToString();
            lockc = false;
        }

        private void txtChunksCount_TextChanged(object sender, EventArgs e)
        {
            if (lockc) return;
            lockc = true;
            RemoveLetters(txtChunksCount);

            if (string.IsNullOrWhiteSpace(txtChunksCount.Text)) txtChunksCount.Text = "1";
            int count = int.Parse(txtChunksCount.Text);
            if (count == 0)
            {
                txtChunksCount.Text = "1";
                count = 1;
            }

                chunkSize = (long)Math.Ceiling((decimal)file.Length / (decimal)count);

                if (rdoB.Checked)
                    txtChunkSize.Text = chunkSize.ToString();
                else if (rdoKB.Checked)
                    txtChunkSize.Text = (chunkSize / 1024).ToString();
                else 
                    txtChunkSize.Text = (chunkSize / (1024 * 1024)).ToString();
            
            lockc = false;
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            if (chunkSize < 200 * 1024)
            {
                MessageBox.Show("Chunk size is less than 200KB. That's too small.", "Error");
                return;
            }

            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.TicketId))
            {
                if (MessageBox.Show(
                                "There's another upload in progress. If you continue,\n" +
                                "you can't resume that upload anymore. Do you want to load this ticket?",
                                "Warning",
                                MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
            }
            Vimeo.API.Ticket t = ApiKeyForm.vc.vimeo_videos_upload_getTicket();
            if (t != null)
            {
                Ticket = t;
                UploadFile = file;
                ChunkSize = (int)chunkSize;

                Properties.Settings.Default.TicketId = Ticket.id;
                Properties.Settings.Default.TicketEndPoint = Ticket.endpoint;
                Properties.Settings.Default.UploadFile = UploadFile.FullName;
                Properties.Settings.Default.ChunkSize = ChunkSize;
                Properties.Settings.Default.Save();

                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Error getting ticket.");
                return;
            }

            Close();
        }
    }
}
