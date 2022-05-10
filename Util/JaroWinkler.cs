using SimMetrics.Net.Metric;

namespace VoiceGradeApi.Util;

public static class JaroWinklerDistance
{
    public static double GetDistance(string original, string modified)
    {
        var jaroWinkler = new JaroWinkler();
        return jaroWinkler.GetSimilarity(original, modified);
    }
}