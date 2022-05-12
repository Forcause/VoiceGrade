using System.Xml.Linq;
using VoiceGradeApi.Models;

namespace VoiceGradeApi.Services.FileService;

public sealed class XmlService : IFileService
{
    private readonly string _generatedFilesDirectory = Directory.GetCurrentDirectory() + @"\GeneratedFiles";

    public string CreateFile(List<Pupil> pupils)
    {
        if (!Directory.Exists(_generatedFilesDirectory)) Directory.CreateDirectory(_generatedFilesDirectory);
        var xmlFilePath = _generatedFilesDirectory + @$"\{Guid.NewGuid()}.xml";
        TextWriter writer = null;
        try
        {
            var xmlText = new XElement("PupilsList",
                from pupil in pupils
                select new XElement("Pupil",
                    new XElement("Name", pupil.Name),
                    new XElement("Surname", pupil.Surname),
                    new XElement("Patronymic", pupil.Patronymic),
                    new XElement("Note", pupil.Note))
            );
            var stringXml = xmlText.ToString();
            writer = new StreamWriter(xmlFilePath);
            writer.Write(stringXml);
            return xmlFilePath;
        }
        finally
        {
            writer?.Close();
        }
    }

    public List<Pupil> ReadFile(string path)
    {
        TextReader reader = null;
        try
        {
            reader = new StreamReader(path);
            var readXml = reader.ReadToEnd();
            var xmlElements = XElement.Parse(readXml);
            var parsedElements = (from el in xmlElements.Elements("pupil")
                select new Pupil(el.Attribute("Name").Value, el.Attribute("Surname").Value,
                    el.Attribute("Patronymic").Value)).ToList();
            return parsedElements;
        }
        finally
        {
            reader?.Close();
        }
    }
}