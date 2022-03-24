namespace VoiceGradeApi.Models;

public class Person
{
    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Patronymic { get; set; }

    public int Score { get; set; }

    public Person(string? name, string? surname, string? patronymic)
    {
        Name = name;
        Surname = surname;
        Patronymic = patronymic;
    }
}