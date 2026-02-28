namespace Algore.Helpers.TestSet;

public static class IntegerTestSetGeneratorHelper
{
    public static List<int> GenerateTestSet(int n)
    {
        return Enumerable.Range(1, n).ToList();
    }
    
    /* Implement this function */
    // Generate a list of length n of random number between min and max (inclusive)
    public static List<int> GenerateRandomNumbers(int n, int min, int max)
    {
        if (n < 0)
            throw new ArgumentOutOfRangeException(nameof(n), "n must be non-negative.");

        if (min > max)
            throw new ArgumentException("min must be less than or equal to max.");

        var result = new List<int>(capacity: n);

        // Random.Shared is thread-safe and avoids repeated seeding issues.
        // NOTE: Random.Next(min, maxExclusive) is exclusive on the upper bound,
        // so we adjust to make [min, max] inclusive.
        if (max == int.MaxValue)
        {
            // Can't do (max + 1) without overflow; use NextInt64 instead.
            for (var i = 0; i < n; i++)
            {
                var value = (int)Random.Shared.NextInt64(min, (long)max + 1);
                result.Add(value);
            }

            return result;
        }

        var maxExclusive = max + 1;

        for (var i = 0; i < n; i++)
        {
            result.Add(Random.Shared.Next(min, maxExclusive));
        }

        return result;
    }
}