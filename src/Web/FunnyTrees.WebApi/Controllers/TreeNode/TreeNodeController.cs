using FunnyTrees.Application.Contracts.Commands.TreeNode;
using FunnyTrees.Application.Contracts.Dispatching;
using FunnyTrees.Application.Contracts.Dto;
using FunnyTrees.Application.Contracts.Queries.TreeNode;
using FunnyTrees.Common.Types;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FunnyTrees.WebApi.Controllers.TreeNode;

[ApiController]
[Route("api/[controller]")]
public class TreeNodeController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public TreeNodeController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
        _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
    }

    [HttpGet("{nodeId:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetNodeAsync([FromRoute] int nodeId, CancellationToken cancellation)
    {
        var query = new GetByNodeIdQuery(nodeId);
        var result = await _queryDispatcher.DispatchAsync<GetByNodeIdQuery, TreeNodeDto?>(query, cancellation);
        return result == default
            ? NotFound()
            : Ok(result);
    }

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> GetTreeAsync([FromQuery] string rootNodeName, CancellationToken cancellation)
    {
        if (string.IsNullOrWhiteSpace(rootNodeName))
            return BadRequest($"{nameof(rootNodeName)} should be specified");

        var query = new GetByRootNameQuery(rootNodeName);
        var result = await _queryDispatcher.DispatchAsync<GetByRootNameQuery, TreeNodeDto?>(query, cancellation);
        return result == default
            ? NotFound()
            : Ok(result);
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateNodeAsync(
                                                    [FromQuery] string nodeName,
                                                    [FromQuery] int? parentId,
                                                    CancellationToken cancellation)
    {
        if (string.IsNullOrWhiteSpace(nodeName))
            return BadRequest($"{nameof(nodeName)} should be specified");

        var command = new CreateTreeNodeCommand(nodeName, parentId);
        var createdId = await _commandDispatcher.DispatchAsync<CreateTreeNodeCommand, int>(command, cancellation);
        return CreatedAtAction(nameof(GetNodeAsync), new { nodeId = createdId }, null);
    }

    [HttpPatch("rename/{nodeId:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> RenameNodeAsync(
                                                    [FromRoute] int nodeId,
                                                    [FromBody] string nodeName,
                                                    CancellationToken cancellation)
    {
        if (string.IsNullOrWhiteSpace(nodeName))
            return BadRequest($"{nameof(nodeName)} should be specified");

        var command = new RenameTreeNodeCommand(nodeId, nodeName);
        _ = await _commandDispatcher.DispatchAsync<RenameTreeNodeCommand, Unit>(command, cancellation);
        return NoContent();
    }

    [HttpDelete("{nodeId:int}")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Produces(MediaTypeNames.Application.Json)]
    public async Task<IActionResult> DeleteNodeAsync([FromRoute] int nodeId, CancellationToken cancellation)
    {
        var command = new DeleteTreeNodeCommand(nodeId);
        _ = await _commandDispatcher.DispatchAsync<DeleteTreeNodeCommand, Unit>(command, cancellation);
        return NoContent();
    }
}
