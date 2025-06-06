using RxEventBus.Core.Events;
using System.Diagnostics;

namespace RxEventBus.Core
{
    public static class GlobalEventErrorHandler
    {
        public static Task OnErrorAsync<T>(AppEvent<T> evt, Exception ex)
        {
            Debug.WriteLine($"🌐 [GlobalError] Error handling event {evt.Type}: {ex.Message}");
            return Task.CompletedTask;
        }

        public static Task OnCompletedAsync(AppEventType eventType)
        {
            Debug.WriteLine($"🌐 [GlobalComplete] EventType {eventType} completed.");
            return Task.CompletedTask;
        }
    }

}
