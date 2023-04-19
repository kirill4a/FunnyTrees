using System;
using System.Collections.Generic;

namespace FunnyTrees.Application.Contracts.Commands.LogEntry;

public record CreateLogEntryCommand(
                                Guid EventId,
                                DateTimeOffset Timestamp,
                                IEnumerable<string> QueryParams,
                                string BodyParam,
                                string StackTrace,
                                string Message)
    : ICommand<int>;
