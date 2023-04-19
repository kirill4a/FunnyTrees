using FunnyTrees.Application.Contracts.Commands.LogEntry;
using FunnyTrees.Application.Contracts.Dispatching;
using FunnyTrees.Application.Contracts.Services.ExceptionHandling;

namespace FunnyTrees.Application.Services.ExceptionHandling;

public abstract class BaseExceptionHandler
{
    private readonly Uri _fakeBaseUri = new("http://localhost");
    private readonly ICommandDispatcher _dispatcher;

    protected BaseExceptionHandler(ICommandDispatcher dispatcher)
    {
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
    }

    protected async Task DispathcException(ExceptionWrapper wrapper, CancellationToken cancellation)
    {
        if (!Uri.TryCreate(wrapper.RequestUrl, UriKind.RelativeOrAbsolute, out var uri))
            throw new Exception($"Value '{wrapper.RequestUrl}' is not correct uri");

        var queryParams = ExtractQueryParams(uri);
        var createLogEntryCommand = new CreateLogEntryCommand(
                                                wrapper.EventId,
                                                wrapper.Timestamp,
                                                queryParams,
                                                wrapper.RequestBody,
                                                wrapper.Error.StackTrace!,
                                                wrapper.Error.Message);

        _ = await _dispatcher.DispatchAsync<CreateLogEntryCommand, int>(createLogEntryCommand, cancellation)
                         .ConfigureAwait(false);
    }

    private IEnumerable<string> ExtractQueryParams(Uri uri)
    {
        if (!uri.IsAbsoluteUri)
            uri = new Uri(_fakeBaseUri, uri);

        return string.IsNullOrWhiteSpace(uri.Query)
                ? Enumerable.Empty<string>()
                : uri.Query.Split('&');
    }
}