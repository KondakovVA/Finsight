using System.Collections.ObjectModel;
using System.Windows.Input;
using Finsight.Client.AppFrame;
using Finsight.Client.Model;

namespace Finsight.Client.Scenarios.Customers
{
    public class CustomersViewModel : ViewModelBase
    {
        public ObservableCollection<Customer> Customers { get; } = [];

        private Customer? _selectedCustomer;

        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value);
        }
        public ICommand? AddCustomerCommand { get; set; }
        public ICommand? DeleteCustomerCommand { get; set; }
        public ICommand? EditCustomerCommand { get; set; }
    }
}
