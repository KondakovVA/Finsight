using System.ComponentModel;
using System.Configuration;
using Finsight.Client.DI;
using Finsight.Client.Services;
using Finsight.Client.Utils;
using Finsight.Contract.Dto;
using Flurl.Http;
using Unity;

namespace Finsight.Client.Scenarios.Login
{
    public class LoginPresenter : IDisposable
    {
        private readonly LoginViewModel _viewModel;
        private readonly LoginView _view;
        private readonly IUserRepo _userRepo;
        private readonly IUserSession _userSession;
        private readonly DelegateCommand _loginCommand;
        private readonly bool _isDebug;
        private readonly string _debugLogin;
        private readonly string _debugPassword;

        public LoginPresenter(IUserRepo userRepo, IUserSession userSession)
        {
            _userRepo = userRepo;
            _userSession = userSession;

            _debugLogin = ConfigurationManager.AppSettings.Get("debugLoginUsername") ?? string.Empty;
            _debugPassword = ConfigurationManager.AppSettings.Get("debugLoginPassword") ?? string.Empty;

            var debugLoginEnabledValue = ConfigurationManager.AppSettings.Get("debugLoginEnabled");
            _isDebug = bool.TryParse(debugLoginEnabledValue, out var parsedDebugFlag) && parsedDebugFlag;

            _loginCommand = new DelegateCommand(LoginAction);
            _viewModel = new LoginViewModel
            {
                LoginCommand = _loginCommand
            };
            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
            _view = new LoginView
            {
                DataContext = _viewModel
            };
        }

        public void StartApplication()
        {
            if (_isDebug)
                StartDebug();
            else
                _view.Show();
        }

        private async void StartDebug()
        {
            var result = await _userRepo.Login(_debugLogin, _debugPassword);
            if (result != null)
            {
                _userSession.SetUser(result);
                OnLoginSuccess?.Invoke(this, result);
            }
        }

        public EventHandler<UserDto> OnLoginSuccess;
        private void _viewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            _loginCommand?.RaiseCanExecuteChanged();
        }

        private async void LoginAction()
        {
            var result = CredentialsValidator.Validate(_viewModel.Login, _view.PasswordBox.GetPassword());
            if (!result.Item1)
            {
                _viewModel.ErrorText = result.Item2;
                return;
            }

            _viewModel.ErrorText = string.Empty;

            try
            {
                _viewModel.IsBusy = true;
                var user = await _userRepo.Login(_viewModel.Login!, GetPassword());
                if (user != null)
                {
                    _userSession.SetUser(user);
                    OnLoginSuccess?.Invoke(this, user);
                }
            }
            catch(FlurlHttpException ex)
            {
                if (ex.StatusCode == 401)
                    _viewModel.ErrorText = "Неверный логин или пароль!";
                else
                    _viewModel.ErrorText = "Ошибка соединения с сервером!";
            }
            finally
            {
                _viewModel.IsBusy = false;
            }
        }

        private string GetPassword() => _view.PasswordBox.GetPassword();

        public void CloseView() => _view.Close();

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
