using System.Text.RegularExpressions;
using VoiceGradeApi.Models;

namespace VoiceGradeApi.Util;

public class Correlator
{
    public void CorrelateScores(List<Pupil> pupils, List<string> transcribedNames)
    {
        Regex getGrade = new Regex(@"[^\d]");
        for (int i = 0; i < pupils.Count; i++)
        {
            int min = Int32.MaxValue;
            int minPosition = 0;
            for (int j = 0; j < transcribedNames.Count; j++)
            {
                int current = DamerauLevenshtein.GetDistance(transcribedNames[j], pupils[i].FullName);
                if (current < min)
                {
                    min = current;
                    minPosition = j;
                }
            }

            pupils[i].Score = int.Parse(getGrade.Replace(transcribedNames[minPosition], ""));
        }
    }
}