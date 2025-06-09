
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RxEventBus.Core.Events;
using RxEventBus.Core;
using System.Diagnostics;


namespace RxEventbus.Tests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // 1. 构建主机 (Host)
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // 2. 注册事件总线为单例
                    services.AddRxEventBus();

        
                    // 显式注册我们在这个测试项目中定义的 Handlers
                    // AddAppEventHandlers 应该会自动扫描并注册，但显式注册可以确保。
                    // 实际上，如果 EventHandlerExtensions.cs 扫描的是 Assembly.GetExecutingAssembly()
                    // 并且 TestEventHandlers.cs 也在这个测试项目的程序集中，那么 AddAppEventHandlers() 会自动注册。
                    // 如果您的 EventBus 库和测试项目在不同的程序集，且 AddAppEventHandlers 只扫描EventBus程序集
                    // 您可能需要修改 AddAppEventHandlers 允许指定要扫描的程序集，或者手动注册这里的 Handlers。
                    // 为了简化，假设当前测试项目包含这些Handlers。
                    services.AddTransient<TestStockInEventHandler>();
                    services.AddTransient<TestStockOutEventHandler>();
                    services.AddTransient<ProductEventHandler>();
                    services.AddTransient<GenericNotificationEventHandler>();

                })
                .Build();

            // 4. 启动主机，这将同时启动 AppEventHandlerInitializer 后台服务
            await host.StartAsync();

            // 5. 从服务提供者中获取 IAppEventBus 实例
            var eventBus = host.Services.GetRequiredService<IAppEventBus>();

            Console.WriteLine("\n--- 发布事件 ---");

            // 发布一个入库事件
            var stockInEvent = new AppEvent<StockInModel>(new StockInModel(101, 50, "仓库A"));
            eventBus.Publish(stockInEvent);
            Console.WriteLine($"已发布入库事件: 产品ID={stockInEvent.Payload.ProductId}");

            await Task.Delay(50); // 稍微延迟，让事件有时间处理

            // 发布一个出库事件
            var stockOutEvent = new AppEvent<StockOutModel>(new StockOutModel(201, 10, "客户X"));
            eventBus.Publish(stockOutEvent);
            Console.WriteLine($"已发布出库事件: 产品ID={stockOutEvent.Payload.ProductId}");

            await Task.Delay(50);

            // 发布一个产品创建事件
            var productCreatedEvent = new AppEvent<ProductCreatedModel>(new ProductCreatedModel(301, "新产品XYZ", 99.99m));
            eventBus.Publish(productCreatedEvent);
            Console.WriteLine($"已发布产品创建事件: 名称={productCreatedEvent.Payload.ProductName}");

            await Task.Delay(50);

            // 发布一个通用通知事件 (不属于 IProductEventPayload 类别)
            var genericNotificationEvent = new AppEvent<GenericNotificationModel>(new GenericNotificationModel("系统维护即将开始！"));
            eventBus.Publish(genericNotificationEvent);
            Console.WriteLine($"已发布通用通知事件: 消息='{genericNotificationEvent.Payload.Message}'");


            Console.WriteLine("\n--- 事件发布完成，等待处理结果 ---");

            // 等待一段时间，让所有异步事件处理完成
            await Task.Delay(2000); // 2 秒

            Console.WriteLine("\n--- 测试完成 ---");

            Console.ReadLine();
            // 停止主机
            await host.StopAsync();
        }
    }
}