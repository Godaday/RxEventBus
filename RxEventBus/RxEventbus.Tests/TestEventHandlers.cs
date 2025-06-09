using RxEventBus.Core;
using RxEventBus.Core.Events;

namespace RxEventbus.Tests
{
    /// <summary>
    /// 测试入库事件处理器
    /// </summary>
    public class TestStockInEventHandler : IAppEventHandler<StockInModel>
    {
        public Task HandleAsync(AppEvent<StockInModel> evt)
        {
            Console.WriteLine($"[TestStockInEventHandler] 处理入库事件: 产品ID={evt.Payload.ProductId}, 数量={evt.Payload.Quantity}, 位置={evt.Payload.Location}");
            return Task.CompletedTask;
        }

        //public Task OnEventHandledAsync(AppEvent<StockInModel> evt)
        //{
           
        //    return Task.CompletedTask;
        //}
    }

    /// <summary>
    /// 测试出库事件处理器 (假设您在主项目中已修改为不再依赖 AppEventType)
    /// </summary>
    public class TestStockOutEventHandler : IAppEventHandler<StockOutModel>
    {
        public Task HandleAsync(AppEvent<StockOutModel> evt)
        {
            Console.WriteLine($"[TestStockOutEventHandler] 处理出库事件: 产品ID={evt.Payload.ProductId}, 数量={evt.Payload.Quantity}, 客户={evt.Payload.CustomerName}");
            return Task.CompletedTask;
        }

        //public Task OnEventHandledAsync(string eventTypeName)
        //{
        //    Console.WriteLine($"[TestStockOutEventHandler] 事件流 '{eventTypeName}' 完成。");
        //    return Task.CompletedTask;
        //}
    }

    /// <summary>
    /// 处理所有产品相关事件的通用处理器 (演示分类订阅)
    /// </summary>
    public class ProductEventHandler : IAppEventHandler<ProductCreatedModel>
    {
        public Task HandleAsync(AppEvent<ProductCreatedModel> evt)
        {
            Console.WriteLine($"[ProductEventHandler] 收到产品事件: 类型={evt.Payload.GetType().Name}, 时间={evt.Timestamp}");

           
            return Task.CompletedTask;
        }

        //public Task OnCompletedAsync(string eventTypeName)
        //{
        //    Console.WriteLine($"4[ProductEventHandler] 事件流 '{eventTypeName}' 完成。");
        //    return Task.CompletedTask;
        //}
    }

    /// <summary>
    /// 处理通用通知事件的处理器
    /// </summary>
    public class GenericNotificationEventHandler : IAppEventHandler<GenericNotificationModel>
    {
        public Task HandleAsync(AppEvent<GenericNotificationModel> evt)
        {
            Console.WriteLine($"[GenericNotificationEventHandler] 收到通用通知: '{evt.Payload.Message}'");
            return Task.CompletedTask;
        }

        //public Task OnCompletedAsync(string eventTypeName)
        //{
        //    Console.WriteLine($"[GenericNotificationEventHandler] 事件流 '{eventTypeName}' 完成。");
        //    return Task.CompletedTask;
        //}
    }
}