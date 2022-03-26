using Newtonsoft.Json;
using VoiceGradeApi.Models;

namespace VoiceGradeApi.Util;

public class JsonWriter
{
    private readonly string _generatedFilesDirectory = Directory.GetCurrentDirectory() + @"\GeneratedFiles"; 
    public string CreateFile(List<Person> pupils)
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
}