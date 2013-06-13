namespace PromoStudio.RenderQueueService
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
            this.promoStudioInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.promoStudioService = new System.ServiceProcess.ServiceInstaller();
            // 
            // promoStudioInstaller
            // 
            this.promoStudioInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.promoStudioInstaller.Password = null;
            this.promoStudioInstaller.Username = null;
            // 
            // promoStudioService
            // 
            this.promoStudioService.Description = "Renders videos and templates for PromoStudio";
            this.promoStudioService.DisplayName = "PromoStudio Renderer";
            this.promoStudioService.ServiceName = "PromoStudioService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.promoStudioInstaller,
            this.promoStudioService});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller promoStudioInstaller;
        private System.ServiceProcess.ServiceInstaller promoStudioService;
    }
}