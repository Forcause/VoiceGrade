using Newtonsoft.Json;
using VoiceGradeApi.Models;

namespace VoiceGradeApi.Util;

public class JsonReader
{
    public List<Person> Read(string path)
    {
        TextReader reader = null;
        try
        {
            reader = new StreamReader(path);
            var readed = reader.ReadToEnd();
            var jsonPersons = JsonConvert.DeserializeObject<List<Person>>(readed);

            return jsonPersons;
        }
        finally
        {
            if (reader is not null) reader.Close();
        }
    }
}