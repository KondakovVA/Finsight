using Unity;
using Unity.Resolution;

namespace Finsight.Client.DI
{
    public static class ContainerExtensions
    {
        public static T Get<T>(this IUnityContainer container, params ParameterOverride[] overrides)
        {
            var p = new ParameterOverrides();
            if (overrides != null)
            {
                foreach (var o in overrides)
                {
                    p.Add(o.Name, o.Value);
                }
            }

            return container.Resolve<T>(p);
        }

        /// <summary>
        /// Создает синглтон или возвращает из контейнера, если его нет
        /// </summary>
        public static T CreateOrGet<T>(this IUnityContainer container)
        {
            if (!container.IsRegistered<T>())
                container.RegisterSingleton<T>();
            return container.Get<T>();
        }
    }
}
