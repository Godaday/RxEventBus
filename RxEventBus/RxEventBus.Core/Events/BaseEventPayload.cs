namespace RxEventBus.Core.Events;

/// <summary>
/// 事件负载基类 <see cref="BaseRxPayload"/>
/// </summary>
[Obsolete("BaseRxPayloadClass 不建议作为事件Payload的基类。请使用 BaseRxPayload 来代替，它提供了不可变性和值相等性等更适合事件的特性。", false)] 
public abstract class BaseRxPayloadClass
{
    protected BaseRxPayloadClass(string userId,DateTime occurredTimestamp)
    {
        UserId= userId;
        BusinessOccurredUtcTimestamp = occurredTimestamp;
    }
    protected BaseRxPayloadClass()
    { }


    /// <summary>
    /// 业务事件实际发生的UTC时间戳
    /// </summary>
    public DateTime BusinessOccurredUtcTimestamp { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// 关联到此业务事件的用户唯一标识符。
    /// </summary>
    public string? UserId { get; init; }

    /// <summary>
    /// 关联ID。
    /// 用于追踪跨服务或跨模块的单个逻辑请求流
    /// </summary>
    public string? CorrelationId { get; init; }

}
/// <summary>
/// 事件负载基类
/// </summary>
public abstract record BaseEventPayload : IBaseEventPayload
{
    string IBaseEventPayload.EventId => throw new NotImplementedException();

    DateTime IBaseEventPayload.OccurredUtcTimestamp => throw new NotImplementedException();

    string IBaseEventPayload.InitiatorId => throw new NotImplementedException();

    string IBaseEventPayload.InitiatorType => throw new NotImplementedException();

    string? IBaseEventPayload.InitiatorName => throw new NotImplementedException();

    string? IBaseEventPayload.CorrelationId => throw new NotImplementedException();

    string? IBaseEventPayload.CausationId => throw new NotImplementedException();
}
