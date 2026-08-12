using KameraSteuerungDeLuxe.Core;
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
            InitializeComponent();
            _settings = settings;
            SpeedSlider.Value = _settings.Speed;
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

        private async void ButtonUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.up, _settings.Speed);
        }

        private async void ButtonLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.left, _settings.Speed);
        }

        private async void ButtonRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.right, _settings.Speed);
        }

        private async void ButtonDown_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.down, _settings.Speed);
        }

        private async void ButtonZoomIn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.zoomin, _settings.Speed);
        }

        private async void ButtonZoomOut_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            await HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.zoomout, _settings.Speed);
        }

        private async void ButtonAll_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            await HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.ptzstop, _settings.Speed);
            await HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.ptzstop, _settings.Speed);
        }

        private async void ButtonZoom_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            await HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.zoomstop, _settings.Speed);
            await HttpHelper.CameraMove(_settings.CameraIP, HttpHelper.MoveCommand.zoomstop, _settings.Speed);
        }

        private void SymbolFastLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SpeedSlider.Value += 1;
        }

        private void SymbolSlowLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SpeedSlider.Value -= 1;
        }

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var newValue = (int)SpeedSlider.Value;
            if (_settings != null && newValue != _settings?.Speed)
            {
                _settings.Speed = (int)SpeedSlider.Value;
                AppSettingsManager.Save(_settings);
            }
        }
    }
}