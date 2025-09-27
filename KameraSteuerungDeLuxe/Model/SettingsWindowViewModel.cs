using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KameraSteuerungDeLuxe
{
    public class SettingsWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly AppSettings _settings;

        public SettingsWindowViewModel(AppSettings settings)
        {
            _settings = settings;
        }

        public ObservableCollection<DisplayButton> Buttons => _settings.DisplayButtons;

        public ObservableCollection<string> AvailableIcons { get; set; } = new()
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

        public ObservableCollection<string> AvailablePresets { get; set; } = new()
        {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
        };

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}