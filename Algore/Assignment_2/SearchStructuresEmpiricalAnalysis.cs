using System.Diagnostics;
using Algore.DataStructures.HashTable;
using Algore.DataStructures.Tree.Implementations;

namespace Algore.Assignment_2;

public static class SearchStructuresEmpiricalAnalysis
{
    private static readonly int[] Sizes = [1_00, 1_000, 10_000];
    private static readonly InputPattern[] Patterns =
        [InputPattern.Random, InputPattern.Sorted, InputPattern.Adversarial];
    private const double MaxCaseSeconds = 2.0;

    public static void Run()
    {
        var rows = new List<ResultRow>();

        foreach (var pattern in Patterns)
        {
            foreach (var size in Sizes)
            {
                var dataset = GenerateDataset(size, pattern);
                var queries = GenerateSearchQueries(dataset, size);

                rows.Add(BenchmarkBst(dataset, queries, pattern));
                rows.Add(BenchmarkRedBlack(dataset, queries, pattern));
                rows.Add(BenchmarkHashTable(dataset, queries, pattern));
            }
        }

        PrintResultsTable(rows);
        PrintComplexitySummary(rows);
    }

    private static ResultRow BenchmarkBst(List<int> dataset, List<int> queries, InputPattern pattern)
    {
        var bst = new BinarySearchTree<int, int>(x => x);
        var timedOut = false;
        var insertedCount = 0;

        var insertTimeMs = MeasureMs(() =>
        {
            var guard = Stopwatch.StartNew();
            foreach (var x in dataset)
            {
                bst.Insert(x);
                insertedCount++;

                if (guard.Elapsed.TotalSeconds > MaxCaseSeconds)
                {
                    timedOut = true;
                    break;
                }
            }
        });

        var searchTimeMs = MeasureMs(() =>
        {
            var guard = Stopwatch.StartNew();
            foreach (var x in queries.Take(insertedCount))
            {
                _ = bst.Search(x);
                if (guard.Elapsed.TotalSeconds > MaxCaseSeconds)
                {
                    timedOut = true;
                    break;
                }
            }
        });

        return new ResultRow(
            "BinarySearchTree",
            pattern,
            dataset.Count,
            insertTimeMs,
            searchTimeMs,
            bst.Height,
            null,
            null,
            timedOut
                ? $"TIMEOUT>{MaxCaseSeconds:0}s (inserted {insertedCount:N0}/{dataset.Count:N0})"
                : "OK");
    }

    private static ResultRow BenchmarkRedBlack(List<int> dataset, List<int> queries, InputPattern pattern)
    {
        var rbt = new RedBlackTree<int, int>(x => x);

        var insertTimeMs = MeasureMs(() =>
        {
            foreach (var x in dataset)
            {
                rbt.Insert(x);
            }
        });

        var searchTimeMs = MeasureMs(() =>
        {
            foreach (var x in queries)
            {
                _ = rbt.Search(x);
            }
        });

        return new ResultRow(
            "RedBlackTree",
            pattern,
            dataset.Count,
            insertTimeMs,
            searchTimeMs,
            rbt.Height,
            null,
            null,
            "OK");
    }

    private static ResultRow BenchmarkHashTable(List<int> dataset, List<int> queries, InputPattern pattern)
    {
        const int baseCapacity = 1024;
        var table = new HashTableLinearProbing<int, int>(capacity: baseCapacity, predicate: x => x);

        var insertTimeMs = MeasureMs(() =>
        {
            foreach (var x in dataset)
            {
                table.Insert(x);
            }
        });

        var searchTimeMs = MeasureMs(() =>
        {
            foreach (var x in queries)
            {
                _ = table.Search(x);
            }
        });

        return new ResultRow(
            "HashTableLinearProbing",
            pattern,
            dataset.Count,
            insertTimeMs,
            searchTimeMs,
            null,
            table.GetTotalCollisions(),
            table.GetAverageProbeLength(),
            "OK");
    }

    private static double MeasureMs(Action action)
    {
        var sw = Stopwatch.StartNew();
        action();
        sw.Stop();
        return sw.Elapsed.TotalMilliseconds;
    }

    private static List<int> GenerateDataset(int n, InputPattern pattern)
    {
        return pattern switch
        {
            InputPattern.Random => GenerateUniqueRandomDataset(n, n * 20, seed: 42),
            InputPattern.Sorted => Enumerable.Range(1, n).ToList(),
            InputPattern.Adversarial => Enumerable.Range(1, n).Select(i => i * 1024).Reverse().ToList(),
            _ => throw new ArgumentOutOfRangeException(nameof(pattern), pattern, null)
        };
    }

