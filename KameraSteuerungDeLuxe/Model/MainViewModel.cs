using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace KameraSteuerungDeLuxe
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private double _height = 400;
        private ManualControlWindow? _manualWindow;

        public event PropertyChangedEventHandler? PropertyChanged;

        public AppSettings Settings { get; set; }

        public ObservableCollection<DisplayButton> Buttons => Settings.DisplayButtons;

        public RelayCommand ButtonClickCommand { get; }

        public RelayCommand ButtonPowerOffCommand { get; }

        public RelayCommand ButtonPowerOnCommand { get; }

        public RelayCommand ButtonManualMoveCommand { get; }

        public MainViewModel(AppSettings settings)
        {
            Settings = settings;
            ButtonClickCommand = new RelayCommand(OnButtonClicked);
            ButtonPowerOffCommand = new RelayCommand(PowerOff);
            ButtonPowerOnCommand = new RelayCommand(PowerOn);
            ButtonManualMoveCommand = new RelayCommand(ShowManualMoveWindow);
        }

        public bool ManualControlButtonIsEnabled
        {
            get
            {
                return Settings.ShowManualControlWindow;
            }
            set
            {
                OnPropertyChanged();
            }
        }

        private async void PowerOff(object? param)
        {
            if (Settings.PresetCameraOff.Length == 1)
            {
                await HttpHelper.CameraPosition(Settings.CameraIP, Settings.PresetCameraOff);
                // 5 Sekunden asynchron warten
                await Task.Delay(5000);
            }
            await HttpHelper.CameraPowerOff(Settings.CameraIP, Settings.CameraPort);
        }

        private async void PowerOn(object? param)
        {
            await HttpHelper.CameraPowerOn(Settings.CameraIP, Settings.CameraPort);

            if (Settings.PresetCameraOn.Length == 1)
            {
                await HttpHelper.CameraPosition(Settings.CameraIP, Settings.PresetCameraOn);
                MarkActivePreset(Settings.PresetCameraOn);
            }
        }

        private void ShowManualMoveWindow(object? param)
        {
            if (_manualWindow is null)
            {
                _manualWindow = new(Settings)
                {
                    Left = Settings.OpenManualWindowPositionLeft,
                    Top = Settings.OpenManualWindowPositionTop
                };
                _manualWindow.Show();
            }
            else
            {
                _manualWindow.Close();
                _manualWindow = null;
            }
        }

        private void MarkActivePreset(string preset)
        {
            foreach (DisplayButton b in Buttons)
            {
                b.Aktiv = b.Preset == preset;
            }
        }

        private async void OnButtonClicked(object? param)
        {
            if (param is not DisplayButton button)
                return;

            MarkActivePreset(button.Preset);

            await HttpHelper.CameraPosition(Settings.CameraIP, button.Preset);

            if (Settings.HideWindowOnClick)
            {
                await Task.Delay(2000);
                Application.Current.MainWindow?.Hide();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}