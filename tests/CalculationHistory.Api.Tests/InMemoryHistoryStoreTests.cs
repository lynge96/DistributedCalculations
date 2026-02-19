using CalculationHistory.Worker.Implementation;
using Calculator.Domain.Events;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Shared.RabbitMq;

namespace History.Worker.Tests;

public class InMemoryHistoryStoreTests
{
    private static InMemoryHistoryStore CreateStub(int recordsToKeep)
    {
        var logger = NullLogger<InMemoryHistoryStore>.Instance;

        var options = Options.Create(new RabbitMqOptions
        {
            RecordsToKeep = recordsToKeep,
            Host = null!,
            Port = 0,
            Username = null!,
            Password = null!,
            Exchange = null!,
            QueueName = null!
        });

        return new InMemoryHistoryStore(logger, options);
    }
    
    [Fact]
    public void Add_stores_event_in_history()
    {
        var stub = CreateStub(recordsToKeep: 5);

        var evt = new CalculationCompletedEvent(
            Guid.NewGuid(),
            "2 + 2",
            4,
            DateTimeOffset.UtcNow);

        stub.Add(evt);

        var history = stub.GetHistory();

        Assert.Single(history);
        Assert.Equal(evt, history[0]);
    }
    
    [Fact]
    public void Add_removes_oldest_event_when_limit_is_exceeded()
    {
        var stub = CreateStub(recordsToKeep: 2);

        var first = new CalculationCompletedEvent(
            Guid.NewGuid(), "1 + 1", 2, DateTimeOffset.UtcNow);

        var second = new CalculationCompletedEvent(
            Guid.NewGuid(), "2 + 2", 4, DateTimeOffset.UtcNow);

        var third = new CalculationCompletedEvent(
            Guid.NewGuid(), "3 + 3", 6, DateTimeOffset.UtcNow);

        stub.Add(first);
        stub.Add(second);
        stub.Add(third);

        var history = stub.GetHistory();

        Assert.Equal(2, history.Count);
        Assert.DoesNotContain(first, history);
    }
    
    [Fact]
    public void Newest_event_is_returned_first()
    {
        var stub = CreateStub(recordsToKeep: 5);

        var first = new CalculationCompletedEvent(
            Guid.NewGuid(), "1 + 1", 2, DateTimeOffset.UtcNow);

        var second = new CalculationCompletedEvent(
            Guid.NewGuid(), "2 + 2", 4, DateTimeOffset.UtcNow);

        stub.Add(first);
        stub.Add(second);

        var history = stub.GetHistory();

        Assert.Equal(second, history[0]);
        Assert.Equal(first, history[1]);
    }
    
    [Fact]
    public void Clear_removes_all_history_records()
    {
        var stub = CreateStub(recordsToKeep: 5);

        var evt1 = new CalculationCompletedEvent(
            Guid.NewGuid(),
            "1 + 1",
            2,
            DateTimeOffset.UtcNow);

        var evt2 = new CalculationCompletedEvent(
            Guid.NewGuid(),
            "2 + 2",
            4,
            DateTimeOffset.UtcNow);

        stub.Add(evt1);
        stub.Add(evt2);

        Assert.Equal(2, stub.GetHistory().Count);
        
        stub.Clear();
        
        var history = stub.GetHistory();
        Assert.Empty(history);
    }
}