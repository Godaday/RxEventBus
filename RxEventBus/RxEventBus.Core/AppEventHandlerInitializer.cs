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

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var handlerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location))
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
                .Where(x => x.Interface.IsGenericType && x.Interface.GetGenericTypeDefinition() == typeof(IAppEventHandler<>))
                .Select(x => x.Type)
                .Distinct()
                .ToList();

            Console.WriteLine("\n--- AppEventHandlerInitializer: Discovered Handlers ---"); // 改为 Console.WriteLine
            foreach (var handlerImplementationType in handlerTypes)
            {
                Console.WriteLine($"[Initializer] Discovered handler: {handlerImplementationType.FullName}"); 

                var handlerInstance = serviceProvider.GetRequiredService(handlerImplementationType);
                var eventPayloadType = handlerImplementationType
                    .GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAppEventHandler<>))
                    .GetGenericArguments()[0];

                var method = typeof(AppEventHandlerInitializer)
                    .GetMethod(nameof(SubscribeGeneric), BindingFlags.Instance | BindingFlags.NonPublic)!
                    .MakeGenericMethod(eventPayloadType);

                method.Invoke(this, new object[] { handlerInstance });
            }
            Console.WriteLine("--- AppEventHandlerInitializer: Handler Discovery Complete ---\n"); 

            return Task.CompletedTask;
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handlerObj"></param>
        /// <param name="type"></param>
        private void SubscribeGeneric<T>(object handlerObj)
        {
            var handler = (IAppEventHandler<T>)handlerObj;

            _eventBus.Listen<T>().ObserveOn(Scheduler.Default).Subscribe(
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
                    finally 
                    {
                        await handler.OnEventHandledAsync(evt);
                    }
                },
                async ex =>
                {
                    await handler.OnErrorAsync(new AppEvent<T>( default(T)!), ex);

                },
                async () =>
                {
                    await handler.OnCompletedAsync(new AppEvent<T>(default(T)!));
                }
            );
        }

    }

}
