using System;

namespace FunnyTrees.Persistence.Entities.Models;

public class LogEntry
{
    public int Id { get; init; }
    public Guid EventId { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public string? QueryParams { get; init; }
    public string? BodyParams { get; init; }
    public string StackTrace { get; init; }
    public string Message { get; init; }
}
