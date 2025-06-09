using RxEventBus.Core.Events;
using System.Diagnostics;

namespace RxEventBus.Core
{
    public static class GlobalEventErrorHandler
    {
        public static Task OnErrorAsync<T>(AppEvent<T> evt, Exception ex)
        {
            Debug.WriteLine($"[GlobalError] Error handling event {typeof(T).Name}: {ex.Message}");
            return Task.CompletedTask;
        }

        public static Task OnCompletedAsync<T>(AppEvent<T> evt)
        {
            Debug.WriteLine($"[GlobalComplete] EventType {typeof(T).Name} completed.");
            return Task.CompletedTask;
        }


        /// <summary>
        /// 单个事件处理完成后执行的全局默认行为
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="evt"></param>
        /// <returns></returns>
        public static Task OnEventHandledAsync<T>(AppEvent<T> evt)
        {
            Console.WriteLine($"[EventCompleted] Event of type {typeof(T).Name}");
            return Task.CompletedTask;
        }
    }

}
