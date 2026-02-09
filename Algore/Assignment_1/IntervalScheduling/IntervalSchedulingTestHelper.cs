using System.Diagnostics;
using System.Text.Json;
using Algore.Assignment_1.Models;
using Algore.Helpers;
using Algore.Helpers.Stopwatch;
using ScottPlot;
using MathNet.Numerics.Statistics;

namespace Algore.Assignment_1.IntervalScheduling;

public static class IntervalSchedulingTestHelper
{
    private static int JobAmount = 0;
    private static int CurrentJobCount = 1;
    
    public static void RunTests(
        IntervalSchedulingTestConfiguration[] configs, 
        string saveFilePath = "",
        decimal intervalDuration = 10m, 
        int trialsPerConfig = 10)
    {
        Console.WriteLine("Starting Tests...");
        Console.WriteLine($"Save Path: ${saveFilePath}");
        
        var sw = new Stopwatch();
        JobAmount = CalculateJobAmount(configs, trialsPerConfig);
        
        Console.WriteLine("******************************************************************************************************");
        foreach (var config in configs)
        {
            foreach (var generator in config.Generators)
            {
                GenerateOneChart(
                    config.N,
                    config.Alpha,
                    generator,
                    config.IntervalDuration ?? intervalDuration,
                    config.TrialsPerConfig ?? trialsPerConfig,
                    sw,
                    saveFilePath,
                    config.Normalizers);
            }
        }

        CurrentJobCount = 1;
    }
    
    private static void GenerateOneChart(
        int[] ns, 
        decimal[] alphas, 
        Func<List<Interval>, object> generator,
        decimal intervalDuration,
        int trialPerConfig,
        Stopwatch sw,
        string saveFilePath,
        List<Func<int, double, (double, double)>> normalizers)
    {
        Console.WriteLine($"Start Generating ({CurrentJobCount}/{JobAmount})- Method: {generator.Method.Name}, N: {ns.Length}, Alpha: {alphas.Length}.");
        var correctAlphaPlots = new List<List<double>>();
        var correctNPlots = new List<int>();

        foreach (var n in ns)
        {
            foreach (var _ in Enumerable.Range(1, trialPerConfig))
            {
                correctNPlots.Add(n);  
            }
        }

        foreach (var alpha in alphas)
        {
            var resultTimeForThisAlpha = new List<double>();
            foreach (var n in correctNPlots)
            {
                Console.WriteLine($"Generating Test Set STARTED ({CurrentJobCount}/{JobAmount}) - Method: {generator.Method.Name}, Time: {TimeHelper.LogTime()}");
                var testSet = IntervalSchedulingHelper.GenerateTestSet(n, alpha, intervalDuration);
                Console.WriteLine($"Generating Test Set FINISHED ({CurrentJobCount}/{JobAmount}) - Method: {generator.Method.Name}, Time: {TimeHelper.LogTime()}");
                    
                Console.WriteLine($"Running Algorithm STARTED ({CurrentJobCount}/{JobAmount}) - Method: {generator.Method.Name}, Time: {TimeHelper.LogTime()}");
                sw.Start();
                generator(testSet);
                sw.Stop();
                Console.WriteLine($"Running Algorithm FINISHED ({CurrentJobCount}/{JobAmount}) - Method: {generator.Method.Name}, Time: {TimeHelper.LogTime()}");
                    
                resultTimeForThisAlpha.Add(StopwatchHelper.GetPreciseElapsedTime(sw));
                sw.Reset();
                CurrentJobCount++;
            }
            correctAlphaPlots.Add(resultTimeForThisAlpha.Skip(1).ToList());
        }

        var plt = new Plot();
        var normalizedPlotMap = new Dictionary<string, Plot>();
        
        plt.XLabel("t(n)");
        plt.YLabel("n");
        plt.Title($"Method: {generator.Method.Name}");
        plt.Legend.Alignment = Alignment.UpperRight;
        plt.ShowLegend();
        
        for (var alphaIndex = 0; alphaIndex < correctAlphaPlots.Count; alphaIndex++)
        {
            var scatter = plt.Add.ScatterPoints(correctNPlots, correctAlphaPlots[alphaIndex]);
            scatter.LegendText = $"Alpha: {alphas[alphaIndex]}";
            foreach (var normalizer in normalizers)
            {
                if (!normalizedPlotMap.TryGetValue(normalizer.Method.Name, out var normalPlot))
                {
                    normalPlot = new Plot();
                    normalizedPlotMap.TryAdd(normalizer.Method.Name, normalPlot);
                }
                
                normalPlot.XLabel("t(n)");
                normalPlot.YLabel("normalized n");
                normalPlot.Title($"Method: {generator.Method.Name}, normalized: {normalizer.Method.Name}");
                normalPlot.Legend.Alignment = Alignment.UpperRight;
                normalPlot.ShowLegend();
                
                var normalizedPlot = correctNPlots.Zip(correctAlphaPlots[alphaIndex]).Select((x) => normalizer(x.First, x.Second)).ToList();
                var normalScatter = normalPlot.Add.ScatterPoints(
                    normalizedPlot.Select(x => x.Item1).ToList(), 
                    normalizedPlot.Select(x => x.Item2).ToList());
                
                normalScatter.LegendText = $"Alpha: {alphas[alphaIndex]}";
                normalizedPlotMap.TryAdd(normalizer.Method.Name, normalPlot);
            }
        }

        var chartFileName = $"{saveFilePath}/{generator.Method.Name}_{TimeHelper.FileNameTime()}.png";
        plt.SavePng(chartFileName, 1280, 720);
        
        foreach (var kvp in normalizedPlotMap)
        {
            var normalChartFileName = $"{saveFilePath}/{generator.Method.Name}_{TimeHelper.FileNameTime()}_{kvp.Key}.png";
            kvp.Value.SavePng(normalChartFileName, 1280, 720);     
        }
        
        var meanAndDeviationDict = new Dictionary<string, double>();
        CalculateMeanAndDeviation(meanAndDeviationDict, correctAlphaPlots, trialPerConfig, ns, alphas, generator.Method.Name);
        GenerateMeanAndDeviationJsonFile(meanAndDeviationDict, saveFilePath, generator.Method.Name);
        
        Console.WriteLine($"Finished Generating - Method: {generator.Method.Name}, N: {ns.Length}, Alpha: {alphas.Length}, File: {chartFileName}.");
        Console.WriteLine("******************************************************************************************************");
    }

