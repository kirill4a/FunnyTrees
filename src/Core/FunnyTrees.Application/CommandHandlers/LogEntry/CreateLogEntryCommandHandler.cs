using FunnyTrees.Application.Contracts.Commands;
using FunnyTrees.Application.Contracts.Commands.LogEntry;
using FunnyTrees.Domain.Repositories;
using DomainLogEntry = FunnyTrees.Domain.Aggregates.LogEntry;

namespace FunnyTrees.Application.CommandHandlers.LogEntry;

internal class CreateLogEntryCommandHandler : ICommandHandler<CreateLogEntryCommand, int>
{
    private readonly ILogEntryRepository _logEntryRepository;

    public CreateLogEntryCommandHandler(ILogEntryRepository logEntryRepository)
    {
        _logEntryRepository = logEntryRepository ?? throw new ArgumentNullException(nameof(logEntryRepository));
    }

    public async Task<int> HandleAsync(CreateLogEntryCommand command, CancellationToken cancellation)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var domainEntity = new DomainLogEntry(
            0,
            command.EventId,
            command.Timestamp,
            command.QueryParams,
            command.BodyParam,
            command.StackTrace,
            command.Message);

        var result = await _logEntryRepository.CreateAsync(domainEntity, cancellation).ConfigureAwait(false);
        return result;
    }
}
