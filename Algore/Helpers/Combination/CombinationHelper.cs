namespace Algore.Helpers.Combination;

public static class CombinationHelper
{
    public static List<List<T>> GenerateAllCombinations<T>(List<T> list)
    {
        var n = list.Count;
        var allCombinations = new List<List<T>>();
        
        for (var i = 0; i < (1 << n); i++)
        {
            var subset = new List<T>();
            for (var j = 0; j < n; j++)
            {
                if ((i & (1 << j)) != 0)
                {
                    subset.Add(list[j]);
                }
            }
            
            allCombinations.Add(subset);
        }

        return allCombinations;
    }
}