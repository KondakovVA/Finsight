using System.Windows;
using System.Windows.Controls;

namespace Finsight.Client.Scenarios.Users.Card
{
    /// <summary>
    /// Interaction logic for UserCardView.xaml
    /// </summary>
    public partial class UserCardView : UserControl
    {
        public UserCardView()
        {
            InitializeComponent();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            //if (DataContext is UserCardViewModel)
            //{
            //    ((UserCardViewModel) DataContext).Password = PasswordBox.Password;
            //}
        }
    }
}
