namespace VoiceGradeApi.Util;

public static class DamerauLevenshtein
{
    public static int GetDistance(string original, string modified)
    {
        if (original == modified)
            return 0;

        int lenOrig = original.Length;
        int lenDiff = modified.Length;
        if (lenOrig == 0 || lenDiff == 0)
            return lenOrig == 0 ? lenDiff : lenOrig;

        var matrix = new int[lenOrig + 1, lenDiff + 1];

        for (int i = 1; i <= lenOrig; i++)
        {
            matrix[i, 0] = i;
            for (int j = 1; j <= lenDiff; j++)
            {
                int cost = modified[j - 1] == original[i - 1] ? 0 : 1;
                if (i == 1)
                    matrix[0, j] = j;

                var vals = new int[]
                {
                    matrix[i - 1, j] + 1,
                    matrix[i, j - 1] + 1,
                    matrix[i - 1, j - 1] + cost
                };
                matrix[i, j] = vals.Min();
                if (i > 1 && j > 1 && original[i - 1] == modified[j - 2] && original[i - 2] == modified[j - 1])
                    matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
            }
        }

        return matrix[lenOrig, lenDiff];
    }
}