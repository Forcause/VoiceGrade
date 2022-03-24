namespace VoiceGradeApi.Services.FileReaderServices;

public class TextReader : IFileReader
{
    public string Read(string path)
    {
            System.IO.TextReader reader = null;
            try
            {
                reader = new StreamReader(path);
                var readed = reader.ReadToEnd();
                return readed;
            }
            finally
            {
                if(reader is not null) reader.Close();
            }
    }
}