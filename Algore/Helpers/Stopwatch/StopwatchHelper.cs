namespace Algore.Helpers.Stopwatch;

public class StopwatchHelper
{
    public static double GetPreciseElapsedTime(System.Diagnostics.Stopwatch stopwatch) =>
        (double)stopwatch.ElapsedTicks / System.Diagnostics.Stopwatch.Frequency;
}