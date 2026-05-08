using KameraSteuerungDeLuxe.Core;
using System.Windows;

namespace KameraSteuerungDeLuxe
{
    public partial class SettingsWindow : Window
    {
        private SettingsWindowViewModel _viewModel;
        private AppSettings _settings;

        public SettingsWindow(AppSettings settings)
        {
            InitializeComponent();
            _settings = settings;
            _viewModel = new SettingsWindowViewModel(settings);
            DataContext = _viewModel;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            AppSettingsManager.Save(_settings, _viewModel.ShowWindowOnStartup);
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private async void SearchCamera_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.SearchCameras();
        }
    }
}