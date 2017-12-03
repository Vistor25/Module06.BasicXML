using System;
using System.IO;
using System.Xml.Serialization;
using XML.Models;

namespace XML
{
    public class XMLRepository : IXMLRepository
    {
        private string FilePath { get; set; }

        public XMLRepository(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException();
            }

            FilePath = filePath;
        }

        public FileStream Load()
        {
            FileStream fileStream;
            try
            {
                fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite);
            }
            catch (IOException)
            {
                throw new IOException($"{FilePath} couldn't be loaded");
            }

            return fileStream;
        }

        public void Save(Catalog catalog)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                using (var fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
                {
                    var serializer = new XmlSerializer(typeof(Catalog));
                    serializer.Serialize(fileStream, catalog);
                }
            }
            catch (IOException)
            {
                throw new IOException($"File wasn't saved!");
            }
        }
    }
}

