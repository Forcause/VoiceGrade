namespace VoiceGradeApi.Models;

public class Pupil
{
    public string Surname { get; }
    
    public string Name { get; }

    public string? MiddleName { get; }

    public string? Note { get; set; }

    public Pupil(string name, string surname, string? middleName)
    {
        Name = name;
        Surname = surname;
        MiddleName = middleName;
    }
}