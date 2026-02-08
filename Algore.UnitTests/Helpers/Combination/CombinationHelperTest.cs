using Algore.Helpers.Combination;
using Xunit;

namespace Algore.UnitTests.Helpers.Combination;

public class CombinationHelperTests
{
    [Fact]
    public void GenerateAllCombinations_EmptyList_ReturnsSingleEmptySubset()
    {
        var input = new List<int>();

        var result = CombinationHelper.GenerateAllCombinations(input);

        Assert.Single(result);
        Assert.Empty(result[0]);
    }

    [Fact]
    public void GenerateAllCombinations_ThreeDistinctElements_Returns2PowNSubsets()
    {
        var input = new List<int> { 10, 20, 30 };

        var result = CombinationHelper.GenerateAllCombinations(input);

        Assert.Equal(1 << input.Count, result.Count);
    }

    [Fact]
    public void GenerateAllCombinations_ContainsEmptySetAndFullSet()
    {
        var input = new List<string> { "a", "b", "c" };

        var result = CombinationHelper.GenerateAllCombinations(input);

        Assert.Contains(result, subset => subset.Count == 0);
        Assert.Contains(result, subset => subset.SequenceEqual(input));
    }

    [Fact]
    public void GenerateAllCombinations_DoesNotIntroduceElementsNotInInput()
    {
        var input = new List<int> { 1, 2, 3 };

        var result = CombinationHelper.GenerateAllCombinations(input);

        foreach (var subset in result)
        {
            Assert.All(subset, item => Assert.Contains(item, input));
        }
    }

    [Fact]
    public void GenerateAllCombinations_TwoElements_ProducesExpectedSubsets_IgnoringOrderOfSubsets()
    {
        var input = new List<int> { 1, 2 };

        var result = CombinationHelper.GenerateAllCombinations(input);

        var actual = ToCanonicalSubsetSet(result);
        var expected = ToCanonicalSubsetSet(new List<List<int>>
        {
            new(),        // []
            new() { 1 },  // [1]
            new() { 2 },  // [2]
            new() { 1, 2 } // [1,2]
        });

        Assert.True(expected.SetEquals(actual));
    }

    private static HashSet<string> ToCanonicalSubsetSet<T>(IEnumerable<List<T>> subsets)
    {
        return subsets
            .Select(subset => string.Join("|", subset.Select(x => x?.ToString() ?? "<null>")))
            .ToHashSet(StringComparer.Ordinal);
    }
}
