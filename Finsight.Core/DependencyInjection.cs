using System;
using Finsight.Core.Bl;
using Finsight.Core.Bl.Implementation;
using Finsight.Core.Dao;
using Finsight.Core.Dao.EF;
using Finsight.Core.Dao.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Finsight.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddScoped<IDataSource, EfDataSource>();

            services.AddScoped<IDao<User>, EntityDao<User>>();
            services.AddScoped<IDao<Customer>, EntityDao<Customer>>();
            services.AddScoped<IDao<Order>, EntityDao<Order>>();

            services.AddScoped<IUserBl, UserBl>();
            services.AddScoped<ICustomerBl, CustomerBl>();
            services.AddScoped<IOrderBl, OrderBl>();

            return services;
        }
    }
}