    private static List<int> GenerateSearchQueries(List<int> inserted, int n)
    {
        n = Math.Min(n, 10_000);
        var random = new Random(12345);
        var queries = new List<int>(n);

        var existingCount = n / 2;
        for (var i = 0; i < existingCount; i++)
        {
            queries.Add(inserted[random.Next(inserted.Count)]);
        }

        var max = inserted.Max();
        for (var i = existingCount; i < n; i++)
        {
            queries.Add(max + 1 + i);
        }

        return Shuffle(queries, seed: 99);
    }

    private static List<int> Shuffle(List<int> values, int seed)
    {
        var random = new Random(seed);

        for (var i = values.Count - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (values[i], values[j]) = (values[j], values[i]);
        }

        return values;
    }

    private static List<int> GenerateUniqueRandomDataset(int n, int maxInclusive, int seed)
    {
        var random = new Random(seed);
        var set = new HashSet<int>();

        while (set.Count < n)
        {
            set.Add(random.Next(1, maxInclusive + 1));
        }

        return set.ToList();
    }

    private static void PrintResultsTable(List<ResultRow> rows)
    {
        Console.WriteLine();
        Console.WriteLine("Empirical Analysis of Search Structures");
        Console.WriteLine(new string('=', 145));
        Console.WriteLine(
            $"{Pad("Pattern", 12)}|{Pad("N", 10)}|{Pad("Structure", 24)}|{Pad("Insert (ms)", 14)}|{Pad("Search (ms)", 14)}|{Pad("Height", 10)}|{Pad("Collisions", 14)}|{Pad("AvgProbe", 12)}|{Pad("Status", 26)}");
        Console.WriteLine(new string('-', 145));

        foreach (var row in rows)
        {
            Console.WriteLine(
                $"{Pad(row.Pattern.ToString(), 12)}|{Pad(row.N.ToString(), 10)}|{Pad(row.Structure, 24)}|{Pad(row.InsertMs.ToString("F3"), 14)}|{Pad(row.SearchMs.ToString("F3"), 14)}|{Pad(row.Height?.ToString() ?? "-", 10)}|{Pad(row.Collisions?.ToString("F0") ?? "-", 14)}|{Pad(row.AverageChainLength?.ToString("F3") ?? "-", 12)}|{Pad(row.Status, 26)}");
        }

        Console.WriteLine(new string('=', 145));
        Console.WriteLine();
    }

    private static void PrintComplexitySummary(List<ResultRow> rows)
    {
        var bstRandom = rows.Where(r => r.Structure == "BinarySearchTree" && r.Pattern == InputPattern.Random)
            .OrderBy(r => r.N)
            .Last();
        var bstSorted = rows.Where(r => r.Structure == "BinarySearchTree" && r.Pattern == InputPattern.Sorted)
            .OrderBy(r => r.N)
            .Last();
        var rbtSorted = rows.Where(r => r.Structure == "RedBlackTree" && r.Pattern == InputPattern.Sorted)
            .OrderBy(r => r.N)
            .Last();
        var hashAdversarial = rows.Where(r => r.Structure == "HashTableLinearProbing" &&
                                              r.Pattern == InputPattern.Adversarial)
            .OrderBy(r => r.N)
            .Last();

        Console.WriteLine("Complexity Discussion");
        Console.WriteLine(new string('=', 145));
        Console.WriteLine(
            $"BST (random vs sorted, n={bstSorted.N}): height {bstRandom.Height} -> {bstSorted.Height}, status={bstSorted.Status}. This shows degeneration under ordered data, matching O(n) worst-case behavior.");
        Console.WriteLine(
            $"Red-Black Tree (sorted, n={rbtSorted.N}): observed height {rbtSorted.Height}, which stays near logarithmic growth and matches O(log n) insert/search.");
        Console.WriteLine(
            $"Hash Table (adversarial, n={hashAdversarial.N}): collisions={hashAdversarial.Collisions:F0}, avg-probe={hashAdversarial.AverageChainLength:F3}. Collision-heavy input increases constants, while expected behavior remains near O(1) average.");
        Console.WriteLine(new string('=', 145));
        Console.WriteLine();
    }

    private static string Pad(string value, int width) => value.PadRight(width);

    private enum InputPattern
    {
        Random,
        Sorted,
        Adversarial
    }

    private sealed record ResultRow(
        string Structure,
        InputPattern Pattern,
        int N,
        double InsertMs,
        double SearchMs,
        int? Height,
        double? Collisions,
        double? AverageChainLength,
        string Status);
}
