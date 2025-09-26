using System.Windows;
using System.Windows.Controls;

namespace Finsight.Client.AppFrame.CustomControls
{
    /// <summary>
    /// Interaction logic for ExtPassBox.xaml
    /// </summary>
    public partial class ExtPassBox : UserControl
    {
        public ExtPassBox()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (MainBox.Password.Length > 0)
                Watermark.Visibility = Visibility.Collapsed;
            else 
                Watermark.Visibility = Visibility.Visible;
        }

        public void Clear() => MainBox.Clear();

        public string GetPassword() => MainBox.Password;
    }
}
