

using RxEventBus.Core.Events;

namespace RxEventBus.Core
{
    /// <summary>
    ///事件总线接口
    /// </summary>
    public interface IAppEventBus
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="evt"></param>
        void Publish<T>(AppEvent<T> evt);
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterType"></param>
        /// <returns></returns>
        IObservable<AppEvent<T>> Listen<T>();
        /// <summary>
        /// 订阅所有事件
        /// </summary>
        /// <returns></returns>
        IObservable<IAppEvent> ListenAll();
    }
}
