using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vimeo.API;

namespace VDNUploader
{
    public partial class ApiKeyForm : Form
    {
        public static VimeoClient vc;

        public ApiKeyForm()
        {
            InitializeComponent();
            txtKey.Text = Properties.Settings.Default.APIKey;
            txtSecret.Text = Properties.Settings.Default.APISecret;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.APIKey = txtKey.Text.Trim();
            Properties.Settings.Default.APISecret = txtSecret.Text.Trim();
            Properties.Settings.Default.Save();
            vc.ChangeKey(Properties.Settings.Default.APIKey, Properties.Settings.Default.APISecret);
            Close();
        }
    }
}
