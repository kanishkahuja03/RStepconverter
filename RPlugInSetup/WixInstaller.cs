using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WixSharp;
using WixSharp.CommonTasks;
using Assembly = System.Reflection.Assembly;
using File = WixSharp.File;

namespace RConnectorSetup
{
    internal class WixInstaller
    {
        internal string CreateInstaller(InstallerDetails installerDetails)
        {
            try
            {
                Console.WriteLine("Installer creation is started.");
                var directoryInfo = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                var outputFile = new FileInfo(Path.Combine(directoryInfo.FullName, "Installers", installerDetails.OutFileName, ".msi"));
                if (outputFile.Exists)
                {
                    Console.WriteLine("Found old installer build and deleted those before creating new installer.");
                    outputFile.Delete();
                }
                else
                {
                    Console.WriteLine("No old installer(s) are found.");
                }
                string rhinoRegKey = $@"SOFTWARE\McNeel\Rhinoceros\{installerDetails.RhinoVersion}\Plug-ins\{installerDetails.ProductId}";
                var project = new Project
                {
                    OutDir = "Installers" + "/" + installerDetails.IsOnNetFramework,
                    Name = installerDetails.ProjectName,
                    UpgradeCode = new Guid(installerDetails.UpgradeCode),
                    GUID = new Guid(installerDetails.ProductId),
                    Description = installerDetails.ProjectName,
                    Platform = Platform.x64,
                    UI = WUI.WixUI_Minimal,
                    Version = installerDetails.Version,
                    MajorUpgrade = MajorUpgrade.Default,
                    ControlPanelInfo = { Manufacturer = installerDetails.CompanyName },

                    // 2. CRITICAL: Force Per-Machine (Admin) Installation
                    // Without this, we cannot write to HKLM, and the "Local User" bug persists.
                    InstallScope = InstallScope.perMachine,

                    RegValues = new[]
                    {
                        // Register the Plugin Name
                        new RegValue(RegistryHive.LocalMachine, rhinoRegKey, "Name", installerDetails.ProductName),

                        // Register the Path (Rhino needs to know where the .rhp is)
                        // [INSTALLDIR] resolves to "C:\Program Files\MyCompany\..."
                        new RegValue(RegistryHive.LocalMachine, rhinoRegKey, "FileName", $"[INSTALLDIR]{installerDetails.RhpFileName}"),

                        // Optional Metadata
                        new RegValue(RegistryHive.LocalMachine, rhinoRegKey, "Organization", installerDetails.CompanyName),
                        new RegValue(RegistryHive.LocalMachine, rhinoRegKey, "Description", "Installed via MSI")
                    }
                    //Actions = new WixSharp.Action[]
                    //{
                    //    new ElevatedManagedAction(CustomActions.CheckRhinoInstalled)
                    //    {
                    //        Return = Return.check,
                    //        When = When.Before,
                    //        Step = Step.InstallFiles
                    //    },
                    //    new ElevatedManagedAction(CustomActions.UnInstallService)
                    //    {
                    //        Return = Return.check,
                    //        When = When.After,
                    //        Step = Step.RemoveFiles,
                    //        Condition = WixSharp.Condition.BeingUninstalled
                    //    }
                    //}
                };
                //ILRepackHelper.MergeAssembly(installerDetails);
                //var wixEntities = GenerateWixEntities(installerDetails.InstallerFilesLocation, installerDetails.FileFilter, installerDetails.RUIFilesLocation, installerDetails.IsOnNetFramework).ToList();
                //project.InstallScope = InstallScope.perUser;
                //project.OutFileName = installerDetails.OutFileName;
                //string pluginDir = Path.Combine(@"%AppDataFolder%", "McNeel", "Rhinoceros", "8.0");
                //string targetFramework;
                //if (installerDetails.IsOnNetFramework.Contains("NET8"))
                //{
                //    targetFramework = "net8.0-windows";
                //}
                //else
                //{

                //    targetFramework = "net48";
                //}
                //RegValue[] regValues = GetRegistoryValues(targetFramework);

                //project.Dirs = new Dir[]
                //{
                //  new InstallDir(pluginDir,wixEntities.ToArray())

                //};
                //project.RegValues = regValues;

                var msi = project.BuildMsi();
                Console.WriteLine("Installer created here : " + msi);


                return msi;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}