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
    public partial class TokenForm : Form
    {
        public TokenForm()
        {
            InitializeComponent();
            NewUrl();
        }

        void NewUrl()
        {
            ApiKeyForm.vc.GetUnauthorizedRequestToken();
            txtUrl.Text = ApiKeyForm.vc.GenerateAuthorizationUrl();
        }

        private void btnProxy_Click(object sender, EventArgs e)
        {
            new ProxyForm().ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            NewUrl();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(txtUrl.Text);
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            ApiKeyForm.vc.GetAccessToken(txtVerifier.Text);
            if (!ApiKeyForm.vc.Login())
            {
                MessageBox.Show("Could not log in.");
                DialogResult = System.Windows.Forms.DialogResult.Abort;
                return;
            }
            else
            {
                Properties.Settings.Default.Token = ApiKeyForm.vc.Token;
                Properties.Settings.Default.TokenSecret = ApiKeyForm.vc.TokenSecret;
                Properties.Settings.Default.Save();
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
