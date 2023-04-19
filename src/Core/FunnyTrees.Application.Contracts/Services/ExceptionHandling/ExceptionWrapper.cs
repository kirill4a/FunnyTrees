using System;
using System.Collections.Generic;

namespace FunnyTrees.Application.Contracts.Services.ExceptionHandling;

public record ExceptionWrapper
{
    public ExceptionWrapper(
        Exception error,
        Guid eventId,
        DateTimeOffset timestamp,
        string requestUrl,
        string requestBody)
    {
        if (eventId == default)
            throw new ArgumentNullException(nameof(eventId));
        if (string.IsNullOrWhiteSpace(requestUrl))
            throw new ArgumentNullException(nameof(requestUrl));

        Error = error ?? throw new ArgumentNullException(nameof(error));
        EventId = eventId;
        Timestamp = timestamp;
        RequestUrl = requestUrl;
        RequestBody = requestBody;
    }

    public Exception Error { get; }
    public Guid EventId { get; }
    public DateTimeOffset Timestamp { get; }
    public string RequestUrl { get; }
    public string RequestBody { get; }
}
