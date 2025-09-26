using Finsight.Client.AppFrame;
using Finsight.Client.DI;
using Finsight.Client.Scenarios.Analytics;
using Finsight.Client.Scenarios.Customers;
using Finsight.Client.Scenarios.Finance;
using Finsight.Client.Scenarios.Home;
using Finsight.Client.Scenarios.Orders;
using Finsight.Client.Scenarios.Users;
using Finsight.Contract.Enum;
using Unity;

namespace Finsight.Client.Services
{
    public class RoleViewManager : IRoleViewManager
    {
        private readonly IUserSession _userSession;
        private static IUnityContainer Container => DI.Container.Instance;

        private static readonly IReadOnlyDictionary<UserRole, Func<IUnityContainer, IPresenter>[]> RolePresenters =
            new Dictionary<UserRole, Func<IUnityContainer, IPresenter>[]>
            {
                [UserRole.Administrator] =
                [
                    static container => container.CreateOrGet<HomePresenter>(),
                    static container => container.CreateOrGet<UsersPresenter>(),
                    static container => container.CreateOrGet<CustomersPresenter>(),
                    static container => container.CreateOrGet<OrdersPresenter>(),
                    static container => container.CreateOrGet<FinancePresenter>(),
                    static container => container.CreateOrGet<AnalyticPresenter>()
                ],
                [UserRole.Manager] =
                [
                    static container => container.CreateOrGet<CustomersPresenter>(),
                    static container => container.CreateOrGet<OrdersPresenter>()
                ],
                [UserRole.Support] =
                [
                    static container => container.CreateOrGet<OrdersPresenter>()
                ]
            };

        public RoleViewManager(IUserSession userSession)
        {
            _userSession = userSession;
        }

        public List<IPresenter> GetMenuItems()
        {
            if (!_userSession.IsAuthenticated)
            {
                return [];
            }

            if (!RolePresenters.TryGetValue(_userSession.CurrentUser.Role, out var presenterFactories))
            {
                return [];
            }

            var container = Container;
            var menuItems = new List<IPresenter>(presenterFactories.Length);

            foreach (var presenterFactory in presenterFactories)
            {
                menuItems.Add(presenterFactory(container));
            }

            return menuItems;
        }

        public string GetCurrentUsername()
        {
            if (!_userSession.IsAuthenticated)
            {
                return string.Empty;
            }

            var user = _userSession.CurrentUser;
            return string.IsNullOrWhiteSpace(user.DisplayName) ? user.Login : user.DisplayName;
        }
    }
}
