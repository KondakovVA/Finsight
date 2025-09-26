using System;
using Finsight.Contract.Dto;

namespace Finsight.Client.Services
{
    public class UserSession : IUserSession
    {
        private UserDto? _currentUser;

        public UserDto CurrentUser => _currentUser ?? throw new InvalidOperationException("Текущий пользователь не установлен.");

        public bool IsAuthenticated => _currentUser != null;

        public void SetUser(UserDto user)
        {
            _currentUser = user ?? throw new ArgumentNullException(nameof(user));
        }

        public void Clear()
        {
            _currentUser = null;
        }
    }
}
