using Finsight.Client.AppFrame;
using Finsight.Client.DI;
using Finsight.Client.Extensions;
using Finsight.Contract.Enum;

namespace Finsight.Client.Scenarios.Home
{
    internal class HomePresenter : PresenterBase<HomeView, HomeViewModel>
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IUserRepo _userRepo;
        private readonly ICustomerRepo _customerRepo;

        public HomePresenter(IOrderRepo orderRepo, IUserRepo userRepo, ICustomerRepo customerRepo)
            : base(new HomeView(), new HomeViewModel { ViewTitle = "Главная" })
        {
            _orderRepo = orderRepo;
            _userRepo = userRepo;
            _customerRepo = customerRepo;
        }

        private async Task LoadOrdersAsync()
        {
            var orders = await _orderRepo.GetAll();
            ViewModel.Orders.Clear();
            ViewModel.Orders.ReplaceWith(orders
                .Where(o => o.Status != OrderStatus.Rejected && o.Status != OrderStatus.Complete)
                .OrderBy(o => o.Customer?.CompanyName));
            ViewModel.TotalIncome = $"{orders.Where(o => o.Status == OrderStatus.Complete).Sum(o => o.Price)} р.";
        }

        private async Task LoadUsersAsync()
        {
            var users = await _userRepo.GetAllUsers();
            ViewModel.Users.Clear();
            ViewModel.Users.ReplaceWith(users.OrderBy(u => u.DisplayName));
        }

        private async Task LoadCustomersAsync()
        {
            var users = await _customerRepo.GetAll();
            ViewModel.Customers.Clear();
            ViewModel.Customers.ReplaceWith(users.OrderBy(u => u.CompanyName));
        }

        protected override async Task OnActivateAsync(bool isInitialActivation)
        {
            await Task.WhenAll(
                LoadOrdersAsync(),
                LoadUsersAsync(),
                LoadCustomersAsync());
        }
    }
}
