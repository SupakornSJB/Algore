namespace Algore.Interfaces;

public interface ISortImplementation
{
    static abstract void Sort<T>(
        List<T> list, 
        bool asc = true, 
        IComparer<T>? comparer = null) where T : IComparable<T>;
}