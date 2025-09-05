using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace KameraControl
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private double _height = 400;

        public event PropertyChangedEventHandler? PropertyChanged;

        public AppSettings Settings { get; set; }

        public ObservableCollection<DisplayButton> Buttons => Settings.DisplayButtons;

        public Orientation Ausrichtung => Settings.WindowIsHorizontalDesign ? Orientation.Horizontal : Orientation.Vertical;

        public DisplayButton DayPresetButton 
        {
            get => Settings.DayPresetButton;
            set
            {
                if (Settings.DayPresetButton != value)
                {
                    Settings.DayPresetButton = value;
                    OnPropertyChanged(nameof(DayPresetButton));
                }
            }
        }

        public RelayCommand ButtonClickCommand { get; }

        public RelayCommand ButtonPowerOffCommand { get; }

        public RelayCommand ButtonPowerOnCommand { get; }

        public MainViewModel(AppSettings settings)
        {
            Settings = settings;
            ButtonClickCommand = new RelayCommand(OnButtonClicked);
            ButtonPowerOffCommand = new RelayCommand(PowerOff);
            ButtonPowerOnCommand = new RelayCommand(PowerOn);
        }

        public void SaveSettings()
        {
            AppSettingsStorage.Save(Settings);
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
                await HttpHelper.CameraPosition(Settings.CameraIP, Settings.PresetCameraOn);
        }

        private int _currentPreset = 0;

        private async void OnButtonClicked(object? param)
        {
            if (param is not DisplayButton button)
                return;

            foreach (DisplayButton b in Buttons)
            {
                b.Aktiv = b.Preset == button.Preset;
            }

            await HttpHelper.CameraPosition(Settings.CameraIP, button.Preset);

            if (Settings.HideWindowOnClick)
            {
                await Task.Delay(2000);
                Application.Current.MainWindow?.Hide();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
