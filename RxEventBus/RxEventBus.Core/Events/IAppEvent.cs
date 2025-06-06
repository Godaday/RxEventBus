
namespace RxEventBus.Core.Events
{
    /// <summary>
    /// IAppEvent事件接口
    /// </summary>
    public interface IAppEvent
    {
        /// <summary>
        /// 时间类型
        /// </summary>
        AppEventType Type { get; }
        /// <summary>
        /// 事件负载
        /// </summary>
        object Payload { get; }
        /// <summary>
        /// 事件发生时间戳
        /// </summary>
        DateTime Timestamp { get; } 
    }
    /// <summary>
    /// IAppEvent泛型实现
    /// </summary>
    /// <typeparam name="TPayload"></typeparam>
    public class AppEvent<TPayload> : IAppEvent
    {
        /// <summary>
        /// EventType
        /// </summary>
        public AppEventType Type { get; }
        /// <summary>
        /// 事件Payload
        /// </summary>
        public TPayload Payload { get; }

        object IAppEvent.Payload => Payload!;

        /// <summary>
        /// 事件事件(默认当前时间)
        /// </summary>
        public DateTime Timestamp => DateTime.Now;

        /// <summary>
        /// 初始化事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="payload">事件Payload参数</param>
        public AppEvent(AppEventType eventType, TPayload payload)
        {
            Type = eventType;
            Payload = payload;
        }
    }

}
