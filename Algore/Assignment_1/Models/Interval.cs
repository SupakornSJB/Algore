namespace Algore.Assignment_1.Models;

public class Interval: IComparable
{
    public int Id { get; set; }
    public decimal Start { get; set; }
    public decimal End { get; set; }
    
    public int CompareTo(object? obj)
    {
        throw new NotImplementedException();
    }
}