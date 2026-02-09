namespace Algore.Helpers.Normalizers;

public class NormalizerHelpers
{
    public static (double, double) LogLogNormalizer(int n, double tn)
    {
        return (Math.Log2(n), Math.Log2(tn));
    }

    public static (double, double) NLogNNormalizer(int n, double tn)
    {
        return (n, tn / Math.Log2(tn));
    }

    public static (double, double) N2Normalizer(int n, double tn)
    {
        return (n, tn / (n * Math.Pow(2, n)));
    }
}
