using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RxEventBus.Core
{
   
    /// <summary>
    /// 事件总线注册
    /// </summary>
    public static class EventBusServiceCollectionExtensions
    {
        /// <summary>
        /// 注册事件总线为单例
        /// </summary>
        public static IServiceCollection AddRxEventBus(this IServiceCollection services)
        {
            services.AddSingleton<IAppEventBus, AppEventBus>();

            var handlerTypes = AppDomain.CurrentDomain.GetAssemblies()
              .Where(assembly => !assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location)) // 过滤掉动态程序集和没有文件路径的程序集
              .SelectMany(assembly => assembly.GetTypes())
              .Where(t => !t.IsAbstract && !t.IsInterface)
              .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
              .Where(x => x.Interface.IsGenericType && x.Interface.GetGenericTypeDefinition() == typeof(IAppEventHandler<>))
              .Select(x => x.Type)
              .Distinct();

            foreach (var handler in handlerTypes)
            {
                services.AddTransient(handler);
            }

            services.AddHostedService<AppEventHandlerInitializer>(); // 自动订阅
            return services;
        }
    }

}
