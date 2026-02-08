namespace Algore.Helpers;

public static class TimeHelper
{
    public static string LogTime() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
    public static string FileNameTime() => DateTime.Now.ToString("yyyyMMddHHmmss");
}