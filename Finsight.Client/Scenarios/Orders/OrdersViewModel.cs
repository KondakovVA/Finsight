using System.Collections.ObjectModel;
using System.Windows.Input;
using Finsight.Client.AppFrame;
using Finsight.Client.Model;

namespace Finsight.Client.Scenarios.Orders
{
    public class OrdersViewModel : ViewModelBase
    {
        public ObservableCollection<Order> Orders { get; } = [];

        private Order? _selectedOrder;
        public Order? SelectedOrder
        {
            get => _selectedOrder;
            set => SetProperty(ref _selectedOrder, value);
        }
        public ICommand AddOrderCommand { get; set; }
        public ICommand DeleteOrderCommand { get; set; }
        public ICommand? EditOrderCommand { get; set; }
    }
}
