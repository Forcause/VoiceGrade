using System.Text.RegularExpressions;
using VoiceGradeApi.Models;

namespace VoiceGradeApi.Util;

public class Correlator
{
    public void CorrelateScores(List<Pupil> pupils, List<string> transcribedNames)
    {
        Regex getGrade = new Regex(@"[^\d]");
        Regex getTextOnly = new Regex(@"[^А-Я\s]+");
        for (int i = 0; i < pupils.Count; i++)
        {
            int minDistance = Int32.MaxValue;
            int minPosition = 0;
            string name = pupils[i].Surname + " " + pupils[i].Name;
            for (int j = 0; j < transcribedNames.Count; j++)
            {
                string currentName = transcribedNames[j].ToUpper().Replace('Ё', 'Е');
                int currentDistance = DamerauLevenshtein.GetDistance(getTextOnly.Replace(currentName, " ").Trim(),
                    name.ToUpper().Replace('Ё', 'Е'));
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    minPosition = j;
                }
            }
            
            pupils[i].Score = int.Parse(getGrade.Replace(transcribedNames[minPosition], ""));
            //transcribedNames.RemoveAt(minPosition);
        }
    }
}