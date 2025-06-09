

# RxEventbus

一个基于 Rx.NET 的轻量级事件总线，旨在帮助 .NET 应用程序实现模块间的低耦合、异步通信和降低并发性。

## 🚀 核心特性

* **轻量与高效**: 基于 Reactive Extensions for .NET (Rx.NET) 构建，提供高效的事件发布和订阅机制。
* **模块解耦**: 通过事件驱动架构，实现模块间的间接通信，有效降低代码耦合度。
* **异步事件处理**: 支持异步事件处理程序，确保事件处理不会阻塞主线程。
* **自动注册与订阅**: 可自动发现、注册所有事件处理程序，并在应用启动时自动订阅。
* **全局错误处理**: 提供默认的全局事件错误处理机制，可根据需要重写。

## 💡 如何使用

### 1. 安装 NuGet 包

在您的 .NET 项目中，通过 NuGet 包管理器控制台安装：

```bash
dotnet add package RxEventbus.Core
````
📦 您可以在这里查看 NuGet 包的详细信息：[RxEventbus.Core on NuGet.org](https://www.nuget.org/packages/RxEventbus.Core/)

### 2\. 服务注册

在您的 `Program.cs` 或 `Startup.cs` 文件中，只需调用 `AddRxEventBus()` 扩展方法即可完成 EventBus 和所有事件处理程序的注册。

```csharp
// Program.cs
 public class Program
 {
     public static void Main(string[] args)
     {
         var builder = WebApplication.CreateBuilder(args);
         builder.Services.AddRxEventBus();//注册事件总线
     }
 }
```

### 3\. 定义布事件

```C#
  // 假设这是入库事件的Payload
  public record StockInEvent(int ProductId, int Quantity, string Location) 
  var stockInEvent = new AppEvent<StockInEvent>(new StockInEvent(101, 50, "仓库A"));
```

然后，为您的事件定义一个 `Payload` 类型对象。这是一个简单的 C\# `record` 示例，推荐将其定义在您应用程序的领域模型或约定好的事件目录中：

### 4\. 发布事件

通过依赖注入获取 `IAppEventBus` 实例，然后调用其 `Publish` 方法并传入您的 `Payload` 类型对象。事件总线会自动包装您的 `Payload` 到 `AppEvent<TPayload>` 中。

```csharp
using RxEventbus.Core.Events; // 确保添加此 using 引用

public class OrderService
{
    private readonly IAppEventBus _eventBus;

    public OrderService(IAppEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void PerformStockOut(int productId, string productName)
    {
        // 创建 Event 对象
        public record StockInEvent(int ProductId, int Quantity, string Location) 
  var stockInEvent = new AppEvent<StockInEvent>(new StockInEvent(101, 50, "仓库A"));
        _eventBus.Publish(stockInEvent);
         Console.WriteLine($"已发布入库事件: 产品ID={stockInEvent.Payload.ProductId}");
    }
}
```

### 5\. 处理事件

要处理事件，您只需创建一个类并实现 `IAppEventHandler<T>` 接口，其中 `T` 是您事件的 `Payload` 类型。该处理器将在应用启动时被自动发现和注册。

```C#
  public class TestStockInEventHandler : IAppEventHandler<StockInEvent>
  {
      public Task HandleAsync(AppEvent<StockInEvent> evt)
      {
          Console.WriteLine($"[TestStockInEventHandler] 处理入库事件: 产品ID={evt.Payload.ProductId}, 数量={evt.Payload.Quantity}, 位置={evt.Payload.Location}");
          return Task.CompletedTask;
      }
  }
```

## Console

```C#
已发布入库事件: 产品ID=101
[TestStockInEventHandler] 处理入库事件: 产品ID=101, 数量=50, 位置=仓库A
```



## 🌐 异步与并发考量

  * **默认异步处理**: `IAppEventHandler<T>` 的 `HandleAsync` 方法返回 `Task`，确保事件处理是非阻塞的。
  * **并发控制**: 对于事件处理程序中包含的耗时或 CPU 密集型操作，为了避免阻塞事件发布线程，您可以在事件总线内部的订阅链中添加 `.ObserveOn(System.Reactive.Concurrency.Scheduler.Default)` 来将事件处理卸载到线程池中，以实现更好的并发性。

## 📋 未来改进 (Roadmap)

  * 简化事件类型的配置，根据事件对象过滤事件类型，提高扩展性。(已完成)

## 🤝 贡献

我们欢迎并感谢您的贡献！如果您有任何问题、建议或发现了 bug，请随时在 GitHub 上提交 [Issue](https://www.google.com/search?q=https://github.com/Godaday/RxEventBus/issues) 或 [Pull Request](https://www.google.com/search?q=https://github.com/Godaday/RxEventBus/pulls)。

## 📜 许可证

本项目在 [MIT 许可证](https://www.google.com/search?q=https://github.com/Godaday/RxEventBus/blob/main/LICENSE) 下发布。

-----

© 2025 Godaday. All rights reserved.
