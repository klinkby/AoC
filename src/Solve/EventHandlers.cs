namespace Solve;

internal static class EventHandlers
{
    public static ConsoleCancelEventHandler GetCancelKeyPressHandler(CancellationTokenSource cancellationTokenSource)
    {
        return (_, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cancellationTokenSource.Cancel();
        };
    }

    public static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
    {
        AnsiConsole.WriteException((Exception)e.ExceptionObject);
        Environment.Exit((int)ExitCode.UnhandledException);
    }
}