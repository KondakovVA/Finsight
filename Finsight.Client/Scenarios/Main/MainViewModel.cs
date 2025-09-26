using System.Collections.ObjectModel;
using System.Windows.Controls;
using Finsight.Client.AppFrame;
using FontAwesome.WPF;

namespace Finsight.Client.Scenarios.Main
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<MenuItemViewModel> MenuItems { get; } = [];

        private string? _displayName;
        public string? DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        private ContentControl? _currentView;
        public ContentControl? CurrentView
        {
            get => _currentView;
            set
            {
                if (SetProperty(ref _currentView, value))
                {
                    OnPropertyChanged(nameof(Icon));
                    OnPropertyChanged(nameof(Caption));
                }
            }
        }

        public string? Caption => SelectedMenuItem?.Title;

        public FontAwesomeIcon? Icon => SelectedMenuItem?.MenuIcon;

        public MenuItemViewModel? SelectedMenuItem { get => MenuItems.FirstOrDefault(s => s.IsChecked); }
        
    }
}
