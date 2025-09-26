using Finsight.Client.AppFrame;
using Finsight.Client.DI;
using Finsight.Client.Extensions;
using Finsight.Client.Model;
using Finsight.Client.Scenarios.Orders.Card;
using Finsight.Client.Utils;
using Finsight.Contract.Dto;
using Finsight.Contract.Enum;
using System.Diagnostics;
using System.Windows;
using Localization = Finsight.Client.Properties.Localization;

namespace Finsight.Client.Scenarios.Orders
{
    public class OrdersPresenter : PresenterBase<OrdersView, OrdersViewModel>
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly IUserRepo _userRepo;
        private readonly OrderCardPresenter _cardPresenter;

        public OrdersPresenter(IOrderRepo orderRepo, ICustomerRepo customerRepo, IUserRepo userRepo, OrderCardPresenter cardPresenter)
            : base(new OrdersView(), new OrdersViewModel { ViewTitle = Localization.OrdersLabel })
        {
            _orderRepo = orderRepo;
            _customerRepo = customerRepo;
            _userRepo = userRepo;
            _cardPresenter = cardPresenter;
            _cardPresenter.OrderCreated = OnOrderCreatedAsync;
            _cardPresenter.OrderUpdated = OnOrderUpdatedAsync;
            _cardPresenter.UploadDocumentsHandler = UploadDocumentsAsync;

            ViewModel.AddOrderCommand = new DelegateCommand(AddOrder);
            ViewModel.DeleteOrderCommand = new DelegateCommand(DeleteOrder);
            ViewModel.EditOrderCommand = new DelegateCommand(EditOrder);
        }

        private async void DeleteOrder()
        {
            if (ViewModel.SelectedOrder == null)
                return;

            var result = MessageBox.Show($"Удалить заказ от {ViewModel.SelectedOrder.Customer?.CompanyName}?", "Внимание", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await _orderRepo.Delete(ViewModel.SelectedOrder.Id);
                await LoadOrdersAsync();
            }
        }
        private async void EditOrder()
        {
            var selectedOrderDto = ViewModel.SelectedOrder;
            if (selectedOrderDto == null)
            {
                return;
            }

            try
            {
                var customers = await _customerRepo.GetAll();
                var executors = await _userRepo.GetAllUsers();
                var executorModels = MapExecutors(executors);

                var order = selectedOrderDto;
                _cardPresenter.Initialize(customers, executorModels);
                _cardPresenter.Show(order);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task<bool> OnOrderUpdatedAsync(Order order)
        {
            try
            {
                await _orderRepo.UpdateOrder(order);
                await LoadOrdersAsync();
                return true;
            }
            catch (Exception ex)
            {
                _cardPresenter.SetError(ex.Message);
                return false;
            }
        }

            

        private async void AddOrder()
        {
            try
            {
                var customers = await _customerRepo.GetAll();
                var executors = await _userRepo.GetAllUsers();
                var executorModels = MapExecutors(executors.Where(e=> e.Role != UserRole.Support));

                _cardPresenter.Initialize(customers, executorModels);
                _cardPresenter.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async Task<bool> OnOrderCreatedAsync(Order order)
        {
            try
            {
                await _orderRepo.AddOrder(order);
                await LoadOrdersAsync();
                return true;
            }
            catch (Exception ex)
            {
                _cardPresenter.SetError(ex.Message);
                return false;
            }
        }

        private static List<Executor> MapExecutors(IEnumerable<UserDto> users)
        {
            return users
                .Select(user => new Executor
                {
                    Id = user.Id,
                    DisplayName = string.IsNullOrWhiteSpace(user.DisplayName) ? user.Login : user.DisplayName
                })
                .ToList();
        }

        private async Task<string> UploadDocumentsAsync(string[] files)
        {
            return await _orderRepo.UploadDocuments(files);
        }

        private async Task LoadOrdersAsync()
        {
            var orders = await _orderRepo.GetAll();
            ViewModel.Orders.ReplaceWith(orders.OrderBy(s=> s.Customer?.CompanyName));
            ViewModel.SelectedOrder = ViewModel.Orders.FirstOrDefault();
        }

        protected override Task OnActivateAsync(bool isInitialActivation)
        {
            return LoadOrdersAsync();
        }
    }
}
