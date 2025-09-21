using Microsoft.Win32;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace KameraSteuerungDeLuxe
{
    public partial class SettingsWindow : Window
    {
        private AppSettings _settings;

        public SettingsWindow(AppSettings settings)
        {
            InitializeComponent();
            DataContext = new SettingsWindowViewModel(settings);

            _settings = settings;

            IpBox.Text = _settings.CameraIP;
            PortBox.Text = _settings.CameraPort.ToString();
            PowerOffPresetBox.Text = _settings.PresetCameraOff;
            PowerOnPresetBox.Text = _settings.PresetCameraOn;
            ShowWindowOnStartup.IsChecked = _settings.OpenOnStart;
            HideOnClick.IsChecked = _settings.HideWindowOnClick;
            ShowManualControlWindow.IsChecked = _settings.ShowManualControlWindow;

            //keine speicherung in der Settings, sondern in der Registry
            WindowsAutostart.IsChecked = AppSettingsManager.IsAutostartEnabled();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _settings.CameraIP = IpBox.Text;
            _settings.CameraPort = int.Parse(PortBox.Text);
            _settings.PresetCameraOff = PowerOffPresetBox.Text;
            _settings.PresetCameraOn = PowerOnPresetBox.Text;
            _settings.OpenOnStart = ShowWindowOnStartup.IsChecked ?? false;
            _settings.HideWindowOnClick = HideOnClick.IsChecked ?? false;
            _settings.ShowManualControlWindow = ShowManualControlWindow.IsChecked ?? false;
            AppSettingsManager.Save(_settings, WindowsAutostart.IsChecked);

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}