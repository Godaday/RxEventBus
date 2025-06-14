namespace  RxEventBus.Core.Events;

/// <summary>
/// 事件Payload基础
/// </summary>
public interface IBaseEventPayload
{
         /// <summary>
        /// 事件的唯一实例标识符 (自动生成)。
        /// 用于在系统中唯一标识和追踪此特定的事件实例。
        /// 这是事件本身的唯一 ID。 
        /// </summary>
        string EventId { get; }

        /// <summary>
        /// 业务事件实际发生的UTC时间戳。
        /// 此时间点是业务逻辑中事件发生的真实时间，而非事件对象被创建或发布的系统时间。
        /// 强调是业务层面发生的准确时间。
        /// </summary>
        DateTime OccurredUtcTimestamp { get; }

        /// <summary>
        /// 事件发起者的唯一标识符。
        /// 例如：用户ID、系统服务ID、API网关ID等。这可以是任何能够唯一标识发起方的字符串。
        /// </summary>
        string InitiatorId { get; }

        /// <summary>
        /// 事件发起者的类型。
        /// 用于区分发起者的类别，例如："User"（用户）, "System"（系统）, "Service"（服务）, "ApiGateway"（API网关）等。
        /// </summary>
        string InitiatorType { get; }

        /// <summary>
        /// 事件发起者的可读名称（可选）。
        /// 例如：用户的用户名、系统服务的友好名称等。便于日志阅读和理解。
        /// </summary>
        string? InitiatorName { get; }

        /// <summary>
        /// 相关性ID (Correlation ID)。
        /// 用于追踪单个逻辑请求在跨服务或跨模块间的传播路径。
        /// 它从请求发起开始贯穿整个处理流程，将所有相关的事件和操作串联起来。
        /// </summary>
        string? CorrelationId { get; }

        /// <summary>
        /// 因果关系ID (Causation ID)。
        /// 如果此事件是由另一个事件直接导致或引发的，此字段存储**导致当前事件的那个事件的 EventId**。
        /// 用于构建事件的因果链，了解事件之间的直接依赖关系。
        /// </summary>
        string? CausationId { get; } // 可选，如果当前事件没有直接前驱事件

}