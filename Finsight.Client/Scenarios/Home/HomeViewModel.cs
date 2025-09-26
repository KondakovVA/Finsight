using System.Collections.ObjectModel;
using Finsight.Client.AppFrame;
using Finsight.Client.Model;
using Finsight.Contract.Dto;

namespace Finsight.Client.Scenarios.Home
{
    public class HomeViewModel : ViewModelBase
    {
        public ObservableCollection<Order> Orders { get; } = [];
        public ObservableCollection<UserDto> Users { get; } = [];
        public ObservableCollection<Customer> Customers { get; } = [];
        private string _totalIncome;
        public string TotalIncome
        {
            get => _totalIncome;
            set => SetProperty(ref _totalIncome, value);
        }
    }
}
