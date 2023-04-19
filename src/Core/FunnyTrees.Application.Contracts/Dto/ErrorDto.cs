using System;

namespace FunnyTrees.Application.Contracts.Dto;

/// <summary>
/// Object containg information about any error
/// </summary>
public class ErrorDto
{
    /// <summary>
    /// Event id
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Error type
    /// </summary>
    public ErrorTypeDto Type { get; init; }

    /// <summary>
    /// Error data
    /// </summary>
    public ErrorDataDto Data { get; init; }
}
