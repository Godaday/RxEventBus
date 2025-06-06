
using RxEventBus.Core.Events;
using System.Diagnostics;

namespace RxEventBus.Core.EventHandlers
{
   
    /// <summary>
    /// 测试record 
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    public record StockOutModel(int Id, string Name);
    /// <summary>
    /// 测试出库事件处理器
    /// </summary>
    public class TestStockOutEventHandler : IAppEventHandler<StockOutModel>
    {

        public AppEventType EventType => AppEventType.StockOutEvent;

        /// <summary>
        /// 事件处理方法
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public Task HandleAsync(AppEvent<StockOutModel> evt)
        {
            Debug.WriteLine($"处理出库事件: {evt.Payload.Id} - {evt.Payload.Name}");
   
            return Task.CompletedTask;
        }
        

    }
}
