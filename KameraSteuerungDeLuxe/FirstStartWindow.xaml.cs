using System.Windows;

namespace KameraSteuerungDeLuxe
{
    /// <summary>
    /// Interaktionslogik für FirstStartWindow.xaml
    /// </summary>
    public partial class FirstStartWindow : Window
    {
        public FirstStartWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}