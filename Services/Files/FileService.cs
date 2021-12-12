using System.Xml;
using static System.IO.File;
using static System.IO.Path;
using static System.IO.Directory;
using Translation.Services.Translations;
using Translation.Model.Translations;
using Newtonsoft.Json;

namespace Translation.Services.Files;

public class FileService
{
    private string fileName;
    private string filePath;
    private ConnectToRemote _connectToRemote;
    XmlDocument xmlDocument;
    ICollection<ICollection<TranslationResponse>> translations = new HashSet<ICollection<TranslationResponse>>();
    public FileService()
    {
        filePath = Combine(GetCurrentDirectory(), "Data");
        fileName = Combine(filePath, "messages.xlf");
    }

    public FileService SetRemote(ConnectToRemote connectToRemote)
    {
        _connectToRemote = connectToRemote;
        return this;
    }

    public string Save(XmlDocument doc, string code)
    {
        var fileName = Combine(filePath, $"messages.{code}.xlf");
        doc.Save(fileName);
        return fileName;
    }

    public FileService LoadXMLFromFile()
    {
        XmlDocument doc = new XmlDocument();
        using (Stream fileStream = Open(fileName, FileMode.Open))
        {
            doc.Load(fileStream);
            xmlDocument = doc;
            var fileNode = doc?.DocumentElement?.ChildNodes;
            var bodyNode = fileNode?.Item(0)?.ChildNodes;
            var translationNode = bodyNode?.Item(0)?.ChildNodes;
            var i = 0;
            foreach (XmlNode item in translationNode)
            {
                var node = item.ChildNodes;
                var source = node?.Item(0)?.InnerText;
                var translation = this._connectToRemote.GetTranslation(source);
                Console.WriteLine(JsonConvert.SerializeObject(translation.Result));
                translations.Add(translation.Result);
            }
            var b = doc?.DocumentType;
        }
        return this;
    }

    public void Build()
    {
        foreach (var code in this._connectToRemote.ToLanguageCode)
        {
            XmlDocument doc = new XmlDocument();
            var name = Save(xmlDocument, code);
            using (Stream fileStream = Open(name, FileMode.Open))
            {
                doc.Load(fileStream);
                var fileNode = doc?.DocumentElement?.ChildNodes;
                var bodyNode = fileNode?.Item(0)?.ChildNodes;
                var translationNode = bodyNode?.Item(0)?.ChildNodes;
                Console.WriteLine(JsonConvert.SerializeObject(translations));
                var enumarator = translations.GetEnumerator();

                foreach (XmlNode nodeItem in translationNode)
                {
                    var node = nodeItem.ChildNodes;
                    enumarator.MoveNext();
                    node.Item(0).InnerText = enumarator.Current.FirstOrDefault(x => x.To == code).Text;
                }
                var b = doc?.DocumentType;
            }
            doc.Save(name);
        }
    }
}