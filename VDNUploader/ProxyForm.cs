using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VDNUploader
{
    public partial class ProxyForm : Form
    {
        public ProxyForm()
        {
            InitializeComponent();

            chkProxy.Checked = Properties.Settings.Default.UseProxy;
            txtHost.Text = Properties.Settings.Default.ProxyHost;
            txtPort.Text = Properties.Settings.Default.ProxyPort;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UseProxy = chkProxy.Checked;
            if (chkProxy.Checked)
                ApiKeyForm.vc.Proxy = new System.Net.WebProxy(
                    txtHost.Text.Trim(),
                    int.Parse(txtPort.Text.Trim())
                    );

            Properties.Settings.Default.ProxyHost = txtHost.Text.Trim();
            Properties.Settings.Default.ProxyPort = txtPort.Text.Trim();
            Properties.Settings.Default.Save();
            
            Close();
        }
    }
}
