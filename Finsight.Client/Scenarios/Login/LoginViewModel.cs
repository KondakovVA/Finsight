using System.Windows.Input;
using Finsight.Client.AppFrame;

namespace Finsight.Client.Scenarios.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private string? _login;
        private string? _password;

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

        private string? _errorText;
        public string? ErrorText
        {
            get => _errorText;
            set => SetProperty(ref _errorText, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        public ICommand? LoginCommand { get; set; }
    }
}
