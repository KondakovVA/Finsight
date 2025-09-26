using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Finsight.Client.AppFrame.CustomControls
{
    public class ExtendedDataGrid : DataGrid
    {
        public ExtendedDataGrid()
        {
            Loaded += OnLoaded;
        }

        private readonly string ActionsHeaderTitle = Properties.Localization.ActionsLabel;

        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register(nameof(DeleteRowCommand), typeof(ICommand), typeof(ExtendedDataGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty EditCommandProperty =
            DependencyProperty.Register(nameof(EditRowCommand), typeof(ICommand), typeof(ExtendedDataGrid), new PropertyMetadata(null));

        public ICommand? DeleteRowCommand
        {
            get => (ICommand)GetValue(DeleteCommandProperty);
            set => SetValue(DeleteCommandProperty, value);
        }

        public ICommand? EditRowCommand
        {
            get => (ICommand)GetValue(EditCommandProperty);
            set => SetValue(EditCommandProperty, value);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AddActionsColumn();
        }

        private void AddActionsColumn()
        {
            if (HasColumn(ActionsHeaderTitle))
                return; // Чтобы не дублировать столбец
            var column = new DataGridTemplateColumn
            {
                Header = ActionsHeaderTitle,
                Width = 100,
                MinWidth = 70
            };

            column.CellTemplate = new DataTemplate
            {
                VisualTree = CreateActionsPanelFactory()
            };

            Columns.Add(column);
        }

        private FrameworkElementFactory CreateActionsPanelFactory()
        {
            var panelFactory = new FrameworkElementFactory(typeof(StackPanel));
            panelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            panelFactory.SetValue(StackPanel.HorizontalAlignmentProperty, HorizontalAlignment.Center);

            var deleteButtonFactory = CreateButtonFactory("DeleteIcon", DeleteButton_Click, new Thickness(0, 0, 8, 0));
            var editButtonFactory = CreateButtonFactory("EditIcon", EditButton_Click, new Thickness(0));

            panelFactory.AppendChild(deleteButtonFactory);
            panelFactory.AppendChild(editButtonFactory);

            return panelFactory;
        }

        private FrameworkElementFactory CreateButtonFactory(string iconKey, RoutedEventHandler handler, Thickness margin)
        {
            var factory = new FrameworkElementFactory(typeof(Button));
            factory.SetValue(Button.WidthProperty, 30.0);
            factory.SetValue(Button.HeightProperty, 30.0);
            factory.SetValue(Button.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            factory.SetValue(Button.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(Button.StyleProperty, Application.Current.FindResource("DeleteButtonStyle"));
            factory.SetValue(FrameworkElement.MarginProperty, margin);

            var image = Application.Current.FindResource(iconKey) as Image;
            if (image != null)
            {
                var imageFactory = new FrameworkElementFactory(typeof(Image));
                imageFactory.SetValue(Image.SourceProperty, image.Source);
                imageFactory.SetValue(Image.WidthProperty, 16.0);
                imageFactory.SetValue(Image.HeightProperty, 16.0);
                factory.AppendChild(imageFactory);
            }

            factory.AddHandler(Button.ClickEvent, handler);
            return factory;
        }

        private bool HasColumn(string headerTitle)
        {
            return Columns.Cast<DataGridColumn>().Any(column => string.Equals(column.Header?.ToString(), headerTitle));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext != null)
            {
                DeleteRowCommand?.Execute(button.DataContext);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext != null)
            {
                EditRowCommand?.Execute(button.DataContext);
            }
        }
    }
}
