using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Install;
using System.ComponentModel;
using System.ServiceProcess;

namespace WindowsService1
{
    [RunInstaller(true)]
    public class WindowsService1Installer : Installer
    {
        public WindowsService1Installer()
        {
            ServiceProcessInstaller serviceProcessInstaller =
                              new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            //# Service Account Information
            serviceProcessInstaller.Account = ServiceAccount.User;
            serviceProcessInstaller.Username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            serviceProcessInstaller.Password = null;

            //# Service Information
            serviceInstaller.DisplayName = "NovediaPrinterServicee";
            serviceInstaller.StartType = ServiceStartMode.Manual;

            //# This must be identical to the WindowsService.ServiceBase name
            //# set in the constructor of WindowsService.cs
            serviceInstaller.ServiceName = "NovediaPrinterService";
          
            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
