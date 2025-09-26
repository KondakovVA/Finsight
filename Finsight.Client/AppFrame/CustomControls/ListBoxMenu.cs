using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Finsight.Client.AppFrame.CustomControls
{
    public class ListBoxMenu : ListBox
    {
        // Создание DependencyProperty для привязки команды
        public static readonly DependencyProperty OnSelectionChangedCommandProperty =
            DependencyProperty.Register(
                "OnSelectionChangedCommand",
                typeof(ICommand),
                typeof(ListBoxMenu),
                new PropertyMetadata(null));

        public ICommand OnSelectionChangedCommand
        {
            get => (ICommand)GetValue(OnSelectionChangedCommandProperty);
            set => SetValue(OnSelectionChangedCommandProperty, value);
        }

        // Переопределение метода для отслеживания изменения выбора
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            // Проверяем, если команда привязана и выбран новый элемент
            if (OnSelectionChangedCommand != null && e.AddedItems.Count > 0)
            {
                // Передаем выбранный элемент в команду
                if (OnSelectionChangedCommand.CanExecute(e.AddedItems[0]))
                {
                    OnSelectionChangedCommand.Execute(e.AddedItems[0]);
                }
            }
        }
    }
}
