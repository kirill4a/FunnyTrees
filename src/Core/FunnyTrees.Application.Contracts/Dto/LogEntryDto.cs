using System;
using System.Collections.Generic;

namespace FunnyTrees.Application.Contracts.Dto;

/// <summary>
/// The entry of the log
/// </summary>
public class LogEntryDto
{
    /// <summary>
    /// Id of the entry
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Id of the event
    /// </summary>
    public Guid EventId { get; init; }

    /// <summary>
    /// Timestamp of the event
    /// </summary>
    public DateTimeOffset Timestamp { get; init; }

    /// <summary>
    /// Query parameters of the request
    /// </summary>
    public IEnumerable<string> QueryParams { get; init; }

    /// <summary>
    /// Body parameters of the request
    /// </summary>
    public IEnumerable<string> BodyParams { get; init; }

    /// <summary>
    /// Exception stack trace
    /// </summary>
    public string StackTrace { get; init; }

    /// <summary>
    /// Exception message
    /// </summary>
    public string Message { get; init; }
}
