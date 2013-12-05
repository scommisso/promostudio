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
    public partial class ManualToken : Form
    {
        public ManualToken()
        {
            InitializeComponent();
            txtToken.Text = Properties.Settings.Default.Token;
            txtSecret.Text = Properties.Settings.Default.TokenSecret;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var oldToken = ApiKeyForm.vc.Token;
            var oldSecret = ApiKeyForm.vc.TokenSecret;
            if (ApiKeyForm.vc.Login(txtToken.Text.Trim(), txtSecret.Text.Trim()))
            {
                Properties.Settings.Default.Token = txtToken.Text.Trim();
                Properties.Settings.Default.TokenSecret = txtSecret.Text.Trim();
                Properties.Settings.Default.Save();
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            else
            {
                ApiKeyForm.vc.Token = oldToken;
                ApiKeyForm.vc.TokenSecret = oldSecret;
                MessageBox.Show("Invalid token");
                return;
            }
        }
    }
}
