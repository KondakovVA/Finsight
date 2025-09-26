using Finsight.Client.Http.Services;
using Finsight.Client.Repository;
using Finsight.Client.Scenarios.Analytics;
using Finsight.Client.Scenarios.Customers;
using Finsight.Client.Scenarios.Customers.Card;
using Finsight.Client.Scenarios.Finance;
using Finsight.Client.Scenarios.Home;
using Finsight.Client.Scenarios.Login;
using Finsight.Client.Scenarios.Main;
using Finsight.Client.Scenarios.Orders;
using Finsight.Client.Scenarios.Orders.Card;
using Finsight.Client.Scenarios.Users;
using Finsight.Client.Scenarios.Users.Card;
using Finsight.Client.Services;
using Finsight.Contract.Services;
using Unity;

namespace Finsight.Client.DI
{
    public static class Container
    {
        public static readonly IUnityContainer Instance = new UnityContainer();
        static Container()
        {
            // Сервисы
            Instance.RegisterSingleton<IUserSession, UserSession>();
            Instance.RegisterSingleton<IRoleViewManager, RoleViewManager>();
            Instance.RegisterSingleton<IUserService, UserService>();
            Instance.RegisterSingleton<ICustomerService, CustomerService>();
            Instance.RegisterSingleton<IOrderService, OrderService>();
            // Репозитории
            Instance.RegisterSingleton<IUserRepo, UserRepo>();
            Instance.RegisterSingleton<ICustomerRepo, CustomerRepo>();
            Instance.RegisterSingleton<IOrderRepo, OrderRepo>();
            // Презентеры
            Instance.RegisterSingleton<LoginPresenter>();
            Instance.RegisterSingleton<MainPresenter>();
            Instance.RegisterSingleton<HomePresenter>();
            Instance.RegisterSingleton<UsersPresenter>();
            Instance.RegisterSingleton<CustomersPresenter>();
            Instance.RegisterSingleton<OrdersPresenter>();
            Instance.RegisterSingleton<FinancePresenter>();
            Instance.RegisterSingleton<AnalyticPresenter>();
            // CardPresenters
            Instance.RegisterType<UserCardPresenter>();
            Instance.RegisterType<CustomerCardPresenter>();
            Instance.RegisterType<OrderCardPresenter>();
        }
    }
}
