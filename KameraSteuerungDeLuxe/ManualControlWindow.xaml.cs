using System.Windows;
using System.Windows.Input;

namespace KameraSteuerungDeLuxe
{
    /// <summary>
    /// Interaktionslogik für ManualControlWindow.xaml
    /// </summary>
    public partial class ManualControlWindow : Window
    {
        private AppSettings _settings;

        public ManualControlWindow(AppSettings settings)
        {
            _settings = settings;

            InitializeComponent();

            this.DataContext = new ManualControlViewModel(settings);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _settings.OpenManualWindowPositionLeft = this.Left;
            _settings.OpenManualWindowPositionTop = this.Top;
            AppSettingsManager.Save(_settings);
        }
    }
}