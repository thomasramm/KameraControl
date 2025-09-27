using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Xml.Serialization;

namespace KameraSteuerungDeLuxe
{
    [Serializable]
    public class DisplayButton : INotifyPropertyChanged
    {
        private string _icon = string.Empty;
        private string _preset = string.Empty;
        private string _name = string.Empty;
        private bool _aktiviert = true;
        private bool _aktiv = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Position { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IconUrl));
            }
        }

        public bool Aktiviert
        {
            get => _aktiviert;
            set
            {
                _aktiviert = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        public bool Aktiv
        {
            get => _aktiv;
            set
            {
                _aktiv = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Background));
            }
        }

        [XmlIgnore]
        public SolidColorBrush Background
        {
            get
            {
                //return (SolidColorBrush)new BrushConverter().ConvertFrom(Aktiv ? "#3597d0" : "#226084");
                return new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(Aktiv ? "#c94453" : "#226084"));
            }
        }

        [XmlIgnore]
        public string IconUrl
        {
            get => $"images/{_icon}.png";
        }

        public string Preset
        {
            get => _preset;
            set
            {
                _preset = value;
                OnPropertyChanged();
            }
        }

        public DisplayButton()
        { }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}