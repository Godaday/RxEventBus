

using RxEventBus.Core.Events;

namespace RxEventBus.Core
{
    /// <summary>
    /// 事件处理程序
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAppEventHandler<T>
    {
        /// <summary>
        /// EventType
        /// </summary>
        AppEventType EventType { get; }
        /// <summary>
        /// 事件处理
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        Task HandleAsync(AppEvent<T> evt);

        /// <summary>
        /// 处理失败时默认行为
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        Task OnErrorAsync(AppEvent<T> evt, Exception ex) => GlobalEventErrorHandler.OnErrorAsync(evt, ex);

        /// <summary>
        /// 事件完成后默认行为
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        Task OnCompletedAsync(AppEventType eventType) => GlobalEventErrorHandler.OnCompletedAsync(eventType);
    }
}
