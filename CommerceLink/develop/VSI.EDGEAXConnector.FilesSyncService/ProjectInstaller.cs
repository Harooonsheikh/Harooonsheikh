using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.FilesSyncService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            //InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            //get Service name at run time during installation , further detail of paramter can be seen in the Customer Action output properties 

            string serviceName = this.Context.Parameters["SERVICENAME"];
            if (serviceName == null)
            {
                throw new InstallException("Missing parameter 'SERVICENAME'");
            }

            serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();

            // serviceProcessInstaller1

            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.User;
            // this.serviceProcessInstaller1.Password = null;
            //  this.serviceProcessInstaller1.Username = null;

            // serviceInstaller1

            this.serviceInstaller1.Description = "Service to download and upload files";
            this.serviceInstaller1.DisplayName = serviceName;
            this.serviceInstaller1.ServiceName = serviceName;


            // ProjectInstaller         

            Installers.Add(serviceProcessInstaller1);
            Installers.Add(serviceInstaller1);

            base.Install(stateSaver);

        }
    }
}
