using System.Text;

namespace VoiceGradeApi.Util;

public static class JaroWinklerDistance
{
    private const double DefaultMatches = 0.0;

    public static double GetDistance(string? firstWord, string? secondWord)
    {
        if (firstWord == null || secondWord == null) return DefaultMatches;
        var jaroDistance = JaroDistance(firstWord, secondWord);
        var prefixLength = PrefixLength(firstWord, secondWord);
        return jaroDistance + prefixLength * 0.1 * (1.0 - jaroDistance);
    }

    private static int PrefixLength(string? firstWord, string? secondWord)
    {
        const int defaultPrefixLength = 4;
        if (firstWord == null || secondWord == null) return defaultPrefixLength;

        var num = Math.Min(defaultPrefixLength, Math.Min(firstWord.Length, secondWord.Length));
        for (var i = 0; i < num; i++)
        {
            if (firstWord[i] != secondWord[i])
            {
                return i;
            }
        }

        return num;
    }

    private static double JaroDistance(string? firstWord, string? secondWord)
    {
        if (firstWord == null || secondWord == null)
        {
            return DefaultMatches;
        }

        var halfRoundedLength = Math.Min(firstWord.Length, secondWord.Length) / 2 + 1;
        var firstToSecondWords = GetCommonCharacters(firstWord, secondWord, halfRoundedLength);
        var matches = firstToSecondWords!.Length;
        if (matches == 0)
        {
            return DefaultMatches;
        }

        var secondToFirstWords = GetCommonCharacters(secondWord, firstWord, halfRoundedLength);
        if (matches != secondToFirstWords!.Length)
        {
            return DefaultMatches;
        }

        var transpositionCounter = 0;
        for (var i = 0; i < matches; i++)
        {
            if (firstToSecondWords[i] != secondToFirstWords[i])
            {
                transpositionCounter++;
            }
        }

        transpositionCounter /= 2;
        return matches / (3.0 * firstWord.Length) + matches / (3.0 * secondWord.Length) +
               (matches - transpositionCounter) / (3.0 * matches);
    }

    private static StringBuilder? GetCommonCharacters(string? firstWord, string? secondWord, int halfRoundedLength)
    {
        if (firstWord == null || secondWord == null)
        {
            return null;
        }

        var commonCharacters = new StringBuilder();
        var comparingWord = new StringBuilder(secondWord);
        for (var i = 0; i < firstWord.Length; i++)
        {
            var currentChar = firstWord[i];
            for (var j = Math.Max(0, i - halfRoundedLength);
                 j < Math.Min(i + halfRoundedLength, secondWord.Length);
                 j++)
            {
                if (comparingWord[j] != currentChar) continue;
                commonCharacters.Append(currentChar);
                comparingWord[j] = '*';
                break;
            }
        }

        return commonCharacters;
    }
}