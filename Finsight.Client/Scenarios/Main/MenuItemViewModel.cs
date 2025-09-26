using System.Windows.Input;
using Finsight.Client.AppFrame;
using FontAwesome.WPF;

namespace Finsight.Client.Scenarios.Main
{
    public class MenuItemViewModel : ViewModelBase
    {
        private readonly Guid _id;
        /// <summary>
        /// Id для открытия
        /// </summary>
        public Guid Id => _id;
        public string Title { get; }
        public IPresenter Presenter { get; }
        public FontAwesomeIcon MenuIcon { get;  }
        public ICommand MenuBtnPressCommand { get; }
        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }
        public MenuItemViewModel(string title, IPresenter presenter, FontAwesomeIcon icon, ICommand pressCommand)
        {
            _id = Guid.NewGuid();
            Title = title;
            Presenter = presenter;
            MenuIcon = icon;
            MenuBtnPressCommand = pressCommand;
        }

    }
}
