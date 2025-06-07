

# RxEventbus

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
````
📦 您可以在这里查看 NuGet 包的详细信息：[RxEventbus.Core on NuGet.org](https://www.nuget.org/packages/RxEventbus.Core/)
### 2\. 简化服务注册

在您的 `Program.cs` 或 `Startup.cs` 文件中，只需调用 `AddRxEventBus()` 扩展方法即可完成 EventBus 和所有事件处理程序的注册。确保您的 `RxEventbus.Core` 项目中包含一个名为 `AddRxEventBus` 的静态扩展方法，它内部会调用 `AddEventBus()` 和 `AddAppEventHandlers()`。

```csharp
// Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RxEventbus.Core; // 确保添加此 using 引用

var builder = WebApplication.CreateBuilder(args);

// 只需一行代码即可注册整个 RxEventBus 系统
builder.Services.AddRxEventBus();

// ... 其他服务注册（例如 AddControllersWithViews(), AddSwaggerGen() 等）

var app = builder.Build();

// ... 其他应用配置 (例如 UseRouting(), UseAuthentication(), UseAuthorization() 等)

app.Run();
```

> **注意**: `AddRxEventBus()` 扩展方法假定您已将其实现为合并了 `AddEventBus()` 和 `AddAppEventHandlers()` 功能的方法，以便提供简洁的注册体验。

### 3\. 定义事件

首先，您需要一个枚举来定义不同的事件类型（在您的 `RxEventbus.Core` 项目中）：

```csharp
// RxEventbus.Core/Events/EventType.cs
namespace RxEventbus.Core.Events
{
    public enum AppEventType
    {
        StockOutEvent, // 出库事件
        StockInEvent,  // 入库事件
        OtherEvent
    }
}
```

然后，为您的事件定义一个 `Payload` 类型对象。这是一个简单的 C\# `record` 示例，推荐将其定义在您应用程序的领域模型或约定好的事件目录中：

```csharp
// 示例: StockOutModel.cs (在您的应用项目中定义)
public record StockOutModel(int Id, string Name);
```

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
        // 创建 Payload 对象
        var stockOutData = new StockOutModel(productId, productName);
        
        // 发布事件，只需传入事件类型和 Payload 对象
        // EventBus 会自动创建 AppEvent<StockOutModel>
        _eventBus.Publish(new AppEvent<StockOutModel>(AppEventType.StockOutEvent, stockOutData));
    }
}
```

### 5\. 处理事件

要处理事件，您只需创建一个类并实现 `IAppEventHandler<T>` 接口，其中 `T` 是您事件的 `Payload` 类型。该处理器将在应用启动时被自动发现和注册。

```csharp
using RxEventbus.Core.Events; // 确保添加此 using 引用
using RxEventbus.Core;       // 确保添加此 using 引用 (IAppEventHandler 在此命名空间)
using System.Diagnostics;
using System.Threading.Tasks;

// 建议将事件处理器定义在您的应用程序自己的命名空间下，
// 例如 YourApp.EventHandlers
namespace YourApp.EventHandlers 
{
    // 假设 StockOutModel 已经定义在可访问的命名空间中
    // public record StockOutModel(int Id, string Name);

    /// <summary>
    /// 测试出库事件处理器
    /// </summary>
    public class TestStockOutEventHandler : IAppEventHandler<StockOutModel>
    {
        // 指定此处理器要监听的事件类型
        public AppEventType EventType => AppEventType.StockOutEvent;

        /// <summary>
        /// 事件处理方法，异步执行。
        /// 当一个 StockOutEvent 类型的事件被发布时，此方法将被调用。
        /// </summary>
        /// <param name="evt">包含事件 Payload 的事件对象</param>
        public Task HandleAsync(AppEvent<StockOutModel> evt)
        {
            Debug.WriteLine($"处理出库事件: ID={evt.Payload.Id}, Name={evt.Payload.Name}");
            // 在这里执行您的业务逻辑，例如更新数据库、发送通知等
            return Task.CompletedTask;
        }

        // 可选：重写默认的错误处理行为（当 HandleAsync 抛出未捕获异常时）
        // public override Task OnErrorAsync(AppEvent<StockOutModel> evt, Exception ex)
        // {
        //     Debug.WriteLine($"[CustomError] 处理出库事件 {evt.Payload.Name} 时发生错误: {ex.Message}");
        //     // 可以在这里进行特定的错误日志记录或补偿逻辑
        //     return Task.CompletedTask;
        // }

        // 可选：重写默认的完成处理行为（当事件流完成时，通常在 EventBus 停止时触发）
        // public override Task OnCompletedAsync(AppEventType eventType)
        // {
        //     Debug.WriteLine($"[CustomCompleted] {eventType} 事件处理流程已完成.");
        //     return Task.CompletedTask;
        // }
    }
}
```

## 🌐 异步与并发考量

  * **默认异步处理**: `IAppEventHandler<T>` 的 `HandleAsync` 方法返回 `Task`，确保事件处理是非阻塞的。
  * **并发控制**: 对于事件处理程序中包含的耗时或 CPU 密集型操作，为了避免阻塞事件发布线程，您可以在事件总线内部的订阅链中添加 `.ObserveOn(System.Reactive.Concurrency.Scheduler.Default)` 来将事件处理卸载到线程池中，以实现更好的并发性。

## 📋 未来改进 (Roadmap)

  * **事件类型可扩展性**: 当前的事件类型基于 `AppEventType` 枚举。未来版本计划取消事件类型的枚举限制，改为**使用 Payload 类型本身作为事件的唯一标识符**，从而允许用户在使用库时自由定义和扩展任何自定义事件类型，无需修改库的源代码。这将使库更加灵活和可扩展。
  * **更细粒度的错误处理配置**: 考虑提供更灵活的错误处理配置，例如为特定事件或处理程序指定自定义的错误处理策略。

## 🤝 贡献

我们欢迎并感谢您的贡献！如果您有任何问题、建议或发现了 bug，请随时在 GitHub 上提交 [Issue](https://www.google.com/search?q=https://github.com/Godaday/RxEventBus/issues) 或 [Pull Request](https://www.google.com/search?q=https://github.com/Godaday/RxEventBus/pulls)。

## 📜 许可证

本项目在 [MIT 许可证](https://www.google.com/search?q=https://github.com/Godaday/RxEventBus/blob/main/LICENSE) 下发布。

-----

© 2025 Godaday. All rights reserved.

```
```
