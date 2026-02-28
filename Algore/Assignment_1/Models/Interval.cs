namespace Algore.Assignment_1.Models;

public class Interval: IComparable<Interval>
{
    public int Id { get; set; }
    public decimal Start { get; set; }
    public decimal End { get; set; }

    public int CompareTo(Interval? other)
    {
        throw new NotImplementedException();
    }
}