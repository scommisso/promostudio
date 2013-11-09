namespace PromoStudio.CloudStatusService
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cloudStatusInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.cloudStatusService = new System.ServiceProcess.ServiceInstaller();
            // 
            // cloudStatusInstaller
            // 
            this.cloudStatusInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.cloudStatusInstaller.Password = null;
            this.cloudStatusInstaller.Username = null;
            // 
            // cloudStatusService
            // 
            this.cloudStatusService.Description = "Checks the cloud rendering status for PromoStudio videos.";
            this.cloudStatusService.DisplayName = "PromoStudio Cloud Status Check";
            this.cloudStatusService.ServiceName = "PromoStudioCloudStatusService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.cloudStatusInstaller,
            this.cloudStatusService});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller cloudStatusInstaller;
        private System.ServiceProcess.ServiceInstaller cloudStatusService;
    }
}