namespace FunnyTrees.Domain.Aggregates;

public class LogEntry : IAggregateRoot
{
    public LogEntry(
        int id,
        Guid eventId,
        DateTimeOffset timestamp,
        IEnumerable<string> queryParams,
        string bodyParam,
        string stackTrace,
        string message)
    {
        if (eventId == default)
            throw new ArgumentNullException(nameof(eventId));

        if (string.IsNullOrWhiteSpace(stackTrace))
            throw new ArgumentNullException(nameof(stackTrace));

        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentNullException(nameof(message));

        Id = id;
        EventId = eventId;
        Timestamp = timestamp;
        QueryParams = queryParams;
        BodyParam = bodyParam;
        StackTrace = stackTrace;
        Message = message;
    }

    public int Id { get; }
    public Guid EventId { get; }
    public DateTimeOffset Timestamp { get; }
    public IEnumerable<string> QueryParams { get; }
    public string BodyParam { get; }
    public string StackTrace { get; }
    public string Message { get; }
}
