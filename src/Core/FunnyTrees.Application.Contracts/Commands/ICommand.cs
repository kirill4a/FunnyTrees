namespace FunnyTrees.Application.Contracts.Commands;

/// <summary>
/// Marker interface for the CQRS commands
/// </summary>
/// <typeparam name="TResult">The type of returning result</typeparam>
public interface ICommand<TResult>
{
}