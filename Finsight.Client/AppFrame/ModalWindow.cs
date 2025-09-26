using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace Finsight.Client.AppFrame
{
    public class ModalWindow : Window
    {
        private Window? _owner;
        private Effect? _previousOwnerEffect;
        private bool _ownerWasEnabled = true;

        private ModalWindow()
        {
            Closed += OnClosed;
        }

        public static ModalWindow Show(ContentControl content, string title)
        {
            ArgumentNullException.ThrowIfNull(content);

            if (Window.GetWindow(content) is ModalWindow existingWindow)
            {
                existingWindow.Title = title;
                if (existingWindow.IsVisible)
                {
                    existingWindow.Activate();
                }

                return existingWindow;
            }

            var owner = Application.Current?.MainWindow;

            var modalWindow = new ModalWindow
            {
                WindowStartupLocation = owner != null
                    ? WindowStartupLocation.CenterOwner
                    : WindowStartupLocation.CenterScreen,
                Title = title,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                Content = content,
                Width = content.Width,
                Height = content.Height,
                Owner = owner,
                ShowInTaskbar = false
            };
            // —брасываем скроллы в начало при открытии окна
            void OnContentRendered(object? sender, EventArgs args)
            {
                ResetScrollOffsets(content);

                if (sender is ModalWindow window)
                {
                    window.ContentRendered -= OnContentRendered;
                }
            }

            modalWindow.ContentRendered += OnContentRendered;
            modalWindow.ApplyOwnerState();
            modalWindow.Show();
            modalWindow.Activate();

            return modalWindow;
        }

        public static bool IsOpen(ContentControl content)
        {
            return Window.GetWindow(content) is ModalWindow window && window.IsVisible;
        }

        public static void Close(ContentControl content)
        {
            if (Window.GetWindow(content) is ModalWindow window)
            {
                window.Close();
            }
        }

        private void ApplyOwnerState()
        {
            _owner = Owner ?? Application.Current?.MainWindow;
            if (_owner == null)
            {
                return;
            }

            _ownerWasEnabled = _owner.IsEnabled;
            _previousOwnerEffect = _owner.Effect;

            _owner.IsEnabled = false;
            _owner.Effect = new BlurEffect { Radius = 7 };
        }

        private void OnClosed(object? sender, EventArgs e)
        {
            if (_owner != null)
            {
                _owner.Effect = _previousOwnerEffect;
                _owner.IsEnabled = _ownerWasEnabled;

                if (_owner.IsVisible && _ownerWasEnabled)
                {
                    _owner.Activate();
                }
            }

            Content = null;
            Closed -= OnClosed;
        }

        private static void ResetScrollOffsets(DependencyObject element)
        {
            if (element == null)
            {
                return;
            }

            if (element is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToHome();
                scrollViewer.ScrollToVerticalOffset(0);
                scrollViewer.ScrollToHorizontalOffset(0);
            }

            if (element is ContentControl { Content: DependencyObject content })
            {
                ResetScrollOffsets(content);
            }

            if (element is not Visual && element is not Visual3D)
            {
                return;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                ResetScrollOffsets(VisualTreeHelper.GetChild(element, i));
            }
        }
    }
}
