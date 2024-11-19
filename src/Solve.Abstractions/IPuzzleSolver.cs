namespace Solve.Abstractions;

/// <summary>
///     Defines a puzzle solver.
/// </summary>
public interface IPuzzleSolver
{
    /// <summary>
    ///     Solves the puzzle.
    /// </summary>
    /// <param name="input">Daily input as lines of text.</param>
    /// <param name="progress">Report progress during the operation.</param>
    /// <param name="cancellationToken">Signals to cancel the operation prematurely.</param>
    /// <returns>The computed solution as text.</returns>
    Task<string> RunAsync(IList<string> input, IProgress<double> progress, CancellationToken cancellationToken);
}