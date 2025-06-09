// RxEventbus.Tests/TestEventPayloads.cs


namespace RxEventbus.Tests
{
    // 定义一个通用的接口，用于分类订阅
    public interface IProductEventPayload { }

    // 假设这是入库事件的Payload
    public record StockInModel(int ProductId, int Quantity, string Location) : IProductEventPayload;

    // 假设这是出库事件的Payload (您已有，这里作为示例再定义一次，实际应使用您EventBus项目中的Record)
    // 为了避免冲突，如果TestStockOutEventHandler已经定义了StockOutModel，这里可以省略或命名不同。
    // 但是为了测试方便，最好让测试项目直接引用主项目的StockOutModel。
    // 这里我们假设主项目中的StockOutModel已经移除了对AppEventType的依赖。
    public record StockOutModel(int ProductId, int Quantity, string CustomerName) : IProductEventPayload;

    // 假设这是一个新的产品创建事件的Payload
    public record ProductCreatedModel(int ProductId, string ProductName, decimal Price) : IProductEventPayload;

    // 假设这是一个完全不相关的通用通知事件
    public record GenericNotificationModel(string Message);
}