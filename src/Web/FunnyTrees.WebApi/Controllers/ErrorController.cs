using FunnyTrees.Application.Contracts.Dto;
using FunnyTrees.Application.Contracts.Services.ExceptionHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FunnyTrees.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public class ErrorController : ControllerBase
{
    private readonly IExceptionHandlerStrategy _exceptionHandlerStrategy;
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(IExceptionHandlerStrategy exceptionHandlerStrategy, ILogger<ErrorController> logger)
    {
        _exceptionHandlerStrategy = exceptionHandlerStrategy
            ?? throw new ArgumentNullException(nameof(exceptionHandlerStrategy));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [Route("error")]
    public async Task<IActionResult> ErrorAsync(CancellationToken cancellation)
    {
        ErrorDto? error;
        try
        {
            error = await HandleExceptionAsync(cancellation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            error = new()
            {
                Type = ErrorTypeDto.Unhandled,
                Data = new()
                {
                    Message = ex.ToString()
                }
            };
        }
        return StatusCode(StatusCodes.Status500InternalServerError, error);
    }

    private async Task<ErrorDto> HandleExceptionAsync(CancellationToken cancellation)
    {
        var traceId = HttpContext.TraceIdentifier;

        var request = HttpContext.Features.Get<IHttpRequestFeature>()
            ?? throw new NullReferenceException($"Couldn\'t extract request from HttpContext '{traceId}'");

        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error
            ?? throw new NullReferenceException($"Couldn\'t extract exception from HttpContext '{traceId}'");

        var url = request.RawTarget;
        var method = request.Method;

        string body;
        request.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(request.Body, Encoding.UTF8, true, -1, true);
        body = await reader.ReadToEndAsync();

        _logger.LogError(exception,
            "Unhandled exception in FunnyTrees.WebApi. URL: {@url}. Method: {@method}. Request body: {@body}",
            url, method, body);

        var wrapper = new ExceptionWrapper(exception, Guid.NewGuid(), DateTimeOffset.UtcNow, url, body);
        var result = await _exceptionHandlerStrategy.ExecuteAsync(wrapper, cancellation);
        return result;
    }
}
