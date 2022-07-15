using System;
using System.Diagnostics;

namespace VSI.EDGEAXConnector.WindowService
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
            try
            {
                //if (EventLog.SourceExists(EDGEAXConnectorWindowsService.ServiceDisplayName)) //TODOUsman
                //{
                //    EventLog.DeleteEventSource(EDGEAXConnectorWindowsService.ServiceDisplayName);
                //}
            }
            catch (Exception)
            {
                // do nothing - access problem
            }

            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Description = "EdgeAX CommerceLink Sync Service - runs scheduler that controls data synchronizin" +
    "g jobs";
            this.serviceInstaller1.DisplayName = "EdgeAX CommerceLink Sync Service";
            this.serviceInstaller1.ServiceName = "EdgeAX CommerceLink Sync Service";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}