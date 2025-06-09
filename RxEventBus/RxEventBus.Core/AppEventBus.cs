

using RxEventBus.Core.Events;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RxEventBus.Core
{
 
    /// <summary>
    /// 事件总线
    /// </summary>
    public class AppEventBus: IAppEventBus
    {
        private readonly Subject<IAppEvent> _subject = new();
        /// <summary>
        /// 发布事件    
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="evt"></param>
        public void Publish<T>(AppEvent<T> evt)
        {
            _subject.OnNext(evt);
        }
        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterType"></param>
        /// <returns></returns>
        public IObservable<AppEvent<T>> Listen<T>()
        {
            return _subject.OfType<AppEvent<T>>();
            //return filterType.HasValue
            //    ? stream.Where(e => e.Type  == filterType.Value)
            //    : stream;
        }
        /// <summary>
        /// 订阅所有事件
        /// </summary>
        /// <returns></returns>
        public IObservable<IAppEvent> ListenAll() => _subject.AsObservable();
    }

}
