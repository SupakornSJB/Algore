using Algore.Assignment_1.Models;

namespace Algore.Assignment_1.IntervalScheduling;

public static class IntervalSchedulingHelper
{
    public static List<Interval> GenerateTestSet(
        long n, 
        decimal alpha, 
        decimal intervalDuration)
    {
        var result = new List<Interval>();
        var random = new Random();
        var T = n * alpha * intervalDuration;
        
        for (long i = 0; i < n; i++)
        {
            var startTime = new decimal(random.NextDouble()) * T;
            var duration = 1m + new decimal(random.NextDouble()) * (intervalDuration - 1);
            
            result.Add(new Interval
            {
                Start = startTime,
                End = startTime + duration
            });
        }
        
        return result;   
    }
    
    public static List<Interval> EnumerateValidIntervals(List<Interval> intervals)
    {
        if (intervals.Count == 0)
        {
            return intervals;
        }
        
        var results = new List<Interval> { intervals[0] };
        var current = intervals[0];

        foreach (var interval in intervals.Skip(1))
        {
            if (!IsCompatible(current, interval))
            {
                continue;
            }
            
            current = interval;
            results.Add(current);
        }
        
        return results;
    }

    public static void PrintIntervals(List<Interval> intervals, long? elapsedTime = null)
    {
        Console.WriteLine($"Count: {intervals.Count}");
        
        if (elapsedTime != null)
        {
            Console.WriteLine($"Time: {elapsedTime} ms");
        }
        
        Console.Write("Intervals: ");
        foreach (var interval in intervals)
        {
            Console.Write($"{interval.Start} - {interval.End}, ");
        }
        Console.Write("\n\n");
    }

    private static bool IsCompatible(Interval first, Interval second)
    {
        return first.End <= second.Start || second.End < first.Start;
    }
}