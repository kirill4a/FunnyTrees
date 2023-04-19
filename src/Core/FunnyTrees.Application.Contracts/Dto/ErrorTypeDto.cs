using System.Text.Json.Serialization;

namespace FunnyTrees.Application.Contracts.Dto;

/// <summary>
/// Types of errors
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorTypeDto
{
    /// <summary>
    /// Unhandled exception
    /// </summary>
    Unhandled,

    /// <summary>
    /// Common exception
    /// </summary>
    Common,

    /// <summary>
    /// Secure exception
    /// </summary>
    Secure
}
