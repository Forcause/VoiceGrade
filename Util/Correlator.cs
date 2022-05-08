using System.Text.RegularExpressions;
using VoiceGradeApi.Models;

namespace VoiceGradeApi.Util;

public class Correlator
{
    public void CorrelateScores(List<Pupil> pupils, List<string> transcribedElements)
    {
        //Regex getGrade = new Regex(@"[^\d]");
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
                int totalDistance =
                    DamerauLevenshtein.GetDistance(surname + " " + name, currentTranscribedElement);
                int minDistanceBetween = currentNameDistance < currentSurnameDistance
                    ? currentNameDistance < totalDistance ? currentNameDistance : totalDistance
                    : currentSurnameDistance < totalDistance ? currentSurnameDistance : totalDistance;
                if (minDistanceBetween < minDistance)
                {
                    minDistance = minDistanceBetween;
                    minPosition = j;
                }
            }

            pupil.Note = transcribedElements[minPosition].Substring(transcribedElements[minPosition].LastIndexOf(" ")).Trim();
            transcribedElements.RemoveAt(minPosition);
        }
    }
}