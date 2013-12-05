using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vimeo.API;
using File = System.IO.File;
namespace VDNUploader
{
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();

            RefreshToken();

            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.TicketId))
            {
                if (File.Exists(Properties.Settings.Default.UploadFile))
                {
                    Ticket t = ApiKeyForm.vc.vimeo_videos_upload_checkTicket(Properties.Settings.Default.TicketId);

                    if (t != null && t.id == Properties.Settings.Default.TicketId &&
                        t.endpoint == Properties.Settings.Default.TicketEndPoint)
                    {
                        TicketForm.UploadFile = new System.IO.FileInfo(Properties.Settings.Default.UploadFile);
                        TicketForm.ChunkSize = Properties.Settings.Default.ChunkSize;
                        TicketForm.Ticket = t;

                        RefreshTicket();
                        VerifyChunks();
                    }
                    else
                    {
                        MessageBox.Show("Invalid ticket.");
                    }
                }
                else
                {
                    MessageBox.Show("File " + Properties.Settings.Default.UploadFile + " not found.");
                }
            }
        }

        VimeoClient vc { get { return ApiKeyForm.vc; } }
        int chunkCount;
        int chunksUploaded = 0;
        int uploaders = 0;

        void RefreshToken()
        {
            if (vc.Me == null)
            {
                uploadToolStripMenuItem.Enabled = false;
            }
            else
            {
                lblUsername.Text = vc.Me.display_name;
                uploadToolStripMenuItem.Enabled = true;
            }
        }

        void RefreshTicket()
        {
            if (TicketForm.Ticket != null)
            {
                if (!TicketForm.UploadFile.Exists)
                {
                    MessageBox.Show("File " + TicketForm.UploadFile.FullName + " does not exist.");
                    return;
                }

                txt_path.Text = TicketForm.UploadFile.FullName;
                txt_chunk_size.Text = TicketForm.ChunkSize.ToString() + "B";
                txt_file_size.Text = TicketForm.UploadFile.Length.ToString() + "B";

                txt_total_chunks.Text = (chunkCount = ApiKeyForm.vc.GetChunksCount(TicketForm.UploadFile.Length, TicketForm.ChunkSize)).ToString();

                lstChunks.Enabled =
                    btnSelectAll.Enabled =
                    btnSelectRest.Enabled =
                    btnDeselectAll.Enabled =
                    btnUpload.Enabled =
                    btnVerify.Enabled =
                    btnComplete.Enabled =
                    true;

                chunksUploaded = 0;
            }
        }

        void RefreshChunks()
        {
            lstChunks.Clear();
            chunkCount = ApiKeyForm.vc.GetChunksCount(TicketForm.UploadFile.Length, TicketForm.ChunkSize);
            for (int i = 0; i < chunkCount; i++)
            {
                lstChunks.Items.Add(i.ToString(), i.ToString(), "normal");
                lstChunks.Items[i.ToString()].BackColor = Color.White;
            }
        }

        void VerifyChunks()
        {
            RefreshChunks();
            var v = ApiKeyForm.vc.vimeo_videos_upload_verifyChunks(TicketForm.Ticket);
            var size = TicketForm.UploadFile.Length;
            if (v != null)
            {
                foreach (var item in v.Items)
                {
                    if (item.size == ApiKeyForm.vc.GetChunkSize(size, item.id, TicketForm.ChunkSize))
                        lstChunks.Items[item.id.ToString()].Remove();
                }

                lblChunksVerified.Text = v.Items.Count.ToString() + "/" + chunkCount + " Chunks Verified";
            }
            else
                lblChunksVerified.Text = "0/" + chunkCount + " Chunks Verified";            
        }

        private void newTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new TokenForm().ShowDialog() == System.Windows.Forms.DialogResult.OK)
                RefreshToken();
        }

        private void saveTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = new SaveFileDialog();
            d.Title = "Save Token";
            d.DefaultExt = "vtk";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;

            using (var sw = File.CreateText(d.FileName))
            {
                sw.WriteLine(vc.Token);
                sw.WriteLine(vc.TokenSecret);
                sw.Close();
            }
        }

        private void loadTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = new OpenFileDialog();
            d.Title = "Load Token";
            d.DefaultExt = "vtk";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;
            if (File.Exists(d.FileName))
            {
                try
                {
                    var lines = File.ReadAllLines(d.FileName);
                    var token = lines[0];
                    var secret = lines[1];
                    var oldtoken = vc.Token;
                    var oldsecret = vc.TokenSecret;
                    if (vc.Login(token, secret))
                        RefreshToken();
                    else
                    {
                        vc.Login(oldsecret, oldsecret);
                    }
                }
                catch
                {
                    MessageBox.Show("Error loading token.");
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aPIKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ApiKeyForm().ShowDialog();
        }

        private void proxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ProxyForm().ShowDialog();
        }

        private void newTicketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = new TicketForm().ShowDialog();
            if (d == System.Windows.Forms.DialogResult.OK)
            {
                RefreshTicket();
                RefreshChunks();
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstChunks.Items)
            {
                item.Checked = true;
            }
        }

        private void btnSelectRest_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstChunks.Items)
            {
                if (item.BackColor == Color.White) item.Checked = true;
            }
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstChunks.Items)
            {
                item.Checked = false;
            }
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            VerifyChunks();
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            VerifyChunks();
            if (lstChunks.Items.Count == 0 || 
                MessageBox.Show(
                    "There are still " + lstChunks.Items.Count + " chunks left.\nAre you sure you want to complete the upload?", 
                    "Warning",
                    MessageBoxButtons.YesNo
                ) == System.Windows.Forms.DialogResult.Yes
                )
            {
                var result = vc.vimeo_videos_upload_complete(TicketForm.UploadFile.Name, TicketForm.Ticket);
                if (string.IsNullOrWhiteSpace(result))
                {
                    MessageBox.Show("Error completing upload");
                    return;
                }

                TicketForm.UploadFile = null;
                TicketForm.Ticket = null;
                Properties.Settings.Default.TicketId = String.Empty;
                Properties.Settings.Default.Save();

                MessageBox.Show("Upload completed successfully. Video ID=" + result);
                txt_path.Text = "Video ID= " + result;

                lstChunks.Enabled =
                    btnSelectAll.Enabled =
                    btnSelectRest.Enabled =
                    btnDeselectAll.Enabled =
                    btnUpload.Enabled =
                    btnVerify.Enabled =
                    btnComplete.Enabled =
                    false;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            Queue<int> indices = new Queue<int>();
            
            Random r = new Random();
            Color c = Color.FromArgb(
                128 + r.Next(128),
                128 + r.Next(128),
                128 + r.Next(128));
            
            foreach (ListViewItem item in lstChunks.Items)
            {
                if (item.Checked)
                {
                    indices.Enqueue(int.Parse(item.Text));
                    item.BackColor = c;
                    item.Checked = false;
                }
            }

            new System.Threading.Thread(
                new System.Threading.ThreadStart(
                    () => PostChunks(indices))).Start();
        }

        void PostChunks(Queue<int> indices)
        {
            uploaders++;
            Invoke(new Action(
                () =>
                {
                    lblActiveThreads.Text = uploaders.ToString() + " Active Uploaders";
                }));

            while (indices.Count > 0)
            {
                var id = indices.Dequeue();
                try
                {
                    vc.PostVideo(TicketForm.Ticket, id, TicketForm.UploadFile.FullName, TicketForm.ChunkSize);
                    Invoke(new Action(
                        () =>
                        {
                            if (lstChunks.Items.ContainsKey(id.ToString())) 
                                lstChunks.Items.RemoveByKey(id.ToString());
                        }));
                }
                catch
                {
                    Invoke(new Action(
                        () =>
                        {
                            if (lstChunks.Items.ContainsKey(id.ToString())) 
                                lstChunks.Items[id.ToString()].BackColor = Color.White;
                        }));
                }
                chunksUploaded++;
                Invoke(new Action(
                    () =>
                    {
                        lblChunksUploaded.Text = chunksUploaded.ToString() + " Chunks Uploaded";
                    }));
            }

            uploaders--;
            Invoke(new Action(
                () =>
                {
                    lblActiveThreads.Text = uploaders.ToString() + " Active Uploaders";
                }));
        }

        private void loadTicketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var d = new SaveFileDialog();
            d.Title = "Save Upload Ticket";
            d.DefaultExt = "vut";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;

            using (var sw = File.CreateText(d.FileName))
            {
                sw.WriteLine(TicketForm.UploadFile.FullName);
                sw.WriteLine(TicketForm.ChunkSize.ToString());
                sw.WriteLine(TicketForm.Ticket.id);
                sw.WriteLine(TicketForm.Ticket.endpoint);
                sw.Close();
            }
        }

        private void loadTicketToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var d = new OpenFileDialog();
            d.Title = "Load Upload Token";
            d.DefaultExt = "vut";
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;
            if (File.Exists(d.FileName))
            {
                try
                {
                    var lines = File.ReadAllLines(d.FileName);
                    var path = lines[0];
                    var chunksize = int.Parse(lines[1]);
                    var ticket_id = lines[2];
                    var endpoint = lines[3];

                    if (!File.Exists(path))
                    {
                        MessageBox.Show("File " + path + "does not exist", "Error");
                        return;
                    }

                    var t = ApiKeyForm.vc.vimeo_videos_upload_checkTicket(ticket_id);
                    if (t != null && t.id == ticket_id && t.endpoint == endpoint)
                    {
                        if (Properties.Settings.Default.TicketId != String.Empty)
                        {
                            var ask = MessageBox.Show(
                                "There's another upload in progress. If you continue,\n" +
                                "you can't resume that upload anymore. Do you want to load this ticket?",
                                "Warning",
                                MessageBoxButtons.YesNo);
                            if (ask == System.Windows.Forms.DialogResult.Yes)
                            {
                                Properties.Settings.Default.TicketId = t.id;
                                Properties.Settings.Default.TicketEndPoint = t.endpoint;
                                Properties.Settings.Default.UploadFile = path;
                                Properties.Settings.Default.ChunkSize = chunksize;
                                Properties.Settings.Default.Save();

                                TicketForm.Ticket = t;
                                TicketForm.UploadFile = new System.IO.FileInfo(path);
                                TicketForm.ChunkSize = chunksize;

                                RefreshTicket();
                                VerifyChunks();
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Error loading token.");
                }
            }
        }

        private void manuallyEnterTokenDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new ManualToken().ShowDialog() == System.Windows.Forms.DialogResult.OK)
                RefreshToken();
        }
    }
}
