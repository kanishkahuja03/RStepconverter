using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RConnectorSetup
{
    internal class InstallerDetails
    {
        // Product Info
        public string ProjectName { get; set; }
        public string ProductName { get; set; }
        public string ProductId { get; set; } // The UpgradeCode for MSI
        public string UpgradeCode { get; set; }
        public Version Version { get; set; }
        public string CompanyName { get; set; }

        // Input/Output Paths
        public string OutFileName { get; set; }
        public string InstallerFilesLocation { get; set; } // Where your bin/Release folder is
        public string RhpFileName { get; set; }    // The actual plugin file (e.g., MyPlugin.rhp)
        public string IsOnNetFramework { get; set; }

        // --- RHINO SPECIFIC SETTINGS ---
        //public string PluginGuid { get; set; }     // The GUID from your C# Plugin class
        public string RhinoVersion { get; set; }   // e.g., "7.0" or "8.0"

    internal static InstallerDetails GetRhinoConnectorInstallerDetailsNetFramework(string isOnNetFramework)
        {
            var rhinoConnectorGuid = "294478EB-58C9-45C2-9B43-BB66B7794C4E"
            ;
            var rhinoConnectoGuidUpgradeCode = "DD9ED6A5-3144-4B75-BB91-925D9CECAA01";
            var directoryInfo = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            directoryInfo = directoryInfo.Parent.Parent.Parent.Parent;
            var rhinoConnectorOutput = Path.Combine(directoryInfo.FullName, "RStepconverter", "RStepconverter", "RStepconverter", "bin", "x64", "Release");
            var rhinoToolsetConnectorFileInfo = new FileInfo(Path.Combine(rhinoConnectorOutput, "RStepconverter.rhp"));
            var versionInforhinoConnector = new Version(1, 0, 0);
            var installerDetails = new InstallerDetails
            {
                ProjectName = "My Rhino Plugin",
                ProductName = "STEP Converter " + versionInforhinoConnector.ToString(),
                CompanyName = "MyCompany",
                Version = versionInforhinoConnector,
                InstallerFilesLocation = rhinoConnectorOutput,
                ProductId = rhinoConnectorGuid.ToString(),
                UpgradeCode = rhinoConnectoGuidUpgradeCode,
                OutFileName = "AutodeskDataExchangeConnectorForRhino8_" + versionInforhinoConnector.ToString(),
                RhpFileName = "RStepconverter.rhp",
                RhinoVersion = "8.0",
                //RUIFilesLocation = Path.Combine(directoryInfo.FullName, "FDXRhinoPlugin", "RUI File", "DataExchangeRhinoConnecter_Template_Meters.3dm"),
                IsOnNetFramework = isOnNetFramework,
            };
            return installerDetails;
        }
    }
}
