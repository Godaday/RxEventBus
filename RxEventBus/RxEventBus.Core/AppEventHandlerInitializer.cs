using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RxEventBus.Core.Events;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;

namespace RxEventBus.Core
{
    /// <summary>
    /// 事件处理程序初始化
    /// </summary>
    public class AppEventHandlerInitializer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAppEventBus _eventBus;
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="eventBus"></param>
        public AppEventHandlerInitializer(IServiceProvider serviceProvider, IAppEventBus eventBus)
        {
            _serviceProvider = serviceProvider;
            _eventBus = eventBus;
        }
        /// <summary>
        /// 服务执行
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var serviceProvider = scope.ServiceProvider;

            var handlerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
                .Where(x => x.Interface.IsGenericType && x.Interface.GetGenericTypeDefinition() == typeof(IAppEventHandler<>))
                .Select(x => new { Implementation = x.Type, Interface = x.Interface })
                .Distinct()
                .ToList();

            foreach (var handler in handlerTypes)
            {
                // 通过接口类型解析服务（
                var handlerInstance = serviceProvider.GetRequiredService(handler.Implementation);

                var eventType = (AppEventType)handler.Interface
                    .GetProperty("EventType")!
                    .GetValue(handlerInstance)!;

                var method = typeof(AppEventHandlerInitializer)
                    .GetMethod(nameof(SubscribeGeneric), BindingFlags.Instance | BindingFlags.NonPublic)!
                    .MakeGenericMethod(handler.Interface.GetGenericArguments()[0]);

                method.Invoke(this, new object[] { handlerInstance, eventType });
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handlerObj"></param>
        /// <param name="type"></param>
        private void SubscribeGeneric<T>(object handlerObj, AppEventType type)
        {
            var handler = (IAppEventHandler<T>)handlerObj;

            _eventBus.Listen<T>(type).ObserveOn(Scheduler.Default).Subscribe(
                async evt =>
                {
                    try
                    {
                        await handler.HandleAsync(evt);
                    }
                    catch (Exception ex)
                    {
                        await handler.OnErrorAsync(evt, ex);
                    }
                },
                async ex =>
                {
                    await handler.OnErrorAsync(new AppEvent<T>(type, default(T)!), ex);

                },
                async () =>
                {
                    await handler.OnCompletedAsync(type);
                }
            );
        }

    }

}
