using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LifeIsStrangeSaveEditor.Ep5Fix
{
    public static class BackupManager
    {
        public static string BackupFolder
        {
            get
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                path = Path.Combine(path, "LiSBackup");
                return path;
            }
        }

        public static int[] GetBackupedSavegameSlots()
        {
            var slots = new List<int>();
            var files = Directory.EnumerateDirectories(BackupFolder, "Slot*");

            foreach (var file in files)
            {
                var nbrStr = Path.GetFileName(file).Replace("Slot", "");
                var nbr = -1;

                if (int.TryParse(nbrStr, out nbr))
                {
                    slots.Add(nbr);
                }
            }

            return slots.ToArray();
        }

        public static DateTime GetDateTimeOfBackup(int slot)
        {
            var path = Path.Combine(BackupFolder, string.Format("Slot{0}", slot));
            var timestamp = DateTime.Now;

            if (Directory.Exists(path))
            {
                timestamp = Directory.GetCreationTime(path);
            }

            return timestamp;
        }

        public static bool BackupSlot(int slot)
        {
            var retVal = true;
            var dstFolder = Path.Combine(BackupFolder, string.Format("Slot{0}", slot));

            if (Directory.Exists(dstFolder))
            {
                var msg = string.Format(
                    "The savegame slot {0} has already been backuped at {1}.\n" +
                    "Do you want to overwrite this backup?",
                    slot,
                    GetDateTimeOfBackup(slot)
                    );

                if (!MsgBox.ShowQuestion(msg))
                {
                    retVal = false;
                }
            }

            if (retVal)
            {
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
                    if (Directory.Exists(dstFolder))
                    {
                        Directory.Delete(dstFolder, true);
                    }
                    Directory.CreateDirectory(dstFolder);
                    File.Copy(file1, Path.Combine(dstFolder, Path.GetFileName(file1)));
                    File.Copy(file2, Path.Combine(dstFolder, Path.GetFileName(file2)));
                }
                else
                {
                    MsgBox.ShowWarning("Some files are missing!\nCannot backup files.");
                    retVal = false;
                }
            }

            return retVal;
        }

        public static bool RestoreSlot(int slot, bool forceOverride = false)
        {
            var retVal = true;
            var srcFolder = Path.Combine(BackupFolder, string.Format("Slot{0}", slot));
            var file1 = Path.Combine(
                srcFolder,
                string.Format("LISOptions{0}.sav", slot)
                );
            var file2 = Path.Combine(
                srcFolder,
                string.Format("LISSave{0}.sav", slot)
                );
            var targetFile1 = Path.Combine(
                FileSystemOperations.SaveGameFolder,
                Path.GetFileName(file1)
                );
            var targetFile2 = Path.Combine(
                FileSystemOperations.SaveGameFolder,
                Path.GetFileName(file2)
                );

            if (!Directory.Exists(FileSystemOperations.SaveGameFolder))
            {
                MsgBox.ShowWarning("Could not find the savegame folder!");
                retVal = false;
            }

            if (retVal)
            {
                if (File.Exists(file1) && File.Exists(file2))
                {
                    if (File.Exists(targetFile1) || File.Exists(targetFile2))
                    {
                        if (!forceOverride)
                        {
                            var msg = string.Format(
                            "Do you want to restore the slot {0}?\n" +
                            "This process will override your current slot with the backup created at {1}.\n" +
                            "This process cannot be undone!",
                            slot,
                            GetDateTimeOfBackup(slot)
                            );

                            if (!MsgBox.ShowQuestion(msg))
                            {
                                retVal = false;
                            }
                        }
                    }
                }
                else
                {
                    MsgBox.ShowWarning("Some files are missing!\nCannot recover backup.");
                    retVal = false;
                }

                if (retVal)
                {
                    File.Delete(targetFile1);
                    File.Delete(targetFile2);
                    File.Copy(file1, targetFile1);
                    File.Copy(file2, targetFile2);

                    MsgBox.ShowInfo("Backup successfully restored.");
                }
            }

            return retVal;
        }
    }
}
