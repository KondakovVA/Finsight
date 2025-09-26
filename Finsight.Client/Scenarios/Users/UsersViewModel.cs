using System.Collections.ObjectModel;
using System.Windows.Input;
using Finsight.Client.AppFrame;
using Finsight.Contract.Dto;

namespace Finsight.Client.Scenarios.Users
{
    public class UsersViewModel : ViewModelBase
    {
        private UserDto? _selectedUser;

        public UsersViewModel()
        {
            Users = new ObservableCollection<UserDto>();
        }
        public ObservableCollection<UserDto> Users { get; }

        public UserDto? SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }
        public ICommand? AddUserCommand { get; set; }
        public ICommand? DeleteUserCommand { get; set; }
        public ICommand? EditUserCommand { get; set; }

    }
}
