using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KameraSteuerungDeLuxe
{
    /// <summary>
    /// Interaktionslogik für ManualControlWindow.xaml
    /// </summary>
    public partial class ManualControlWindow : Window
    {
        AppSettings _settings;
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
