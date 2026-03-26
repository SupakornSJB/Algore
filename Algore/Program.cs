using Algore.Algorithms.Sort;
using Algore.Assignment_1.IntervalScheduling;
using Algore.Assignment_1.Models;
using Algore.DataStructures.Tree;
using Algore.DataStructures.Tree.Implementations;
using Algore.Helpers.Normalizers;
using Algore.Helpers.TestSet;

// Uncomment to test
// TestInvervalScheduling();
// TestQuickSort();
// TestHeapSort();
// TestMergeSort();
TestBST();
return;

void TestIntervalScheduling()
{
    int Pow(int x, int y) => (int)Math.Pow(x, y);
    var currentDirectory = Directory.GetCurrentDirectory();

    var configs = new List<IntervalSchedulingTestConfiguration>
    {
        new() 
        {
            N = Enumerable.Range(10, 14).Select(x => Pow(2,x)).ToArray(),
            Alpha = [0.1m, 1, 5],
            Generators =
            [
                IntervalScheduling.EarliestFinishTime,
                IntervalScheduling.EarliestStartTime,
                IntervalScheduling.ShortestDuration,
            ],
            Normalizers = [
                NormalizerHelpers.LogLogNormalizer,
                NormalizerHelpers.NLogNNormalizer
            ]
        },
        new()
        {
            N = [5,10,15,20],
            Alpha = [0.1m, 1, 5],
            Generators =
            [
                IntervalScheduling.Exhaustive
            ],
            Normalizers = [
                NormalizerHelpers.N2Normalizer
            ]
        }
    }.ToArray();

    IntervalSchedulingTestHelper.RunTests(configs, currentDirectory);
}

void TestHeapSort()
{
    var randomNumbers = IntegerTestSetGeneratorHelper.GenerateRandomNumbers(100, 0, 100);
    Console.WriteLine("Heap sort (Before): " + string.Join(",", randomNumbers));
    HeapSortImplementation.Sort(randomNumbers);
    Console.WriteLine("Heap sort (After): " + string.Join(",", randomNumbers));
}

void TestQuickSort()
{
    var randomNumbers = IntegerTestSetGeneratorHelper.GenerateRandomNumbers(100, 0, 100);
    Console.WriteLine("Quick sort (Before): " + string.Join(",", randomNumbers));
    QuickSortImplementation.Sort(randomNumbers);
    Console.WriteLine("Quick sort (After): " + string.Join(",", randomNumbers));
}

void TestMergeSort()
{
    var randomNumbers = IntegerTestSetGeneratorHelper.GenerateRandomNumbers(100, 0, 100);
    Console.WriteLine("Merge sort (Before): " + string.Join(",", randomNumbers));
    MergeSortImplementation.Sort(randomNumbers);
    Console.WriteLine("Merge sort (After): " + string.Join(",", randomNumbers));
}

void TestBST()
{
    var bst = new BinarySearchTree<int, int>((i => i));
    var numList = new List<int> {2, 5, 3, 9, 10, 1, 8, 7, 4, 6};
    foreach (var num in numList)
    {
        Console.WriteLine("Inserting: " + num);
        bst.Insert(num);
    }

    foreach (var num in numList)
    {
        var result = bst.Search(num);
        Console.WriteLine("Searching for: " + num + " - Result: " + result);
    }
}