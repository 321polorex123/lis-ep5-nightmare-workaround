using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LifeIsStrangeSaveEditor.Ep5Fix
{
    /// <summary>
    /// Interaction logic for BackupWindow.xaml
    /// </summary>
    public partial class BackupWindow : Window, INotifyPropertyChanged
    {
        #region View Model Properties
        private ObservableCollection<string> availableSavegames = new ObservableCollection<string>();
        public ObservableCollection<string> AvailableSavegames
        {
            get
            {
                return availableSavegames;
            }
            set
            {
                availableSavegames = value;
                OnPropertyChanged("AvailableSavegames");
            }
        }
        private string selectedSavegame = "";
        public string SelectedSavegame
        {
            get
            {
                return selectedSavegame;
            }
            set
            {
                selectedSavegame = value;
                OnPropertyChanged("SelectedSavegame");
            }
        }

        private ObservableCollection<string> availableBackups = new ObservableCollection<string>();
        public ObservableCollection<string> AvailableBackups
        {
            get
            {
                return availableBackups;
            }
            set
            {
                availableBackups = value;
                OnPropertyChanged("AvailableBackups");
            }
        }
        private string selectedBackup = "";
        public string SelectedBackup
        {
            get
            {
                return selectedBackup;
            }
            set
            {
                selectedBackup = value;
                OnPropertyChanged("SelectedBackup");
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public BackupWindow()
        {
            DataContext = this;

            InitializeComponent();

            ReloadSlots();
        }

        private void ReloadSlots()
        {
            AvailableSavegames.Clear();
            foreach (var slot in FileSystemOperations.GetSaveSlots())
            {
                AvailableSavegames.Add(string.Format("Slot {0}", slot));
            }
            if (AvailableSavegames.Count > 0)
            {
                SelectedSavegame = AvailableSavegames[0];
            }

            AvailableBackups.Clear();
            foreach (var slot in BackupManager.GetBackupedSavegameSlots())
            {
                AvailableBackups.Add(string.Format("Slot {0}", slot));
            }
            if (AvailableBackups.Count > 0)
            {
                SelectedBackup = AvailableBackups[0];
            }
        }
        
        private void BackupButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedSavegame))
            {
                var nbr = -1;

                if (int.TryParse(SelectedSavegame.Replace("Slot ", ""), out nbr))
                {
                    BackupManager.BackupSlot(nbr);
                    ReloadSlots();
                }
            }
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SelectedBackup))
            {
                var nbr = -1;

                if (int.TryParse(SelectedBackup.Replace("Slot ", ""), out nbr))
                {
                    BackupManager.RestoreSlot(nbr);
                    ReloadSlots();
                }
            }
        }

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
