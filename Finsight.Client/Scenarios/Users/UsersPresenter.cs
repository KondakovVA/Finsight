using System.Windows;
using Finsight.Client.AppFrame;
using Finsight.Client.DI;
using Finsight.Client.Extensions;
using Finsight.Client.Scenarios.Users.Card;
using Finsight.Client.Services;
using Finsight.Client.Utils;
using Finsight.Contract.Dto;

namespace Finsight.Client.Scenarios.Users
{
    public class UsersPresenter : PresenterBase<UsersView, UsersViewModel>
    {
        private readonly IUserRepo _userRepo;
        private readonly IUserSession _userSession;
        private readonly UserCardPresenter _cardPresenter;

        public UsersPresenter(IUserRepo userRepo, IUserSession userSession, UserCardPresenter cardPresenter)
            : base(new UsersView(), new UsersViewModel { ViewTitle = Properties.Localization.EmployeesLabel })
        {
            _userRepo = userRepo;
            _userSession = userSession;
            _cardPresenter = cardPresenter;
            _cardPresenter.OnSave = OnUserCreatedAsync;
            _cardPresenter.OnUpdate = OnUserUpdatedAsync;

            ViewModel.AddUserCommand = new DelegateCommand(AddUserAction);
            ViewModel.DeleteUserCommand = new DelegateCommand(DeleteUserAction);
            ViewModel.EditUserCommand = new DelegateCommand(EditUserAction);
        }

        private void AddUserAction()
        {
            _cardPresenter.Show();
        }

        private void EditUserAction()
        {
            var selectedUser = ViewModel.SelectedUser;
            if (selectedUser is null)
            {
                return;
            }

            _cardPresenter.Show(selectedUser);
        }

        private async void DeleteUserAction()
        {
            var selectedUser = ViewModel.SelectedUser;
            if (selectedUser is null)
            {
                return;
            }

            //TODO: скрывать с таблицы кнопку для удаления текущего пользователя
            if (selectedUser.Id.Equals(_userSession.CurrentUser.Id))
            {
                MessageBox.Show("Вы не можете удалить самого себя!", "Ошибка", MessageBoxButton.OK);
                return;
            }
            var result = MessageBox.Show($"Удалить пользователя {selectedUser.Login}?", "Внимание", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                await _userRepo.DeleteUser(selectedUser.Id);
                await LoadUsersAsync();
            }
        }

        private async Task<bool> OnUserCreatedAsync(UserDto user)
        {
            var (isValid, validationMessage) = CredentialsValidator.Validate(user.Login, user.Password);
            if (!isValid)
            {
                _cardPresenter.SetResponseText(validationMessage);
                return false;
            }

            var login = user.Login ?? string.Empty;
            var duplicate = ViewModel.Users.Any(u => string.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase));
            if (duplicate)
            {
                _cardPresenter.SetResponseText("Пользователь с таким именем уже существует!");
                return false;
            }

            await _userRepo.AddUser(user);
            await LoadUsersAsync();
            return true;
        }

        private async Task<bool> OnUserUpdatedAsync(UserDto user)
        {
            if (user == null)
            {
                return false;
            }

            var login = user.Login ?? string.Empty;
            if (login.Length < 4)
            {
                _cardPresenter.SetResponseText("      4 !");
                return false;
            }

            var password = user.Password ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(password) && password.Length < 8)
            {
                _cardPresenter.SetResponseText("      8 !");
                return false;
            }

            var duplicate = ViewModel.Users.Any(u => u.Id != user.Id && string.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase));
            if (duplicate)
            {
                _cardPresenter.SetResponseText("     !");
                return false;
            }

            await _userRepo.UpdateUser(user);
            await LoadUsersAsync();
            return true;
        }

        private async Task LoadUsersAsync()
        {
            var users = await _userRepo.GetAllUsers();
            ViewModel.Users.ReplaceWith(users.OrderBy(u => u.Login));
        }

        protected override Task OnActivateAsync(bool isInitialActivation)
        {
            return LoadUsersAsync();
        }
    }
}
