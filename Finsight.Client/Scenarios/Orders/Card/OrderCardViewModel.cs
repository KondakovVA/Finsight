using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Finsight.Client.AppFrame;
using Finsight.Client.Extensions;
using Finsight.Client.Model;
using Finsight.Contract.Enum;

namespace Finsight.Client.Scenarios.Orders.Card
{
    public class OrderCardViewModel : ViewModelBase
    {
        private Guid _id;
        private Customer? _selectedCustomer;
        private Executor? _selectedExecutor;
        private DateTime? _expireDate;
        private string? _price;
        private string? _description;
        private string? _comment;
        private string? _documentsPath;
        private string? _validationErrorText;
        private string _title = "Добавить заказ";
        private OrderStatus _status = OrderStatus.New;
        private DateTime _startDate = DateTime.Now;

        public OrderCardViewModel()
        {
            ExpireDate = DateTime.Today;
        }

        public ObservableCollection<Customer> Customers { get; } = [];

        public ObservableCollection<Executor> Executors { get; } = [];

        public ICommand? SaveCommand { get; set; }

        public ICommand? SelectDocumentsCommand { get; set; }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public Guid Id
        {
            get => _id;
            set
            {
                if (SetProperty(ref _id, value))
                {
                    OnPropertyChanged(nameof(IsEditMode));
                }
            }
        }

        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value);
        }

        public Executor? SelectedExecutor
        {
            get => _selectedExecutor;
            set => SetProperty(ref _selectedExecutor, value);
        }

        public DateTime? ExpireDate
        {
            get => _expireDate;
            set => SetProperty(ref _expireDate, value);
        }

        public string? Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string? Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public string? DocumentsPath
        {
            get => _documentsPath;
            set
            {
                SetProperty(ref _documentsPath, value);
            }
        }

        private bool _isDocumentsUploaded;
        public bool IsDocumentsUploaded
        {
            get => _isDocumentsUploaded;
            set => SetProperty(ref _isDocumentsUploaded, value);
        }

        private string _documentsDescription = string.Empty;
        public string DocumentsDescription
        {
            get => _documentsDescription;
            set => SetProperty(ref _documentsDescription, value);
        }

        public OrderStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        public string? ValidationErrorText
        {
            get => _validationErrorText;
            set => SetProperty(ref _validationErrorText, value);
        }

        public bool IsEditMode => Id != Guid.Empty;

        public void SetCustomers(IEnumerable<Customer> customers)
        {
            var selectedId = SelectedCustomer?.Id;
            Customers.ReplaceWith(customers ?? Enumerable.Empty<Customer>());
            SelectedCustomer = selectedId == null
                ? null
                : Customers.FirstOrDefault(c => c.Id == selectedId) ?? Customers.FirstOrDefault();
        }

        public void SetExecutors(IEnumerable<Executor> executors)
        {
            var selectedId = SelectedExecutor?.Id;
            Executors.ReplaceWith(executors ?? Enumerable.Empty<Executor>());
            SelectedExecutor = selectedId == null
                ? null
                : Executors.FirstOrDefault(e => e.Id == selectedId) ?? Executors.FirstOrDefault();
        }

        public string? Validate()
        {
            if (SelectedCustomer == null)
            {
                return "Выберите клиента.";
            }

            if (!ExpireDate.HasValue)
            {
                return "Укажите дату выполнения заказа.";
            }

            if (ExpireDate.Value.Date < DateTime.Today)
            {
                return "Дата выполнения не может быть в прошлом.";
            }

            if (SelectedExecutor == null)
            {
                return "Выберите исполнителя.";
            }

            if (string.IsNullOrWhiteSpace(Price))
            {
                return "Укажите стоимость заказа.";
            }

            if (!decimal.TryParse(Price, NumberStyles.Any, CultureInfo.CurrentCulture, out var price) || price <= 0)
            {
                return "Некорректная стоимость заказа.";
            }

            return null;
        }

        public Order CreateOrder()
        {
            var price = decimal.Parse(Price!, NumberStyles.Any, CultureInfo.CurrentCulture);
            var expireDate = ExpireDate ?? DateTime.Today;

            return new Order
            {
                Id = Id == Guid.Empty ? Guid.NewGuid() : Id,
                Customer = SelectedCustomer == null
                    ? null
                    : new Customer
                    {
                        Id = SelectedCustomer.Id,
                        CompanyName = SelectedCustomer.CompanyName,
                        ContactName = SelectedCustomer.ContactName,
                        Email = SelectedCustomer.Email,
                        Phone = SelectedCustomer.Phone,
                        Status = SelectedCustomer.Status,
                        User = SelectedCustomer.User,
                        UserId = SelectedCustomer.UserId,
                        CreatedDate = SelectedCustomer.CreatedDate
                    },
                Executor = SelectedExecutor == null
                    ? null
                    : new Executor
                    {
                        Id = SelectedExecutor.Id,
                        DisplayName = SelectedExecutor.DisplayName
                    },
                StartDate = StartDate == default ? DateTime.Now : StartDate,
                ExpireDate = expireDate,
                Status = Status,
                Price = price,
                Description = (Description ?? string.Empty).Trim(),
                Comment = (Comment ?? string.Empty).Trim(),
                DocumentsPath = DocumentsPath ?? string.Empty
            };
        }

        public void Reset()
        {
            Id = Guid.Empty;
            Title = "Добавить заказ";
            StartDate = DateTime.Now;
            Status = OrderStatus.New;
            ExpireDate = DateTime.Today;
            Price = string.Empty;
            Description = string.Empty;
            Comment = string.Empty;
            DocumentsPath = string.Empty;
            ValidationErrorText = string.Empty;
            DocumentsDescription = string.Empty;
        }

        public void Load(Order order)
        {
            if (order == null)
            {
                return;
            }
            Id = order.Id;
            StartDate = order.StartDate;
            ExpireDate = order.ExpireDate;
            Status = order.Status;
            Price = order.Price.ToString(CultureInfo.CurrentCulture);
            Description = order.Description;
            Comment = order.Comment;
            DocumentsPath = order.DocumentsPath;
            ValidationErrorText = string.Empty;
            DocumentsDescription = string.Empty;

            if (order.Customer != null)
            {
                var customerInList = Customers.FirstOrDefault(c => c.Id == order.Customer.Id);
                if (customerInList == null)
                {
                    Customers.Add(order.Customer);
                    customerInList = order.Customer;
                }

                SelectedCustomer = customerInList;
            }

            if (order.Executor != null)
            {
                var executorInList = order.Executor.Id.HasValue
                    ? Executors.FirstOrDefault(e => e.Id == order.Executor.Id)
                    : null;

                executorInList ??= Executors.FirstOrDefault(e => string.Equals(e.DisplayName, order.Executor.DisplayName, StringComparison.OrdinalIgnoreCase));

                if (executorInList == null)
                {
                    Executors.Add(order.Executor);
                    executorInList = order.Executor;
                }

                SelectedExecutor = executorInList;
            }
        }
    }
}
