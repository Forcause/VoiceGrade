using System.Text.RegularExpressions;
using SimMetrics.Net.Metric;
using VoiceGradeApi.Models;

namespace VoiceGradeApi.Util;

public class Correlation
{
    public void CorrelateScores(List<Pupil> pupils, List<string> transcribedElements)
    {
        var getTextOnly = new Regex(@"[^А-Я\s]+");
        foreach (var pupil in pupils)
        {
            double minDistance = 0;
            var minPosition = 0;
            var name = pupil.Name.ToUpper().Replace('Ё', 'Е');
            var surname = pupil.Surname.ToUpper().Replace('Ё', 'Е');
            for (var j = 0; j < transcribedElements.Count; j++)
            {
                var currentTranscribedElement = transcribedElements[j].ToUpper().Replace('Ё', 'Е');
                currentTranscribedElement = getTextOnly.Replace(currentTranscribedElement, "").Trim();

                var currentNameDistance =
                    JaroWinklerDistance.GetDistance(name, currentTranscribedElement);

                var currentSurnameDistance =
                    JaroWinklerDistance.GetDistance(surname, currentTranscribedElement);

                var totalDistance =
                    JaroWinklerDistance.GetDistance(surname + " " + name, currentTranscribedElement);
                
                var minDistanceBetween = currentNameDistance > currentSurnameDistance
                    ? currentNameDistance > totalDistance ? currentNameDistance : currentSurnameDistance
                    : currentSurnameDistance > totalDistance ? currentSurnameDistance : totalDistance;
                if (minDistanceBetween > minDistance)
                {
                    minDistance = minDistanceBetween;
                    minPosition = j;
                }
            }

            pupil.Note = transcribedElements[minPosition][transcribedElements[minPosition].LastIndexOf(" ")..].Trim();
        }
    }
}