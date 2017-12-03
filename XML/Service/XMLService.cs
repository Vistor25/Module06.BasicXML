using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using XML.Models;

namespace XML.Service
{
    public class XMLService : IEnumerable<Catalog>
    {
        private XMLRepository repository;
        private FileStream stream;
        private XmlReader reader;
        private readonly Dictionary<Type, XmlSerializerNamespaces> namepsaces;

        private const string END_XML = "</Catalog>";

        public XMLService() : this(null, null)
        {

        }

        public XMLService(string pathFile) : this(pathFile, null)
        {

        }

        public XMLService(Catalog catalog) : this(null, catalog)
        {

        }

        public XMLService(string filePath, Catalog catalog)
        {
            XmlQualifiedName catalogName = new XmlQualifiedName("ctl", "http://LibraryStorage.Catalog/1.0.0.0");
            XmlQualifiedName bookName = new XmlQualifiedName("bk", "http://LibraryStorage.Book/1.0.0.0");
            XmlQualifiedName paperName = new XmlQualifiedName("npr", "http://LibraryStorage.Newspaper/1.0.0.0");
            XmlQualifiedName patentName = new XmlQualifiedName("ptn", "http://LibraryStorage.Patent/1.0.0.0");

            namepsaces = new Dictionary<Type, XmlSerializerNamespaces>
            {
                {
                    typeof(Catalog), new XmlSerializerNamespaces(new[] { catalogName, bookName, paperName, patentName })
                },
                {
                    typeof(Book), new XmlSerializerNamespaces(new[] { bookName })
                },
                {
                    typeof(Patent), new XmlSerializerNamespaces(new[] { patentName })
                },
                {
                    typeof(Newspaper), new XmlSerializerNamespaces(new[] { paperName })
                }
            };

            repository = String.IsNullOrWhiteSpace(filePath) ? new XMLRepository("DefaultName.xml") : new XMLRepository(filePath);

            if (catalog != null)
            {
                repository.Save(catalog);
            }

            stream = repository.Load();
            stream.Seek(0, SeekOrigin.Begin);
            reader = XmlReader.Create(stream, new XmlReaderSettings() { Async = true, IgnoreWhitespace = true });
            reader.ReadToFollowing("Catalog");
            reader.Read();
        }

        public bool AddEntity(IEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            stream.Position = stream.Length - END_XML.Length;
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings()
            {
                Indent = true,
                OmitXmlDeclaration = true
            }))
            {
                try
                {
                    var serializer = new XmlSerializer(entity.GetType());
                    serializer.Serialize(writer, entity, namepsaces[entity.GetType()]);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            var streamWriter = new StreamWriter(stream);
            streamWriter.Write($"\n{END_XML}");
            streamWriter.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            return true;
        }

        public IEnumerator<Catalog> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private struct Enumerator : IEnumerator<Catalog>
        {
            private readonly MemoryStream stream;
            private readonly XMLService xmlService;
            private XmlReader xmlReader;

            public Enumerator(XMLService service)
            {
                stream = new MemoryStream();
                long position = service.stream.Position;
                service.stream.Seek(0, SeekOrigin.Begin);
                service.stream.CopyTo(stream);
                service.stream.Position = position;
                this.xmlService = service;
                xmlReader = null;
                Reset();
            }

            public Catalog Current
            {
                get
                {
                    xmlReader.ReadSubtree();
                    Catalog entity = null;
                    switch (xmlReader.LocalName)
                    {
                        case "Patent":
                            entity = (Catalog)new XmlSerializer(typeof(Patent)).Deserialize(xmlReader);
                            break;
                        case "Newspaper":
                            entity = (Catalog)new XmlSerializer(typeof(Newspaper)).Deserialize(xmlReader);
                            break;
                        case "Book":
                            try
                            {
                                entity = (Catalog)new XmlSerializer(typeof(Book)).Deserialize(xmlReader);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                            break;
                    }

                    return entity;
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                stream.Dispose();
                xmlReader.Dispose();
            }

            public bool MoveNext()
            {
                while (!xmlReader.EOF)
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && (xmlReader.LocalName == "Book" || xmlReader.LocalName == "Patent" || xmlReader.LocalName == "Magazine"))
                    {
                        return true;
                    }
                    if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.LocalName == "Catalog")
                    {
                        return false;
                    }
                }

                return false;
            }

            public void Reset()
            {
                stream.Seek(0, SeekOrigin.Begin);
                xmlReader = XmlReader.Create(stream, new XmlReaderSettings() { Async = true, IgnoreWhitespace = true });
                xmlReader.ReadToFollowing("Catalog");
                xmlReader.Read();
            }
        }
    }
}
