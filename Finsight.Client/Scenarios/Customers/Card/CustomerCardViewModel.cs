using System.Windows.Input;
using Finsight.Client.AppFrame;
using Finsight.Client.Model;
using Finsight.Contract.Enum;

namespace Finsight.Client.Scenarios.Customers.Card
{
    public class CustomerCardViewModel : ViewModelBase
    {
        private string _title = "Добавить клиента";
        private string? _validationErrorText;
        private DateTime _createdDate = DateTime.Now;
        private string _companyName = string.Empty;
        private string _contactName = string.Empty;
        private string _phone = string.Empty;
        private string _email = string.Empty;
        private CustomerStatus _status;

        public Guid Id { get; set; }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string CompanyName
        {
            get => _companyName;
            set => SetProperty(ref _companyName, value);
        }

        public string ContactName
        {
            get => _contactName;
            set => SetProperty(ref _contactName, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public CustomerStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
        public ICommand SaveCommand { get; set; }

        public DateTime CreatedDate
        {
            get => _createdDate;
            set => SetProperty(ref _createdDate, value);
        }

        public string? ValidationErrorText
        {
            get => _validationErrorText;
            set => SetProperty(ref _validationErrorText, value);
        }

        public Customer CreateCustomer() => new Customer
        {
            Id = Id,
            ContactName = ContactName,
            CompanyName = CompanyName,
            Phone = Phone,
            Email = Email,
            Status = Status,
            CreatedDate = CreatedDate == default ? DateTime.Now : CreatedDate,
        };

        public void Load(Customer customer)
        {
            if (customer == null)
            {
                return;
            }

            Id = customer.Id;
            ContactName = customer.ContactName;
            CompanyName = customer.CompanyName;
            Phone = customer.Phone;
            Email = customer.Email;
            Status = customer.Status;
            CreatedDate = customer.CreatedDate;
            ValidationErrorText = string.Empty;
        }

        public void Clear()
        {
            Id = Guid.Empty;
            ContactName = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            CompanyName = string.Empty;
            Status = CustomerStatus.Active;
            CreatedDate = DateTime.Now;
            ValidationErrorText = string.Empty;
        }
    }
}
