using Newtonsoft.Json;
using VoiceGradeApi.Models;

namespace VoiceGradeApi.Services.FileService;

public sealed class JsonService : IFileService
{
    private readonly string _generatedFilesDirectory = Directory.GetCurrentDirectory() + @"\GeneratedFiles"; 
    public string CreateFile(List<Pupil> pupils)
    {
        if (!Directory.Exists(_generatedFilesDirectory)) Directory.CreateDirectory(_generatedFilesDirectory);
        string jsonFilePath = _generatedFilesDirectory + @$"\{Guid.NewGuid()}.json";
        TextWriter writer = null;
        try
        {
            string json = JsonConvert.SerializeObject(pupils, Formatting.Indented);
            writer = new StreamWriter(jsonFilePath);
            writer.Write(json);
            return jsonFilePath;
        }
        finally
        {
            if (writer is not null) writer.Close();
        }
    }

    public List<Pupil> ReadFile(string path)
    {
        TextReader reader = null;
        try
        {
            reader = new StreamReader(path);
            var readed = reader.ReadToEnd();
            var jsonPersons = JsonConvert.DeserializeObject<List<Pupil>>(readed);

            return jsonPersons;
        }
        finally
        {
            if (reader is not null) reader.Close();
        }
    }
}