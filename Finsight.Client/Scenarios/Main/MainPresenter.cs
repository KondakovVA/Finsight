using Finsight.Client.AppFrame;
using Finsight.Client.Extensions;
using Finsight.Client.Properties;
using Finsight.Client.Scenarios.Analytics;
using Finsight.Client.Scenarios.Customers;
using Finsight.Client.Scenarios.Finance;
using Finsight.Client.Scenarios.Home;
using Finsight.Client.Scenarios.Orders;
using Finsight.Client.Scenarios.Users;
using Finsight.Client.Services;
using Finsight.Client.Utils;
using FontAwesome.WPF;

namespace Finsight.Client.Scenarios.Main
{
    public class MainPresenter
    {
        private readonly MainView _view;
        private readonly MainViewModel _viewModel;
        private readonly IRoleViewManager _roleManager;

        public MainPresenter(IRoleViewManager roleManager)
        {
            _roleManager = roleManager;
            _viewModel = new MainViewModel();
            _view = new MainView { DataContext = _viewModel };
        }

        public async Task ShowAsync()
        {
            _viewModel.DisplayName = _roleManager.GetCurrentUsername();
            _view.Show();
            await BuildMainMenuAsync();
        }

        private MenuItemViewModel CreateMenuItemViewModel(
            string title,
            IPresenter presenter,
            FontAwesomeIcon icon)
        {
            return new MenuItemViewModel(
                title,
                presenter,
                icon,
                new DelegateCommand<IPresenter>(async presenterBase => await ActivatePresenterAsync(presenterBase)));
        }

        private MenuItemViewModel GetViewModel(IPresenter presenterBase)
        {
            switch (presenterBase)
            {
                case CustomersPresenter customers:
                {
                    return CreateMenuItemViewModel(Localization.CustomersLabel, customers, FontAwesomeIcon.Users);
                }
                case OrdersPresenter orders:
                {
                    return CreateMenuItemViewModel(Localization.OrdersLabel, orders, FontAwesomeIcon.Cubes);
                }
                case UsersPresenter users:
                {
                    return CreateMenuItemViewModel(Localization.EmployeesLabel, users, FontAwesomeIcon.UserMd);
                }
                case HomePresenter home:
                {
                    return CreateMenuItemViewModel(Localization.MyCompanyLabel, home, FontAwesomeIcon.Home);
                }
                case AnalyticPresenter analytic:
                {
                    return CreateMenuItemViewModel(Localization.AnalyticLabel, analytic, FontAwesomeIcon.LineChart);
                }
                case FinancePresenter finance:
                {
                    return CreateMenuItemViewModel(Localization.FinanceLabel, finance, FontAwesomeIcon.Money);
                }
                default:
                    throw new Exception(string.Format(Localization.PresenterNotRegisteredError, presenterBase.GetType()));
            }
        }

        private async Task BuildMainMenuAsync()
        {
            var items = _roleManager.GetMenuItems();
            _viewModel.MenuItems.AddRange(items.Select(GetViewModel));
            //Ставим вью для первой загрузки
            await SetDefaultViewAsync();
        }

        private async Task SetDefaultViewAsync()
        {
            var firstItem = _viewModel.MenuItems.FirstOrDefault();
            if (firstItem == null)
            {
                return;
            }

            //Для первой загрузки ставим у кнопки IsChecked вручную
            firstItem.IsChecked = true;
            await ActivatePresenterAsync(firstItem.Presenter);
        }

        private async Task ActivatePresenterAsync(IPresenter presenter)
        {
            if (!ReferenceEquals(_viewModel.CurrentView, presenter.ViewContent))
            {
                await presenter.ActivateAsync();
                _viewModel.CurrentView = presenter.ViewContent;
            }
        }
    }
}
