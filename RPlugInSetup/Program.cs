using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RConnectorSetup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var rhinoInstallerDetails = InstallerDetails.GetRhinoConnectorInstallerDetailsNetFramework("Net48");
            var wixInstaller = new WixInstaller();
            wixInstaller.CreateInstaller(rhinoInstallerDetails);
        }
    }
}
