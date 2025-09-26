using Finsight.Client.AppFrame;
using Finsight.Client.Model;
using Finsight.Client.Utils;

namespace Finsight.Client.Scenarios.Customers.Card
{
    public class CustomerCardPresenter : PresenterBase<CustomerCardView, CustomerCardViewModel>
    {
        private const string AddTitle = "Добавить клиента";
        private const string EditTitle = "Редактировать клиента";

        private bool _isEditMode;
        private string _viewTitle = AddTitle;

        public CustomerCardPresenter()
            : base(new CustomerCardView(), new CustomerCardViewModel { Id = Guid.NewGuid() })
        {
            ViewModel.SaveCommand = new DelegateCommand(async () => await SaveCustomerAsync());
        }

        private async Task SaveCustomerAsync()
        {
            ViewModel.ValidationErrorText = string.Empty;
            var handler = _isEditMode ? CustomerUpdated : CustomerCreated;
            if (handler == null)
            {
                Close();
                return;
            }

            var customer = ViewModel.CreateCustomer();
            var isSaved = await handler(customer);
            if (isSaved)
            {
                Close();
            }
        }

        public void Show(Customer? customer = null)
        {
            if (ModalWindow.IsOpen(ViewContent))
            {
                return;
            }

            if (customer is null)
            {
                _isEditMode = false;
                _viewTitle = AddTitle;
                ViewModel.Title = AddTitle;
                ViewModel.Clear();
            }
            else
            {
                _isEditMode = true;
                _viewTitle = EditTitle;
                ViewModel.Title = EditTitle;
                ViewModel.ValidationErrorText = string.Empty;
                ViewModel.Load(customer);
            }

            ModalWindow.Show(ViewContent, ViewTitle);
        }

        public void Close() => ModalWindow.Close(ViewContent);

        public Func<Customer, Task<bool>>? CustomerCreated { get; set; }
        public Func<Customer, Task<bool>>? CustomerUpdated { get; set; }

        public override string ViewTitle => _viewTitle;

        public void SetError(string message)
        {
            ViewModel.ValidationErrorText = message;
        }
    }
}
