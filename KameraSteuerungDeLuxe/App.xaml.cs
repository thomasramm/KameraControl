using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using WinForms = System.Windows.Forms;
using System.Windows.Media;
using System.ComponentModel;


namespace KameraControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon? _trayIcon;
        MainWindow? mainWindow = null;
        private AppSettings _settings;

        public App()
        {
            // Lade die Einstellungen asynchron
            _settings = AppSettingsManager.Load();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _trayIcon = (TaskbarIcon)FindResource("TrayIcon");
            _trayIcon.TrayLeftMouseUp += TrayIcon_TrayLeftMouseUp;

            if (_settings.OpenOnStart)
            {
                MainWindowShow();
            }
        }

        private void TrayIcon_TrayLeftMouseUp(object? sender, RoutedEventArgs e)
        {
            MainWindowShow();
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            if (mainWindow != null)
            {
                _settings.OpenPositionX = mainWindow.Left;
                _settings.OpenPositionY = mainWindow.Top;
                mainWindow.Closing -= MainWindow_Closing;
                mainWindow = null;
            }

            _trayIcon?.Dispose();
            AppSettingsManager.Save(_settings);
            Shutdown(); // Beendet die gesamte App
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _trayIcon?.Dispose();
            base.OnExit(e);
        }

        private void MenuItem_ToggleWindow_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow == null)
            {
                MainWindowShow();
            }
            else
            {
                if (mainWindow.Visibility == Visibility.Visible)
                    mainWindow.Hide();
                else
                    mainWindow.Show();
            }
        }

        private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            // Beispiel: ein Settings-Fenster öffnen
            var settingsWindow = new SettingsWindow(_settings);
            settingsWindow.ShowDialog();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            _trayIcon?.Dispose();
            Shutdown();
        }

        void MainWindowShow()
        {
            if (mainWindow == null || !mainWindow.IsVisible)
            {
                if (mainWindow == null)
                {
                    mainWindow = new MainWindow(_settings);
                    mainWindow.Closing += MainWindow_Closing;

                    mainWindow.Left = _settings.OpenPositionX;
                    mainWindow.Top = _settings.OpenPositionY;
                }

                mainWindow.Show();
                mainWindow.Activate();

            }
            else
            {
                mainWindow.Hide();
            }
        }
    }
}