    private static void GenerateMeanAndDeviationJsonFile(Dictionary<string, double> dict, string saveFilePath, string methodName)
    {
        Console.WriteLine($"Saving Mean and Deviation STARTED ({CurrentJobCount}/{JobAmount}) - Method: {methodName}, Time: {TimeHelper.LogTime()}");
        var jsonFileName = $"{saveFilePath}/{methodName}_{TimeHelper.FileNameTime()}.json";
        var options = new JsonSerializerOptions
        {
            WriteIndented = true 
        };

        var json = JsonSerializer.Serialize(dict, options);
        File.WriteAllText(jsonFileName, json);
        Console.WriteLine($"Saving Mean and Deviation FINISHED ({CurrentJobCount}/{JobAmount}) - Method: {methodName}, Time: {TimeHelper.LogTime()}");
    }

    private static int CalculateJobAmount(IntervalSchedulingTestConfiguration[] configs, int trialsPerConfig)
    {
        var jobAmount = 0;
        foreach (var config in configs)
        {
            jobAmount += config.Generators.Count * config.Alpha.Length * config.N.Length * (config.TrialsPerConfig ?? trialsPerConfig);
        }
        return jobAmount;
    }

    private static void CalculateMeanAndDeviation(
        Dictionary<string, double> resultDict, 
        List<List<double>> correctAlphaPlots,
        int trialsPerConfig,
        int[] ns,
        decimal[] alphas,
        string methodName)
    {
        Console.WriteLine($"Calculating Mean and Deviation STARTED ({CurrentJobCount}/{JobAmount}) - Method: {methodName}, Time: {TimeHelper.LogTime()}");
        for (var i = 0; i < alphas.Length; i++)
        {
            var mean = correctAlphaPlots[i].Select(x => x).AsEnumerable().Mean();
            var deviation = correctAlphaPlots[i].Select(x => x).AsEnumerable().StandardDeviation();
            
            resultDict.TryAdd($"Mean By Alpha: {alphas[i]}", mean);
            resultDict.TryAdd($"SD By Alpha: {alphas[i]}", deviation);
            
            correctAlphaPlots[i] = RemoveOutlierByAlpha(correctAlphaPlots[i], mean, deviation);
        }

        for (var i = 0; i < ns.Length; i++)
        {
            var startIndex = i * trialsPerConfig;
            var endIndex = startIndex + trialsPerConfig;

            var allValueOfThisN = new List<double>();
            for (var j = 0; j < alphas.Length; j++)
            {
                allValueOfThisN.AddRange(correctAlphaPlots[j][startIndex..(endIndex-1)].Select(x => x));
            }
            
            resultDict.TryAdd($"Mean By N: {ns[i]}", allValueOfThisN.Mean());
            resultDict.TryAdd($"SD By N: {ns[i]}", allValueOfThisN.StandardDeviation());
        }
        Console.WriteLine($"Calculating Mean and Deviation FINISHED ({CurrentJobCount}/{JobAmount}) - Method: {methodName}, Time: {TimeHelper.LogTime()}");
    }

    private static List<double> RemoveOutlierByAlpha(List<double> data, double mean, double std)
    {
        var threshold = 2d;
        return data.Select(x => Math.Abs((x - mean) / std) >= threshold ? mean : x).ToList();
    }
}