using LifeIsStrangeSaveEditor.Win;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LifeIsStrangeSaveEditor.Ep5Fix
{
    internal static class SafegamePatcher
    {
        public static bool PatchSafegame(int slot)
        {
            var retVal = true;
            var file1 = Path.Combine(
                FileSystemOperations.SaveGameFolder,
                string.Format("LISOptions{0}.sav", slot)
                );
            var file2 = Path.Combine(
                FileSystemOperations.SaveGameFolder,
                string.Format("LISSave{0}.sav", slot)
                );

            if (File.Exists(file1) && File.Exists(file2))
            {
                var msg = string.Format(
                    "Do you want to patch safegame slot {0}?\n" +
                    "This step cannot be undone!",
                    slot
                    );

                if (!MsgBox.ShowQuestion(msg))
                {
                    retVal = false;
                }
            }
            else
            {
                MsgBox.ShowWarning("Cannot patch the safegame.\nSome files are missing.");
                retVal = false;
            }

            if (retVal)
            {
                retVal = Patch(file2, slot);
            }

            return retVal;
        }

        private static bool Patch(string path, int slot)
        {
            var retVal = true;
            var api = new MainForm();

            api.SaveFilePath = path;
            try
            {
                retVal = api.LoadFile();
            }
            catch (Exception e)
            {
                ExceptionRaised(e, slot);
                retVal = false;
            }

            if (retVal)
            {
                // Level start states
                api.dgvSubLevelStartStates.Rows.Add("Episode5Sub16", string.Empty);
                // Reached checkpoints
                api.dgvCheckpointReached.Rows.Add("E5_7A_CP01_Insertion");
                // Misc
                api.cbCheckPointID.Text = "E5_7A_CP01_Insertion";
                api.tbCheckpointLocationX.Text = "-150.0391";
                api.tbCheckpointLocationY.Text = "-36.35254";
                api.tbCheckpointLocationZ.Text = "257.7305";
                api.tbCheckpointRotationPitch.Text = "0";
                api.tbCheckpointRotationYaw.Text = "36864";
                api.tbCheckpointRotationRoll.Text = "0";

                try
                {
                    api.btnSave_Click(null, new EventArgs());
                }
                catch (Exception e)
                {
                    ExceptionRaised(e, slot);
                    retVal = false;
                }
            }

            return retVal;
        }

        private static void ExceptionRaised(Exception e, int slot)
        {
            File.WriteAllText(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "LiSPatcherError.txt"),
                "Error:\n" + e.Message + "\n\n" +
                "Stack Trace:\n" + e.StackTrace
                );

            BackupManager.RestoreSlot(slot, true);

            MsgBox.ShowWarning(
                "An error occured while patching the safegame.\n" +
                "The support file \"LiSPatcherError.txt\" has been created on the desktop, " +
                "please post it in the Steam thread in order to help the error get fixed.\n" +
                "Backup will be restored."
                );

        }
    }
}
