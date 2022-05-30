using System.Text.RegularExpressions;
using VoiceGradeApi.Models;

namespace VoiceGradeApi.Util;

public class Correlation
{
    public void CorrelateScores(List<Pupil> pupils, List<string> transcribedElements)
    {
        var getTextOnly = new Regex(@"[^А-Я\s]+");
        for (var i = 0; i < transcribedElements.Count; i++)
        {
            double minDistance = 0;
            var minPosition = 0;
            foreach (var pupil in pupils)
            {
                var name = pupil.Name.ToUpper().Replace('Ё', 'Е');
                var surname = pupil.Surname.ToUpper().Replace('Ё', 'Е');
                var currentTranscribedElement = transcribedElements[i].ToUpper().Replace('Ё', 'Е');
                currentTranscribedElement = getTextOnly.Replace(currentTranscribedElement, "").Trim();

                var currentNameDistance =
                    JaroWinklerDistance.GetDistance(name, currentTranscribedElement);

                var currentSurnameDistance =
                    JaroWinklerDistance.GetDistance(surname, currentTranscribedElement);

                var totalDistance =
                    JaroWinklerDistance.GetDistance(surname + " " + name, currentTranscribedElement);

                var minDistanceBetween = currentNameDistance > currentSurnameDistance
                    ? currentNameDistance > totalDistance ? currentNameDistance : currentSurnameDistance
                    : currentSurnameDistance > totalDistance
                        ? currentSurnameDistance
                        : totalDistance;
                if (minDistanceBetween > minDistance)
                {
                    minDistance = minDistanceBetween;
                    minPosition = i;
                }
            }

            pupils[minPosition].Note = transcribedElements[i][transcribedElements[i].LastIndexOf(" ")..].Trim();
        }
    }
}