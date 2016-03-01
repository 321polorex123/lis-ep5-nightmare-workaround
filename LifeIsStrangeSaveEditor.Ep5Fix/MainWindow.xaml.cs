using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LifeIsStrangeSaveEditor.Ep5Fix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Constants
        private const string ToDoStepSymbol = "û";
        private const string DoneStepSymbol = "ü";
        private static readonly Brush ToDoStepColor = Brushes.Red;
        private static readonly Brush DoneStepColor = Brushes.Green;
        #endregion
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
        private string selectedSafegame = "";
        public string SelectedSafegame
        {
            get
            {
                return selectedSafegame;
            }
            set
            {
                selectedSafegame = value;
                OnPropertyChanged("SelectedSafegame");
            }
        }
        private bool firstStepActive = false;
        public bool FirstStepActive
        {
            get { return firstStepActive; }
        }
        private bool secondStepActive = false;
        public bool SecondStepActive
        {
            get { return secondStepActive; }
        }
        private bool thirdStepActive = false;
        public bool ThirdStepActive
        {
            get { return thirdStepActive; }
        }
        private bool fourthStepActive = false;
        public bool FourthStepActive
        {
            get { return fourthStepActive; }
        }
        private string firstStepSymbol = ToDoStepSymbol;
        public string FirstStepSymbol
        {
            get { return firstStepSymbol; }
        }
        private Brush firstStepColor = ToDoStepColor;
        public Brush FirstStepColor
        {
            get { return firstStepColor; }
        }
        private string secondStepSymbol = ToDoStepSymbol;
        public string SecondStepSymbol
        {
            get { return secondStepSymbol; }
        }
        private Brush secondStepColor = ToDoStepColor;
        public Brush SecondStepColor
        {
            get { return secondStepColor; }
        }
        private string thirdStepSymbol = ToDoStepSymbol;
        public string ThirdStepSymbol
        {
            get { return thirdStepSymbol; }
        }
        private Brush thirdStepColor = ToDoStepColor;
        public Brush ThirdStepColor
        {
            get { return thirdStepColor; }
        }
        #endregion

        private int selectedSlot = -1;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            DataContext = this;

            InitializeComponent();

            LoadAvailableSavegames();
            SwitchStep(1);
        }

        private void SwitchStep(int step)
        {
            firstStepActive = (step == 1);
            secondStepActive = (step == 2);
            thirdStepActive = (step == 3);
            fourthStepActive = (step == 4);

            firstStepSymbol = (step < 2) ? ToDoStepSymbol : DoneStepSymbol;
            secondStepSymbol = (step < 3) ? ToDoStepSymbol : DoneStepSymbol;
            thirdStepSymbol = (step < 4) ? ToDoStepSymbol : DoneStepSymbol;

            firstStepColor = (step < 2) ? ToDoStepColor : DoneStepColor;
            secondStepColor = (step < 3) ? ToDoStepColor : DoneStepColor;
            thirdStepColor = (step < 4) ? ToDoStepColor : DoneStepColor;

            OnPropertyChanged("FirstStepActive");
            OnPropertyChanged("SecondStepActive");
            OnPropertyChanged("ThirdStepActive");
            OnPropertyChanged("FourthStepActive");
            OnPropertyChanged("FirstStepSymbol");
            OnPropertyChanged("SecondStepSymbol");
            OnPropertyChanged("ThirdStepSymbol");
            OnPropertyChanged("FirstStepColor");
            OnPropertyChanged("SecondStepColor");
            OnPropertyChanged("ThirdStepColor");
        }

        private void LoadAvailableSavegames()
        {
            var slots = FileSystemOperations.GetSaveSlots();

            if (slots.Length > 0)
            {
                AvailableSavegames.Add("Choose a slot");
                slots.ToList().ForEach(i => AvailableSavegames.Add(string.Format("Slot {0}", i)));
            }
            else
            {
                AvailableSavegames.Add("No slots found");
            }

            SelectedSafegame = AvailableSavegames[0];
        }

        private void SafegamesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedSafegame != AvailableSavegames[0])
            {
                var nbr = -1;

                if (int.TryParse(SelectedSafegame.Replace("Slot ", ""), out nbr))
                {
                    selectedSlot = nbr;

                    SwitchStep(2);
                }
            }
        }

        private void BackupButton_Click(object sender, RoutedEventArgs e)
        {
            if (BackupManager.BackupSlot(selectedSlot))
            {
                SwitchStep(3);
            }
        }

        private void PatchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SafegamePatcher.PatchSafegame(selectedSlot))
            {
                SwitchStep(4);
            }
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("steam://run/319630");
        }

        private void BckManagerButton_Click(object sender, RoutedEventArgs e)
        {
            new BackupWindow().ShowDialog();
            LoadAvailableSavegames();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
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
