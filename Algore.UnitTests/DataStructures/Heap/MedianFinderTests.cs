using Algore.DataStructures.Heap;
using Xunit;

namespace Algore.UnitTests.DataStructures.Heap;

public class MedianFinderTests
{
    [Fact]
    public void FindMedian_Empty_Throws()
    {
        var medianFinder = new MedianFinder();

        Assert.Throws<InvalidOperationException>(() => medianFinder.FindMedian());
    }

    [Fact]
    public void Insert_OddCount_ReturnsMiddle()
    {
        var medianFinder = new MedianFinder();
        medianFinder.Insert(5);
        medianFinder.Insert(2);
        medianFinder.Insert(9);

        Assert.Equal(5, medianFinder.FindMedian());
    }

    [Fact]
    public void Insert_EvenCount_ReturnsAverage()
    {
        var medianFinder = new MedianFinder();
        medianFinder.Insert(5);
        medianFinder.Insert(2);
        medianFinder.Insert(9);
        medianFinder.Insert(1);

        Assert.Equal(3.5, medianFinder.FindMedian(), precision: 10);
    }

    [Fact]
    public void Delete_ExistingValue_UpdatesMedian()
    {
        var medianFinder = new MedianFinder();
        foreach (var value in new[] { 1, 2, 3, 4, 5 })
        {
            medianFinder.Insert(value);
        }

        var deleted = medianFinder.Delete(3);

        Assert.True(deleted);
        Assert.Equal(3, medianFinder.FindMedian(), precision: 10);
    }

    [Fact]
    public void Delete_NonExistingValue_ReturnsFalse()
    {
        var medianFinder = new MedianFinder();
        medianFinder.Insert(10);
        medianFinder.Insert(20);

        var deleted = medianFinder.Delete(15);

        Assert.False(deleted);
        Assert.Equal(15, medianFinder.FindMedian());
    }

    [Fact]
    public void InsertAndDelete_Duplicates_WorkCorrectly()
    {
        var medianFinder = new MedianFinder();
        medianFinder.Insert(2);
        medianFinder.Insert(2);
        medianFinder.Insert(2);
        medianFinder.Insert(10);

        Assert.Equal(2, medianFinder.FindMedian());

        Assert.True(medianFinder.Delete(2));
        Assert.Equal(2, medianFinder.FindMedian());
        Assert.True(medianFinder.Delete(2));
        Assert.Equal(6, medianFinder.FindMedian());
    }

    [Fact]
    public void Sequence_OfOperations_MaintainsCorrectMedian()
    {
        var medianFinder = new MedianFinder();
        var values = new List<int>();

        foreach (var value in new[] { 7, 1, 5, 3, 9, 2, 8 })
        {
            medianFinder.Insert(value);
            values.Add(value);
            Assert.Equal(GetMedian(values), medianFinder.FindMedian(), precision: 10);
        }

        foreach (var value in new[] { 5, 1, 9, 7 })
        {
            Assert.True(medianFinder.Delete(value));
            values.Remove(value);
            Assert.Equal(GetMedian(values), medianFinder.FindMedian(), precision: 10);
        }
    }

    private static double GetMedian(List<int> values)
    {
        var sorted = values.OrderBy(x => x).ToArray();
        var n = sorted.Length;
        if (n % 2 == 1)
        {
            return sorted[n / 2];
        }

        return (sorted[n / 2 - 1] + sorted[n / 2]) / 2.0;
    }
}
