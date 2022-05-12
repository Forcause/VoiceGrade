using Newtonsoft.Json;

namespace VoiceGradeApi.Models;

public class Pupil
{
    public string Surname { get; }
    
    public string Name { get; }

    public string? Patronymic { get; }

    [JsonIgnore]
    public string FullName => Surname + " " + Name + " " + Patronymic;  

    public string? Note { get; set; }

    public Pupil(string name, string surname, string? patronymic)
    {
        Name = name;
        Surname = surname;
        Patronymic = patronymic;
    }
}