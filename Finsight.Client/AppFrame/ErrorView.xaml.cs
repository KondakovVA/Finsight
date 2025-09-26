using System.Windows;
using System.Windows.Input;

namespace Finsight.Client.AppFrame
{
    /// <summary>
    /// Interaction logic for ErrorView.xaml
    /// </summary>
    public partial class ErrorView : Window
    {
        public ErrorView(string message, string stackTrace = null)
        {
            InitializeComponent();
            ErrorMessage.Text = message;
            // Если передан stacktrace, то показываем его в скрытом поле
            if (!string.IsNullOrEmpty(stackTrace))
            {
                StackTraceText.Text = stackTrace;
            }
        }

        // Обработчик кнопки "Подробнее"
        private void OnShowDetailsClick(object sender, RoutedEventArgs e)
        {
            // Переключаем видимость StackTraceText
            if (StackTraceText.Visibility == Visibility.Collapsed)
            {
                StackTraceText.Visibility = Visibility.Visible;
            }
            else
            {
                StackTraceText.Visibility = Visibility.Collapsed;
            }
        }

        private void ErrorView_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MinimizeBtn_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
