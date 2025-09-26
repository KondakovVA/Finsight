using System.IO;
using Finsight.Client.AppFrame;
using Finsight.Client.Model;
using Finsight.Client.Utils;
using Microsoft.Win32;

namespace Finsight.Client.Scenarios.Orders.Card
{
    public class OrderCardPresenter : PresenterBase<OrderCardView, OrderCardViewModel>
    {
        private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf",
            ".xls",
            ".xlsx",
            ".doc",
            ".docx"
        };

        private const string AddTitle = "Добавить заказ";
        private const string EditTitle = "Редактировать заказ";

        private bool _isEditMode;
        private string _viewTitle = AddTitle;

        public OrderCardPresenter()
            : base(new OrderCardView(), new OrderCardViewModel())
        {
            ViewModel.SaveCommand = new DelegateCommand(async () => await SaveOrderAsync());
            ViewModel.SelectDocumentsCommand = new DelegateCommand(SelectDocumentsAction);
        }

        public Func<string[], Task<string>>? UploadDocumentsHandler { get; set; }

        public void Initialize(IEnumerable<Customer> customers, IEnumerable<Executor> executors)
        {
            ViewModel.SetCustomers(customers);
            ViewModel.SetExecutors(executors);
        }

        public void Show(Order? order = null)
        {
            if (ModalWindow.IsOpen(ViewContent))
            {
                return;
            }

            if (order is null)
            {
                _isEditMode = false;
                _viewTitle = AddTitle;
                ViewModel.Title = AddTitle;
                ViewModel.Reset();
            }
            else
            {
                _isEditMode = true;
                _viewTitle = EditTitle;
                ViewModel.Title = EditTitle;
                ViewModel.Load(order);
            }

            ModalWindow.Show(ViewContent, ViewTitle);
        }

        public void Close() => ModalWindow.Close(ViewContent);

        public void SetError(string message)
        {
            ViewModel.ValidationErrorText = message;
        }

        private async Task SaveOrderAsync()
        {
            var validationMessage = ViewModel.Validate();
            if (!string.IsNullOrEmpty(validationMessage))
            {
                ViewModel.ValidationErrorText = validationMessage;
                return;
            }

            ViewModel.ValidationErrorText = string.Empty;
            var order = ViewModel.CreateOrder();
            var handler = _isEditMode ? OrderUpdated : OrderCreated;
            if (handler == null)
            {
                Close();
                return;
            }

            var isSaved = await handler(order);
            if (isSaved)
            {
                Close();
            }
        }

        private async void SelectDocumentsAction()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Документы (*.pdf;*.xls;*.xlsx;*.doc;*.docx)|*.pdf;*.xls;*.xlsx;*.doc;*.docx"
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            var selectedFiles = dialog.FileNames.Where(File.Exists).ToArray();
            if (selectedFiles.Length == 0)
            {
                ViewModel.ValidationErrorText = "Не удалось прочитать выбранные файлы.";
                return;
            }

            if (selectedFiles.Any(file => !IsAllowedExtension(file)))
            {
                ViewModel.ValidationErrorText = "Разрешены только файлы форматов PDF, XLS, XLSX, DOC, DOCX.";
                return;
            }

            if (UploadDocumentsHandler == null)
            {
                ViewModel.ValidationErrorText = "Загрузка документов недоступна.";
                return;
            }

            try
            {
                var path = await UploadDocumentsHandler(selectedFiles);
                if (!string.IsNullOrEmpty(path))
                {
                    ViewModel.DocumentsPath = path;
                    ViewModel.IsDocumentsUploaded = true;
                    ViewModel.DocumentsDescription = "Документы успешно загружены.";
                }
                else
                {
                    ViewModel.DocumentsPath = string.Empty;
                    ViewModel.IsDocumentsUploaded = false;
                    ViewModel.DocumentsDescription = "Не удалось загрузить документы.";
                }
                ViewModel.ValidationErrorText = string.Empty;
            }
            catch (Exception ex)
            {
                ViewModel.ValidationErrorText = ex.Message;
                ViewModel.IsDocumentsUploaded = false;
                ViewModel.DocumentsDescription = "Не удалось загрузить документы.";
            }
        }

        private static bool IsAllowedExtension(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            return !string.IsNullOrWhiteSpace(extension) && AllowedExtensions.Contains(extension);
        }

        public Func<Order, Task<bool>>? OrderCreated { get; set; }
        public Func<Order, Task<bool>>? OrderUpdated { get; set; }

        public override string ViewTitle => _viewTitle;
    }
}
