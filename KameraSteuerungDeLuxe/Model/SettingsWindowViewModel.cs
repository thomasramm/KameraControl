using KameraSteuerungDeLuxe.Core;
using KameraSteuerungDeLuxe.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace KameraSteuerungDeLuxe
{
    public class SettingsWindowViewModel : INotifyPropertyChanged
    {
        private readonly AppSettings _settings;

        private Camera? _selectedCamera;

        private bool _showWindowOnStartup;
        private bool searchIsIdle = true;

        public SettingsWindowViewModel(AppSettings settings)
        {
            _settings = settings;
            ShowWindowOnStartup = AppSettingsManager.IsAutostartEnabled();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Camera> AvailableCameras { get; set; } = new();

        public bool AvailableCamerasAny => AvailableCameras.Any();

        public List<string> AvailableIcons { get; set; } = new()
        {
            "Bühne 01",
            "Bühne 02",
            "Bühne 03",
            "Bühne 04",
            "2 Personen 01",
            "2 Personen 02",
            "2 Personen 03",
            "2 Personen 04",
            "Redner 01",
            "Redner 02",
            "Redner 03",
            "Redner 04",
            "Redner 05",
            "Redner Leser 01",
            "Redner Leser 02",
            "Stil B 01",
            "Stil B 02",
            "Stil B 03",
            "Stil B 04",
            "Stil B 05",
            "Stil B 06",
            "Tisch 01",
            "Tisch 02",
            "Tisch 03",
            "Tisch 04",
            "Kamera An",
            "Kamera Aus",
        };

        public List<string> AvailablePresets { get; set; } = new()
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
        };

        public ObservableCollection<DisplayButton> Buttons => _settings.DisplayButtons;

        public string CameraIp
        {
            get
            {
                return _settings.CameraIP;
            }
            set
            {
                _settings.CameraIP = value;
                OnPropertyChanged();
            }
        }

        public int CameraPort
        {
            get
            {
                return _settings.CameraPort;
            }
            set
            {
                _settings.CameraPort = value;
                OnPropertyChanged();
            }
        }

        public bool HideWindowOnClick
        {
            get
            {
                return _settings.HideWindowOnClick;
            }
            set
            {
                _settings.HideWindowOnClick = value;
                OnPropertyChanged();
            }
        }

        public bool OpenOnStart
        {
            get
            {
                return _settings.OpenOnStart;
            }
            set
            {
                _settings.OpenOnStart = value;
                OnPropertyChanged();
            }
        }

        public string PresetCameraOff
        {
            get
            {
                return _settings.PresetCameraOff;
            }
            set
            {
                _settings.PresetCameraOff = value;
                OnPropertyChanged();
            }
        }

        public string PresetCameraOn
        {
            get
            {
                return _settings.PresetCameraOn;
            }
            set
            {
                _settings.PresetCameraOn = value;
                OnPropertyChanged();
            }
        }

        public bool SearchIsIdle
        {
            get => searchIsIdle;
            set
            {
                searchIsIdle = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SearchIsWorking));
            }
        }
        public bool SearchIsWorking => !SearchIsIdle;

        public Camera? SelectedCamera
        {
            get
            {
                return _selectedCamera;
            }
            set
            {
                _selectedCamera = value;
                if (_selectedCamera != null)
                {
                    CameraIp = _selectedCamera.Address;
                    //CameraPort = uri.Port;
                }
                OnPropertyChanged();
            }
        }

        public bool ShowManualControlWindow
        {
            get
            {
                return _settings.ShowManualControlWindow;
            }
            set
            {
                _settings.ShowManualControlWindow = value;
                OnPropertyChanged();
            }
        }

        public bool ShowWindowOnStartup
        {
            get
            {
                return _showWindowOnStartup;
            }
            set
            {
                _showWindowOnStartup = value;
                OnPropertyChanged();
            }
        }

        public async Task SearchCameras()
        {
            SearchIsIdle = false;
            AvailableCameras.Clear();

            var cameras = await HttpHelper.SearchCamera();

            foreach (Camera c in cameras)
            {
                AvailableCameras.Add(c);
            }
            if (AvailableCamerasAny)
                SelectedCamera = AvailableCameras[0];
            else
                MessageBox.Show("Keine Kamera gefunden, bitte IP Adresse manuell eingeben,", "Suche erfolglos", MessageBoxButton.OK, MessageBoxImage.Warning);

            OnPropertyChanged(nameof(AvailableCamerasAny));
            SearchIsIdle = true;
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}