using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LifeIsStrangeSaveEditor.Ep5Fix
{
    internal static class FileSystemOperations
    {
        private static string saveGameFolder = "";
        public static string SaveGameFolder
        {
            get
            {
                var path = saveGameFolder;
                if (string.IsNullOrEmpty(path))
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    path = Path.Combine(path, "my games", "Life Is Strange", "Saves");
                }
                return path;
            }
        }

        public static void SetSaveGameFolder(string path)
        {
            saveGameFolder = path;
        }

        public static int[] GetSaveSlots()
        {
            var files = Directory.EnumerateFiles(SaveGameFolder, "LISSave*.sav");
            var slots = new List<int>();

            foreach (var file in files)
            {
                var nbrStr = Path.GetFileName(file).Replace("LISSave", "").Replace(".sav", "");
                var nbr = -1;

                if (int.TryParse(nbrStr, out nbr))
                {
                    slots.Add(nbr);
                }
            }

            return slots.ToArray();
        }
    }
}
