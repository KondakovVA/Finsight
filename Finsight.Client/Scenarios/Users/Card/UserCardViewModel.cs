using System.Windows.Input;
using Finsight.Client.AppFrame;
using Finsight.Contract.Dto;
using Finsight.Contract.Enum;

namespace Finsight.Client.Scenarios.Users.Card
{
    public class UserCardViewModel : ViewModelBase
    {
        private string _title = "Добавить сотрудника";
        private string? _login;
        private string? _password;
        private UserRole _role;
        private string? _validationErrorText;
        private string? _displayName;

        public Guid Id { get; set; }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public List<UserRole> Roles { get; set; } = [];
        public ICommand? SaveCommand { get; set; }

        public string? Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public string? Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string? DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        public UserRole Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }

        public string? ValidationErrorText
        {
            get => _validationErrorText;
            set => SetProperty(ref _validationErrorText, value);
        }

        public void Clear()
        {
            Id = Guid.Empty;
            Title = "Добавить сотрудника";
            Login = string.Empty;
            Password = string.Empty;
            DisplayName = string.Empty;
            Role = Roles!.FirstOrDefault();
            ValidationErrorText = string.Empty;
        }

        public void Load(UserDto user)
        {
            if (user == null)
            {
                return;
            }

            Id = user.Id;
            Login = user.Login;
            DisplayName = string.IsNullOrWhiteSpace(user.DisplayName) ? user.Login : user.DisplayName;
            Role = user.Role;
            Password = string.Empty;
            ValidationErrorText = string.Empty;
        }
    }
}
