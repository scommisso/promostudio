using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;

namespace VDNUploader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ApiKeyForm.vc = new Vimeo.API.VimeoClient(
                Properties.Settings.Default.APIKey,
                Properties.Settings.Default.APISecret);

            if (Properties.Settings.Default.UseProxy)
                ApiKeyForm.vc.Proxy = new System.Net.WebProxy(
                    Properties.Settings.Default.ProxyHost,
                    int.Parse(Properties.Settings.Default.ProxyPort));

            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.APIKey))
            {
                new ApiKeyForm().ShowDialog();
            }

            AsyncForm af = new AsyncForm();
            af.label1.Text = "Logging in...";
            af.Text = "VimeoDotNet Uploader";
            af.Show();
            Debug.WriteLine(ApiKeyForm.vc.Login(
                Properties.Settings.Default.Token,
                Properties.Settings.Default.TokenSecret));
            af.Close();

            Application.Run(new BaseForm());
        }
    }
}
