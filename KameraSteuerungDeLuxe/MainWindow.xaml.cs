using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace KameraControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public AppSettings _settings;
        public MainViewModel _model;

        public MainWindow(AppSettings settings)
        {
            _settings = settings;

            InitializeComponent();
            _model = new MainViewModel(settings);
            DataContext = _model;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _model.Settings.OpenPositionX = this.Left;
            _model.Settings.OpenPositionY = this.Top;
        }
    }
}
