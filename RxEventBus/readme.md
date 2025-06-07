```markdown
# RxEventbus.Core

一个基于 Rx.NET 的轻量级事件总线，旨在帮助 .NET 应用程序实现模块间的低耦合、异步通信和降低并发性。

## 🚀 核心特性

* **轻量与高效**: 基于 Reactive Extensions for .NET (Rx.NET) 构建，提供高效的事件发布和订阅机制。
* **模块解耦**: 通过事件驱动架构，实现模块间的间接通信，有效降低代码耦合度。
* **异步事件处理**: 支持异步事件处理程序，确保事件处理不会阻塞主线程。
* **自动注册与订阅**: 只需一行代码即可自动发现、注册所有事件处理程序，并在应用启动时自动订阅。
* **全局错误处理**: 提供默认的全局事件错误处理机制，可根据需要重写。

## 💡 如何使用

### 1. 安装 NuGet 包

在您的 .NET 项目中，通过 NuGet 包管理器控制台安装：

```bash
dotnet add package RxEventbus.Core
```

### 2. 简化服务注册

在您的 `Program.cs` 或 `Startup.cs` 文件中，只需调用 `AddRxEventBus()` 扩展方法即可完成 EventBus 和所有事件处理程序的注册：

```csharp
// Program.cs 或 Startup.cs
using  RxEventBus.Core; // 确保添加此 using 引用

var builder = WebApplication.CreateBuilder(args);

// 只需一行代码即可注册整个 RxEventBus 
builder.Services.AddRxEventBus();



### 3. 定义事件

首先，定义一个事件类型枚举：

```csharp
// RxEventBus.Core.Events.cs
namespace RxEventBus.Core.Events
{
    public enum AppEventType
    {
        StockOutEvent, // 出库事件
        StockInEvent,  // 入库事件
        OtherEvent
    }
}
```

然后，为您的事件定义一个 `Payload` 类型对象。这是一个简单的 C# `record` 示例：

```csharp
// 示例: StockOutModel.cs
public record StockOutModel(int Id, string Name);
```

### 4. 发布事件

通过依赖注入获取 `IAppEventBus` 实例，然后调用其 `Publish` 方法并传入您的 `Payload` 类型对象：

```csharp
using HRxEventBus.Core.Events; // 确保添加此 using 引用

public class OrderService
{
    private readonly IAppEventBus _eventBus;

    public OrderService(IAppEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void PerformStockOut(int productId, string productName)
    {
        // 创建 Payload 对象
        var stockOutData = new StockOutModel(productId, productName);
        
        // 发布事件，只需传入事件类型和 Payload 对象
        _eventBus.Publish(new AppEvent<StockOutModel>(AppEventType.StockOutEvent, stockOutData));
    }
}
```

### 5. 处理事件

要处理事件，您只需创建一个类并实现 `IAppEventHandler<T>` 接口，其中 `T` 是您事件的 `Payload` 类型：

```csharp
using RxEventBus.Core.Events;
using System.Diagnostics;

namespace RxEventbus.EventHandlers
{
    // 假设 StockOutModel 已经定义
    public record StockOutModel(int Id, string Name);

    /// <summary>
    /// 测试出库事件处理器
    /// </summary>
    public class TestStockOutEventHandler : IAppEventHandler<StockOutModel>
    {
        // 指定此处理器要监听的事件类型
        public AppEventType EventType => AppEventType.StockOutEvent;

        /// <summary>
        /// 事件处理方法，异步执行
        /// </summary>
        /// <param name="evt">包含事件 Payload 的事件对象</param>
        public Task HandleAsync(AppEvent<StockOutModel> evt)
        {
            Debug.WriteLine($"处理出库事件: ID={evt.Payload.Id}, Name={evt.Payload.Name}");
            // 执行您的业务逻辑
            return Task.CompletedTask;
        }

       
    }
}
```

## 🌐 异步与并发考量

* **默认异步处理**: `HandleAsync` 方法是异步的，有助于防止阻塞。
* **并发控制**: 对于耗时或 CPU 密集型操作，为了避免阻塞事件发布线程，您可以在事件总线内部的订阅链中添加 `.ObserveOn(System.Reactive.Concurrency.Scheduler.Default)` 来将事件处理卸载到线程池中，以实现更好的并发性。


## 待改进

* **事件类型**  取消事件类型的枚举，使用PlayLoad类型，作为处理标识 简化操作

## 🤝 贡献

我们欢迎并感谢您的贡献！如果您有任何问题、建议或发现了 bug，请随时在 GitHub 上提交 Issue 或 Pull Request。

## 📜 许可证

本项目在 MIT 许可证下发布。
```