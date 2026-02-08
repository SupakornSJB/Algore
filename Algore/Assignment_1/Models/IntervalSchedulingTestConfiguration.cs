namespace Algore.Assignment_1.Models;

public class IntervalSchedulingTestConfiguration
{
    public required int[] N { get; set; }
    public required decimal[] Alpha { get; set; } // - Different line
    public required List<Func<List<Interval>, object>> Generators { get; set; } // - Different chart
    public decimal? IntervalDuration { get; set; }    
    public int? TrialsPerConfig { get; set; }
}