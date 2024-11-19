using System.Diagnostics;
using System.Globalization;

// decouple from host culture
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

// install global event handlers
AppDomain.CurrentDomain.UnhandledException += EventHandlers.UnhandledExceptionHandler;
using CancellationTokenSource cts = new();
Console.CancelKeyPress += EventHandlers.GetCancelKeyPressHandler(cts);
CancellationToken cancellationToken = cts.Token;

try
{
    const double completion = 100d;
    string solution = "(not yet)";
    long allocatedMem = 0;
    Stopwatch sw = Stopwatch.StartNew();

    // what are we going to solve today? 
    PuzzleId id = Parser.ParseArguments(args);
    TypeLocator locator = new(id);
    string starsPerPuzzle = new('*', id.Puzzle);
    AnsiConsole.MarkupLineInterpolated(
        CultureInfo.CurrentCulture,
        $"[bold green]AoC[/] [green]{id.Year}[/] [darkgreen]day[/] [green]{id.Day}[/] [darkgreen]puzzle[/] [bold yellow]{starsPerPuzzle}[/] [link]https://adventofcode.com/{id.Year}/day/{id.Day}[/] ");
    string name = "(n/a)";
    await AnsiConsole.Progress()
        .Columns(
            new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(),
            new RemainingTimeColumn(), new SpinnerColumn(Spinner.Known.Christmas))
        .StartAsync(async ctx =>
        {
            ProgressTask[] tasks =
            [
                ctx.AddTask("Reflecting solver"),
                ctx.AddTask("Reading input"),
                ctx.AddTask("Solving puzzle")
            ];

            IPuzzleSolver solver = Reflector.CreateInstance<IPuzzleSolver>(locator.PuzzleTypeName);
            name = solver.Name;
            tasks[0].Value = completion;

            IList<string> input = await Reflector.ReadResourceTextLinesAsync(locator.AssemblyName, locator.InputResourceName, cancellationToken);
            tasks[1].Value = completion;

            long beforeMem = GC.GetTotalAllocatedBytes();
            solution = await solver.RunAsync(input, tasks[2], cancellationToken); // GO!
            allocatedMem = GC.GetTotalAllocatedBytes() - beforeMem;
            tasks[2].Value = completion;
        });

    // struck gold!
    AnsiConsole.MarkupLineInterpolated(CultureInfo.CurrentCulture,
        $"[darkgreen]Solution for [green]{name}[/][bold yellow]{starsPerPuzzle}[/] was found in [green]{sw.Elapsed:g}[/] with [green]{allocatedMem >> 10:N0}[/] kB allocated:[/]");
    AnsiConsole.WriteLine(solution);
}
catch (OperationCanceledException)
{
    AnsiConsole.MarkupLine("[italic red]Operation canceled[/]");
    Environment.Exit((int)ExitCode.OperationCanceled);
}
catch (ArgumentException e)
{
    AnsiConsole.MarkupLineInterpolated(CultureInfo.CurrentCulture, $"[italic red]Error: {e.Message}[/]");
    AnsiConsole.MarkupLine("Usage: [green]dotnet run <year> <day> <puzzle>[/]");
    Environment.Exit((int)ExitCode.InvalidArguments);
}