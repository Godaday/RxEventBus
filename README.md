# English | [简体中文](README.zh-cn.md)
# RxEventbus

A lightweight Rx.NET EventBus designed to help .NET applications achieve low coupling, asynchronous communication, and high extensibility between modules.

## 🚀 Core Features

* **Lightweight and Efficient**: Built upon Reactive Extensions for .NET (Rx.NET), providing an efficient event publishing and subscription mechanism.
* **Module Decoupling**: Achieves indirect communication between modules through an event-driven architecture, effectively reducing code coupling.
* **Asynchronous Event Handling**: Supports asynchronous event handlers, ensuring event processing does not block the main thread.
* **Automatic Registration and Subscription**: Automatically discovers and registers all event handlers, subscribing them upon application startup.
* **Global Error Handling**: Provides a default global event error handling mechanism, which can be overridden as needed.

## 💡 How to Use

### 1. Installation

Install the NuGet package in your .NET project using the NuGet Package Manager Console:

```bash
dotnet add package RxEventbus.Core
````

📦 Find more details about the NuGet package here: [RxEventbus.Core on NuGet.org](https://www.nuget.org/packages/RxEventbus.Core/)

### 2\. Service Registration

In your `Program.cs` or `Startup.cs` file, simply call the `AddEventBus()` extension method to register the EventBus and all event handlers.

```csharp
// Program.cs
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddEventBus(); // Register EventBus and Handlers
    }
}
```

### 3\. Defining Events

Define an event type object for your event. This is a simple C\# `record` example, recommended to be defined within your application's domain models or a designated events directory.

```csharp
// Example Event Payload
public record StockInEvent(int ProductId, int Quantity, string Location);
```

### 4\. Publishing Events

Obtain an `IAppEventBus` instance via dependency injection, then call its `Publish` method, passing an `AppEvent<T>` object where `T` is your event's payload type.

```csharp
using HaiyuEBR.Service.RxEventbus.Events; // Ensure this using directive is present

// Assume StockInEvent record is defined elsewhere, e.g., in a shared library or domain model
// public record StockInEvent(int ProductId, int Quantity, string Location);

public class OrderService
{
    private readonly IAppEventBus _eventBus;

    public OrderService(IAppEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void ProcessOrder(int productId, int quantity, string location)
    {
        // Create an AppEvent object with your specific payload
        var stockInPayload = new StockInEvent(productId, quantity, location);
        var stockInAppEvent = new AppEvent<StockInEvent>(stockInPayload);

        _eventBus.Publish(stockInAppEvent);
        Console.WriteLine($"Published Stock In Event: Product ID={stockInAppEvent.Payload.ProductId}");
    }
}
```

### 5\. Handling Events

To handle an event, simply create a class that implements the `IAppEventHandler<T>` interface, where `T` is the type of your event's payload. This handler will be automatically discovered and registered upon application startup.

```csharp
using HaiyuEBR.Service.RxEventbus; // Ensure this using directive is present
using HaiyuEBR.Service.RxEventbus.Events; // Ensure this using directive is present
using System.Threading.Tasks;

// Assume StockInEvent record is defined elsewhere
// public record StockInEvent(int ProductId, int Quantity, string Location);

public class TestStockInEventHandler : IAppEventHandler<StockInEvent>
{
    public Task HandleAsync(AppEvent<StockInEvent> evt)
    {
        Console.WriteLine($"[TestStockInEventHandler] Handling Stock In Event: Product ID={evt.Payload.ProductId}, Quantity={evt.Payload.Quantity}, Location={evt.Payload.Location}");
        return Task.CompletedTask;
    }

    // Optional: Override OnEventHandledAsync, OnErrorAsync, OnCompletedAsync for specific behavior
    public Task OnEventHandledAsync(AppEvent<StockInEvent> evt)
    {
        Console.WriteLine($"[TestStockInEventHandler] Finished handling Stock In Event for Product ID: {evt.Payload.ProductId}");
        return Task.CompletedTask;
    }
}
```

## 🐛  Console Output

```
Published Stock In Event: Product ID=101
[TestStockInEventHandler] Handling Stock In Event: Product ID=101, Quantity=50, Location=仓库A
[TestStockInEventHandler] Finished handling Stock In Event for Product ID: 101
```

## 🌐 Asynchronous & Concurrency Considerations

  * **Default Asynchronous Processing**: The `HandleAsync` method of `IAppEventHandler<T>` returns a `Task`, ensuring that event processing is non-blocking.
  * **Concurrency Control**: For time-consuming or CPU-intensive operations within event handlers, to prevent blocking the event publishing thread, you can add `.ObserveOn(System.Reactive.Concurrency.Scheduler.Default)` within the event bus's internal subscription chain. This offloads event processing to the thread pool for improved concurrency.

## 📋 Future Improvements (Roadmap)

  * Simplified event type configuration, filtering event types based on the event object itself, enhancing extensibility. (Completed)

## 🤝 Contributing

We welcome and appreciate your contributions\! If you have any questions, suggestions, or find a bug, please feel free to submit an [Issue](https://www.google.com/search?q=https://github.com/Godaday/RxEventBus/issues) or a [Pull Request](https://www.google.com/search?q=https://github.com/Godaday/RxEventBus/pulls) on GitHub.

## 📜 License

This project is released under the [MIT License](https://www.google.com/search?q=https://github.com/Godaday/RxEventBus/blob/main/LICENSE).

-----

© 2025 Godaday. All rights reserved.

