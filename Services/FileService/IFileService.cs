using VoiceGradeApi.Models;

namespace VoiceGradeApi.Services.FileService;

public interface IFileService
{
    string CreateFile(List<Pupil> pupils);
    List<Pupil> ReadFile(string path);
}