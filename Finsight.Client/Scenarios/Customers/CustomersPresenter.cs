using System.Windows;
using Finsight.Client.AppFrame;
using Finsight.Client.DI;
using Finsight.Client.Extensions;
using Finsight.Client.Model;
using Finsight.Client.Scenarios.Customers.Card;
using Finsight.Client.Services;
using Finsight.Client.Utils;
using Localization = Finsight.Client.Properties.Localization;

namespace Finsight.Client.Scenarios.Customers
{
    public class CustomersPresenter : PresenterBase<CustomersView, CustomersViewModel>
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly CustomerCardPresenter _cardPresenter;
        private readonly IUserSession _userSession;

        public CustomersPresenter(ICustomerRepo customerRepo, IUserSession userSession, CustomerCardPresenter cardPresenter)
            : base(new CustomersView(), new CustomersViewModel { ViewTitle = Localization.CustomersLabel })
        {
            _customerRepo = customerRepo;
            _userSession = userSession;
            _cardPresenter = cardPresenter;
            _cardPresenter.CustomerCreated = OnSavedAsync;
            _cardPresenter.CustomerUpdated = OnUpdatedAsync;

            ViewModel.AddCustomerCommand = new DelegateCommand(AddCustomer);
            ViewModel.DeleteCustomerCommand = new DelegateCommand(DeleteCustomer);
            ViewModel.EditCustomerCommand = new DelegateCommand(EditCustomer);
        }

        private async Task<bool> OnSavedAsync(Customer customer)
        {
            try
            {
                customer.UserId = _userSession.CurrentUser.Id;
                await _customerRepo.AddCustomer(customer);
                await LoadCustomersAsync();
                return true;
            }
            catch (Exception ex)
            {
                _cardPresenter.SetError(ex.Message);
                return false;
            }
        }

        private async void DeleteCustomer()
        {
            if (ViewModel.SelectedCustomer is null)
                return;
            var result = MessageBox.Show(
                string.Format(Localization.DeleteCustomerQuestion, ViewModel.SelectedCustomer.CompanyName),
                Localization.Attention,
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await _customerRepo.DeleteCustomer(ViewModel.SelectedCustomer.Id);
                await LoadCustomersAsync();
            }
        }

        private void EditCustomer()
        {
            if (ViewModel.SelectedCustomer == null)
            {
                return;
            }

            _cardPresenter.Show(ViewModel.SelectedCustomer);
        }

        private async Task<bool> OnUpdatedAsync(Customer customer)
        {
            try
            {
                customer.UserId = _userSession.CurrentUser.Id;
                await _customerRepo.UpdateCustomer(customer);
                await LoadCustomersAsync();
                return true;
            }
            catch (Exception ex)
            {
                _cardPresenter.SetError(ex.Message);
                return false;
            }
        }

        private void AddCustomer()
        {
            _cardPresenter.Show();
        }

        private async Task LoadCustomersAsync()
        {
            var customersList = await _customerRepo.GetAll();
            ViewModel.Customers.ReplaceWith(customersList.OrderBy(c => c.CompanyName));
            ViewModel.SelectedCustomer = ViewModel.Customers.FirstOrDefault();
        }

        protected override Task OnActivateAsync(bool isInitialActivation)
        {
            return LoadCustomersAsync();
        }
    }
}
