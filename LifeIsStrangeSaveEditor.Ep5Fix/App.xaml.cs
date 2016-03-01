using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LifeIsStrangeSaveEditor.Ep5Fix
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (DoStartupChecks())
            {
                new MainWindow().ShowDialog();
            }

            Shutdown();
        }

        private bool DoStartupChecks()
        {
            var retVal = true;

            if (!Directory.Exists(FileSystemOperations.SaveGameFolder))
            {
                var dlg = new System.Windows.Forms.FolderBrowserDialog()
                {
                    Description = "Please select the Life is Strange save games folder:",
                    ShowNewFolderButton = false
                };

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileSystemOperations.SetSaveGameFolder(dlg.SelectedPath);
                }
                else
                {
                    retVal = false;
                }
            }

            if (!Directory.Exists(BackupManager.BackupFolder))
            {
                Directory.CreateDirectory(BackupManager.BackupFolder);
            }

            return retVal;
        }
    }
}
