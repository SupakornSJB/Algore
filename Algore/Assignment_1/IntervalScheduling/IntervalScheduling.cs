using Algore.Assignment_1.Models;
using Algore.Helpers.Combination;
using Algore.Helpers.Sort;

namespace Algore.Assignment_1.IntervalScheduling;

public static class IntervalScheduling
{
    public static List<Interval> EarliestFinishTime(List<Interval> intervals)
    {
        IComparer<Interval> comparer = Comparer<Interval>.Create((x, y) => x.End.CompareTo(y.End));
        return Greedy(intervals, comparer);
    }    
    
    public static List<Interval> EarliestStartTime(List<Interval> intervals)
    {
        IComparer<Interval> comparer = Comparer<Interval>.Create((x, y) => x.Start.CompareTo(y.Start));
        return Greedy(intervals, comparer);
    }

    public static List<Interval> ShortestDuration(List<Interval> intervals)
    {
        IComparer<Interval> comparer = Comparer<Interval>.Create((x, y) =>
        {
            var xDiff = x.End - x.Start;    
            var yDiff = y.End - y.Start;
            
            return xDiff.CompareTo(yDiff);
        });
        return Greedy(intervals, comparer);
    }

    public static List<List<Interval>> Exhaustive(List<Interval> intervals)
    {
        var comparer = Comparer<Interval>.Create((x, y) => x.End.CompareTo(y.End));
        var allCombinations = CombinationHelper.GenerateAllCombinations(intervals);
        
        var currentBestCombinationCount = 0;
        var allPossibleAnswers = new List<List<Interval>>();
        
        foreach (var combination in allCombinations)
        {
            var combinationResults = Greedy(combination, comparer);

            if (combinationResults.Count != combination.Count) continue; // This combination is invalid
            if (currentBestCombinationCount > combinationResults.Count) continue; // Not the optimal solution
            
            if (currentBestCombinationCount == combinationResults.Count)
            {
                allPossibleAnswers.Add(combinationResults);
                continue;
            }
            
            allPossibleAnswers = [combinationResults]; 
            currentBestCombinationCount = combinationResults.Count; 
        }

        return allPossibleAnswers;
    }

    private static List<Interval> Greedy(List<Interval> intervals, IComparer<Interval> comparer)
    {
        SortImplementation.MergeSort(intervals, true, comparer);
        return IntervalSchedulingHelper.EnumerateValidIntervals(intervals);
    }
}