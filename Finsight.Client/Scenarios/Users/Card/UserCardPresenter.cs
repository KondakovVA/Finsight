using System;
using System.Threading.Tasks;
using Finsight.Client.AppFrame;
using Finsight.Client.Utils;
using Finsight.Contract.Dto;

namespace Finsight.Client.Scenarios.Users.Card
{
    public class UserCardPresenter : PresenterBase<UserCardView, UserCardViewModel>
    {
        private const string AddViewTitle = "Добавить пользователя";
        private const string EditViewTitle = "Редактировать пользователя";
        private const string AddHeaderTitle = "Добавить сотрудника";
        private const string EditHeaderTitle = "Редактировать сотрудника";

        private bool _isEditMode;
        private string _viewTitle = AddViewTitle;

        public UserCardPresenter()
            : base(new UserCardView(), new UserCardViewModel())
        {
            ViewModel.SaveCommand = new DelegateCommand(async () => await SaveAsync());
        }

        public Func<UserDto, Task<bool>>? OnSave { get; set; }
        public Func<UserDto, Task<bool>>? OnUpdate { get; set; }

        public void Close() => ModalWindow.Close(ViewContent);

        public void Show(UserDto? user = null)
        {
            if (ModalWindow.IsOpen(ViewContent))
            {
                return;
            }

            if (user is null)
            {
                _isEditMode = false;
                _viewTitle = AddViewTitle;
                ViewModel.Title = AddHeaderTitle;
                ViewModel.Clear();
            }
            else
            {
                _isEditMode = true;
                _viewTitle = EditViewTitle;
                ViewModel.Title = EditHeaderTitle;
                ViewModel.Load(user);
            }

            View.PasswordBox.Clear();
            ModalWindow.Show(ViewContent, ViewTitle);
        }

        public void SetResponseText(string responseText)
        {
            ViewModel.ValidationErrorText = responseText;
        }

        private async Task SaveAsync()
        {
            ViewModel.ValidationErrorText = string.Empty;
            var handler = _isEditMode ? OnUpdate : OnSave;
            if (handler == null)
            {
                Close();
                return;
            }

            var password = View.PasswordBox.GetPassword();
            var displayName = string.IsNullOrWhiteSpace(ViewModel.DisplayName)
                ? ViewModel.Login ?? string.Empty
                : ViewModel.DisplayName;

            var user = new UserDto
            {
                Id = _isEditMode && ViewModel.Id != Guid.Empty ? ViewModel.Id : Guid.NewGuid(),
                Login = ViewModel.Login ?? string.Empty,
                Password = password,
                Role = ViewModel.Role,
                DisplayName = displayName
            };

            var isSaved = await handler(user);
            if (isSaved)
            {
                Close();
            }
        }

        public override string ViewTitle => _viewTitle;
    }
}
