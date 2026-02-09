using Algore.Assignment_1.IntervalScheduling;
using Algore.Assignment_1.Models;
using Algore.Helpers.Normalizers;

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