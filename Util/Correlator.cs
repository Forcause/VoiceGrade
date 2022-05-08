using System.Text.RegularExpressions;
using VoiceGradeApi.Models;

namespace VoiceGradeApi.Util;

public class Correlator
{
    public void CorrelateScores(List<Pupil> pupils, List<string> transcribedElements)
    {
        Regex getGrade = new Regex(@"[^\d]");
        Regex getTextOnly = new Regex(@"[^А-Я\s]+");
        foreach (var pupil in pupils)
        {
            int minDistance = Int32.MaxValue;
            int minPosition = 0;
            string name = pupil.Name.ToUpper().Replace('Ё', 'Е');
            string surname = pupil.Surname.ToUpper().Replace('Ё', 'Е');
            for (int j = 0; j < transcribedElements.Count; j++)
            {
                string currentTranscribedElement = transcribedElements[j].ToUpper().Replace('Ё', 'Е');
                currentTranscribedElement = getTextOnly.Replace(currentTranscribedElement, "").Trim();
                int currentNameDistance =
                    DamerauLevenshtein.GetDistance(name, currentTranscribedElement);
                int currentSurnameDistance =
                    DamerauLevenshtein.GetDistance(surname, currentTranscribedElement);
                int minDistanceBetween = currentNameDistance < currentSurnameDistance
                    ? currentNameDistance
                    : currentSurnameDistance;
                if (minDistanceBetween < minDistance)
                {
                    minDistance = minDistanceBetween;
                    minPosition = j;
                }
            }

            pupil.Score = int.Parse(getGrade.Replace(transcribedElements[minPosition], ""));
            //transcribedElements.RemoveAt(minPosition);
        }
    }
}